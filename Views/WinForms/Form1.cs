using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EducationCentreSystem.Controllers;
using EducationCentreSystem.Models;
using EducationCentreSystem.Common;
using EducationCentreSystem.Repositories;

namespace EducationCentreSystem.Views.WinForms
{
    /// <summary>
    /// Optional WinForms UI for the education centre system.
    /// The coursework requires a console menu; this form provides a desktop UI on Windows as an extension.
    /// </summary>
    /// <remarks>
    /// This form reuses the same controller and domain model as the console UI.
    ///
    /// UI responsibilities:
    /// - Display a list of all records (DataGridView)
    /// - Provide filters, search, and sorting on the displayed list
    /// - Collect user input for add/edit operations
    /// - Present validation errors next to controls (ErrorProvider)
    ///
    /// Business rules (kept outside the UI):
    /// - Validation runs in request models and the controller
    /// - Repository implementation determines persistence (in-memory vs MySQL)
    /// </remarks>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Controller that exposes CRUD operations and hides storage details (in-memory vs MySQL).
        /// </summary>
        private PersonController _controller;

        /// <summary>
        /// Tracks whether the form is currently editing an existing record or adding a new one.
        /// </summary>
        private bool _isEditMode = false;

        /// <summary>
        /// Email used to identify the record being edited (email is the unique key in this application).
        /// </summary>
        private string _editTargetEmail = string.Empty;

        /// <summary>
        /// Initializes the form and loads initial data into the grid.
        /// </summary>
        public Form1()
            : this(new PersonController(new InMemoryPersonRepository()))
        {
        }

        public Form1(PersonController controller)
        {
            _controller = controller;
            InitializeComponent();
            SetupUI();
            LoadData();
        }

        /// <summary>
        /// One-time UI setup: populate dropdowns and configure the DataGridView columns.
        /// </summary>
        private void SetupUI()
        {
            // --- Role & filter controls ---
            // Role dropdown is bound to the PersonRole enum so UI stays consistent with the model.
            cmbRole.DataSource = Enum.GetValues(typeof(PersonRole));

            // Filter dropdown includes an "All" option plus each enum value.
            cmbFilterRole.Items.Add("All");
            foreach (PersonRole role in Enum.GetValues(typeof(PersonRole)))
            {
                cmbFilterRole.Items.Add(role);
            }
            cmbFilterRole.SelectedIndex = 0;
            
            // Admin job type options (normalized values used by validation).
            cmbJobType.Items.Add("Full-time");
            cmbJobType.Items.Add("Part-time");
            cmbJobType.SelectedIndex = 0;

            // --- Search & sort controls ---
            // Default selections for search/sort.
            cmbSearchBy.SelectedIndex = 0;
            cmbSortBy.SelectedIndex = 0;

            // --- Initial layout state ---
            // Start in "view mode" (details panel hidden).
            SetFormState(false);
            
            // --- Data grid configuration ---
            // Grid is configured manually (AutoGenerateColumns = false) to control layout and ordering.
            dgvPersons.Columns.Clear();
            dgvPersons.AutoGenerateColumns = false;
            
            // Common columns shared by all roles.
            dgvPersons.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", FillWeight = 30 });
            dgvPersons.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Role", HeaderText = "Role", FillWeight = 50 });
            dgvPersons.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name", FillWeight = 100 });
            dgvPersons.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", FillWeight = 120 });
            dgvPersons.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Telephone", HeaderText = "Telephone", FillWeight = 80 });
            
            // This column is computed at runtime to show role-specific information in one place.
            DataGridViewTextBoxColumn detailsCol = new DataGridViewTextBoxColumn { HeaderText = "Detailed Info", FillWeight = 200, Name = "DetailedInfo" };
            dgvPersons.Columns.Add(detailsCol);
            
            // Remove previous handler to avoid duplicate attachments
            dgvPersons.CellFormatting -= DgvPersons_CellFormatting;
            dgvPersons.CellFormatting += DgvPersons_CellFormatting;
        }

        /// <summary>
        /// Formats the computed "Detailed Info" column based on the runtime type of each row.
        /// Demonstrates polymorphism: each row is bound as a Person but may be Student/Teacher/Admin.
        /// </summary>
        private void DgvPersons_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            // Only handle formatting for the computed "DetailedInfo" column.
            if (e.RowIndex >= 0 && dgvPersons.Columns[e.ColumnIndex].Name == "DetailedInfo")
            {
                if (dgvPersons.Rows[e.RowIndex].DataBoundItem is Person)
                {
                    Person person = (Person)dgvPersons.Rows[e.RowIndex].DataBoundItem;
                    // Role-specific display using type checks.
                    if (person is Student) 
                    {
                        Student s = (Student)person;
                        // Student: show three subjects.
                        e.Value = "Subjects: " + s.Subject1 + ", " + s.Subject2 + ", " + s.Subject3;
                        e.FormattingApplied = true;
                    }
                    else if (person is Teacher) 
                    {
                        Teacher t = (Teacher)person;
                        // Teacher: show salary and two subjects.
                        e.Value = "Salary: $" + t.Salary.ToString("0.00") + ", Subjects: " + t.Subject1 + ", " + t.Subject2;
                        e.FormattingApplied = true;
                    }
                    else if (person is Admin) 
                    {
                        Admin a = (Admin)person;
                        // Admin: show salary, job type, and working hours.
                        e.Value = "Salary: $" + a.Salary.ToString("0.00") + ", Type: " + a.FullTimeOrPartTime + ", Hours: " + a.WorkingHours;
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        /// <summary>
        /// Loads data from the controller and applies UI filters (role, search, sort).
        /// The grid always binds to a list snapshot so filtering does not mutate stored data.
        /// </summary>
        private void LoadData()
        {
            IEnumerable<Person> query;

            // This method always starts from repository data, then applies UI transformations:
            // filter -> search -> sort.

            // 1. Filter by Role
            if (cmbFilterRole.SelectedItem?.ToString() == "All")
            {
                // "All" means no role filtering.
                query = _controller.ViewAll();
            }
            else
            {
                // Filter selection is stored as text; parse it back into the enum.
                PersonRole role;
                if (Enum.TryParse<PersonRole>(cmbFilterRole.SelectedItem?.ToString(), out role))
                {
                    query = _controller.ViewByRole(role);
                }
                else
                {
                    // Fallback safety: if parsing fails, show all records.
                    query = _controller.ViewAll();
                }
            }

            // 2. Search
            string searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                // SearchBy combo selects which field to search.
                if (cmbSearchBy.SelectedIndex == 0) // Search by Name
                {
                    // Case-insensitive substring search for usability.
                    query = query.Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (cmbSearchBy.SelectedIndex == 1) // Search by Email
                {
                    // Case-insensitive substring search for usability.
                    query = query.Where(p => p.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                }
            }

            // 3. Sort
            if (cmbSortBy.SelectedIndex == 1) // Name (A-Z)
            {
                // Stable sort by a single field.
                query = query.OrderBy(p => p.Name);
            }
            else if (cmbSortBy.SelectedIndex == 2) // Role
            {
                // Sort by role then by name for stable grouping.
                query = query.OrderBy(p => p.Role.ToString()).ThenBy(p => p.Name);
            }
            else
            {
                // Default sort: keep original order from repository.
            }

            // Materialize to a list so the DataGridView binds to a concrete collection.
            List<Person> list = query.ToList();
            dgvPersons.DataSource = list;

            if (list.Count == 0 && !string.IsNullOrEmpty(searchText))
            {
                // Optional: You can handle UI feedback here if needed, but MessageBox on every keystroke is bad UX.
            }
        }

        /// <summary>
        /// Live search: reloads data whenever the search text changes.
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Reloads data when the sort selection changes.
        /// </summary>
        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Reloads data when the role filter selection changes.
        /// </summary>
        private void cmbFilterRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Toggles between view mode and edit/add mode.
        /// When editing/adding, the grid and navigation controls are disabled to prevent conflicting actions.
        /// </summary>
        private void SetFormState(bool isEditingOrAdding)
        {
            // Details group becomes visible only when adding/editing.
            grpDetails.Visible = isEditingOrAdding;
            tlpMain.RowStyles[3].Height = isEditingOrAdding ? 232 : 0;
            
            // Disable navigation controls while editing/adding to keep workflow simple.
            dgvPersons.Enabled = !isEditingOrAdding;
            btnAdd.Enabled = !isEditingOrAdding;
            btnEdit.Enabled = !isEditingOrAdding;
            btnDelete.Enabled = !isEditingOrAdding;
            cmbFilterRole.Enabled = !isEditingOrAdding;
            
            if (!isEditingOrAdding)
            {
                // When returning to view mode, clear form inputs and reset edit flags.
                ClearForm();
            }
        }

        /// <summary>
        /// Clears all input fields and resets state to allow adding a new record.
        /// </summary>
        private void ClearForm()
        {
            // Reset text inputs.
            txtName.Clear();
            txtEmail.Clear();
            txtTelephone.Clear();
            txtSubject1.Clear();
            txtSubject2.Clear();
            txtSubject3.Clear();
            txtSalary.Clear();

            // Reset numeric inputs.
            numWorkingHours.Value = 0;

            // Reset internal state tracking.
            _isEditMode = false;
            _editTargetEmail = string.Empty;

            // Re-enable fields that are locked during edit mode.
            txtEmail.Enabled = true;
            cmbRole.Enabled = true;

            // Clear any visible validation errors.
            errorProvider1.Clear();
        }

        /// <summary>
        /// Enters "add mode" and shows the detail panel for input.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Add mode: show input panel and allow selecting role and entering email.
            SetFormState(true);
            _isEditMode = false;
            cmbRole.SelectedIndex = 0;
            txtName.Focus();
            UpdateVisibility();
        }

        /// <summary>
        /// Loads the selected record into the input fields and enters "edit mode".
        /// Email and role are locked during edit to keep the record identity consistent.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Edit requires a selected row in the grid.
            if (dgvPersons.SelectedRows.Count == 0)
            {
                MessageBox.Show("No records selected or no records found.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // DataBoundItem is the Person instance from the current grid row.
            var person = dgvPersons.SelectedRows[0].DataBoundItem as Person;
            if (person == null) return;

            SetFormState(true);
            _isEditMode = true;
            _editTargetEmail = person.Email;
            
            // Lock role and email so the identity and type of the record remain consistent during editing.
            cmbRole.SelectedItem = person.Role;
            cmbRole.Enabled = false;
            txtEmail.Text = person.Email;
            txtEmail.Enabled = false;
            txtName.Text = person.Name;
            txtTelephone.Text = person.Telephone;

            if (person is Student s)
            {
                // Populate student subject fields.
                txtSubject1.Text = s.Subject1;
                txtSubject2.Text = s.Subject2;
                txtSubject3.Text = s.Subject3;
            }
            else if (person is Teacher t)
            {
                // Populate teacher salary and subject fields.
                txtSubject1.Text = t.Subject1;
                txtSubject2.Text = t.Subject2;
                txtSalary.Text = t.Salary.ToString();
            }
            else if (person is Admin a)
            {
                // Populate admin employment fields.
                txtSalary.Text = a.Salary.ToString();
                cmbJobType.SelectedItem = a.FullTimeOrPartTime;
                numWorkingHours.Value = a.WorkingHours;
            }

            txtName.Focus();
            UpdateVisibility();
        }

        /// <summary>
        /// Deletes the selected record after confirmation.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete requires a selected row in the grid.
            if (dgvPersons.SelectedRows.Count == 0)
            {
                MessageBox.Show("No records selected or no records found.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var person = dgvPersons.SelectedRows[0].DataBoundItem as Person;
            if (person == null) return;

            // Confirmation dialog prevents accidental deletion.
            var confirm = MessageBox.Show($"Are you sure you want to permanently delete {person.Name}?\n(Y/N)", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                // Controller performs validation and deletion in the repository.
                var result = _controller.Delete(person.Email);
                if (result.Success)
                {
                    // Refresh list after successful delete.
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Saves changes by creating either an UpdatePersonRequest (edit mode) or a CreatePersonRequest (add mode).
        /// Validation runs before calling the controller, and UI errors are mapped via ErrorProvider.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Clear previous UI errors on every save attempt.
            errorProvider1.Clear();

            // Salary is optional depending on role and mode; parse if provided.
            decimal? salary = null;
            if (decimal.TryParse(txtSalary.Text, out decimal parsedSalary))
            {
                salary = parsedSalary;
            }
            else
            {
                // If parsing fails, salary stays null; validation will decide whether this is acceptable.
            }
            
            if (_isEditMode)
            {
                // Update uses TargetEmail to find the record and applies only provided fields.
                UpdatePersonRequest req = UpdatePersonRequest.Create(
                    targetEmail: _editTargetEmail,
                    name: txtName.Text,
                    telephone: txtTelephone.Text,
                    subject1: txtSubject1.Text,
                    subject2: txtSubject2.Text,
                    subject3: txtSubject3.Text,
                    salary: salary,
                    fullTimeOrPartTime: cmbJobType.SelectedItem?.ToString() ?? "",
                    workingHours: numWorkingHours.Value > 0 ? (int)numWorkingHours.Value : null
                );

                // Notes about update semantics:
                // - Blank strings are interpreted by the controller as "keep current" for some fields.
                // - Nullable Salary/WorkingHours allow "no change" when left empty/zero.
                // - Job type is validated only when provided.

                // Validate request and display field-level errors in the UI.
                Dictionary<string, string> validationErrors = req.Validate();
                if (validationErrors.Count > 0)
                {
                    MapErrorsToUI(validationErrors);
                    return;
                }

                // Controller performs repository update; UI reacts to success/failure.
                OperationResult res = _controller.Edit(req);
                if (res.Success)
                {
                    // Exit edit mode and refresh list.
                    SetFormState(false);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(res.Error, "Backend Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Create requires the role to determine which derived type is created.
                if (cmbRole.SelectedItem is PersonRole)
                {
                    PersonRole role = (PersonRole)cmbRole.SelectedItem;
                    // Create always includes the common fields and may include role-specific fields.
                    // Validation enforces which fields are required for each role.
                    CreatePersonRequest req = CreatePersonRequest.Create(
                        role: role,
                        name: txtName.Text,
                        email: txtEmail.Text,
                        telephone: txtTelephone.Text,
                        subject1: txtSubject1.Text,
                        subject2: txtSubject2.Text,
                        subject3: txtSubject3.Text,
                        salary: salary ?? 0,
                        fullTimeOrPartTime: cmbJobType.SelectedItem?.ToString() ?? "",
                        workingHours: (int)numWorkingHours.Value
                    );

                    // Notes about create semantics:
                    // - Email is required and must be unique (checked by the controller).
                    // - Salary is required for Teacher/Admin; for Student it may remain 0 and is ignored.
                    // - WorkingHours and job type are only required for Admin.

                    // Validate request and display field-level errors in the UI.
                    Dictionary<string, string> validationErrors = req.Validate();
                    if (validationErrors.Count > 0)
                    {
                        MapErrorsToUI(validationErrors);
                        return;
                    }

                    // Controller performs repository insert and checks unique email.
                    OperationResult<Person> res = _controller.Add(req);
                    if (res.Success)
                    {
                        // Exit add mode and refresh list.
                        SetFormState(false);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show(res.Error, "Backend Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Role is always selected from a dropdown; this guard avoids unexpected nulls.
                }
            }
        }

        /// <summary>
        /// Maps validation errors (field name -> message) to UI controls using ErrorProvider.
        /// This keeps validation logic in request models while the UI only handles presentation.
        /// </summary>
        private void MapErrorsToUI(Dictionary<string, string> errors)
        {
            // Field keys come from nameof(...) in validation logic.
            foreach (KeyValuePair<string, string> error in errors)
            {
                Control control = null;
                switch (error.Key)
                {
                    case "Name": control = txtName; break;
                    case "Email": control = txtEmail; break;
                    case "Telephone": control = txtTelephone; break;
                    case "Salary": control = txtSalary; break;
                    case "FullTimeOrPartTime": control = cmbJobType; break;
                    case "WorkingHours": control = numWorkingHours; break;
                }

                if (control != null)
                {
                    // ErrorProvider displays a small icon with the message near the control.
                    errorProvider1.SetError(control, error.Value);
                }
            }
        }

        /// <summary>
        /// Cancels the current edit/add operation and returns to view mode.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancel discards user input and leaves data unchanged.
            errorProvider1.Clear();
            SetFormState(false);
        }

        /// <summary>
        /// Updates which input panels are visible based on the selected role.
        /// </summary>
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        /// <summary>
        /// Shows and hides group-specific fields to match the coursework specification:
        /// Student: 3 subjects; Teacher: salary + 2 subjects; Admin: salary + job type + working hours.
        /// </summary>
        private void UpdateVisibility()
        {
            if (cmbRole.SelectedItem is PersonRole)
            {
                PersonRole role = (PersonRole)cmbRole.SelectedItem;
                bool isStudent = role == PersonRole.Student;
                bool isTeacher = role == PersonRole.Teacher;
                bool isAdmin = role == PersonRole.Admin;

                // Subject panels: Student uses 3, Teacher uses 2, Admin uses none.
                pnlSubject1.Visible = isStudent || isTeacher;
                pnlSubject2.Visible = isStudent || isTeacher;
                pnlSubject3.Visible = isStudent;
                
                // Admin/Teacher panels.
                pnlSalary.Visible = isTeacher || isAdmin;
                pnlJobType.Visible = isAdmin;
                pnlWorkingHours.Visible = isAdmin;
            }
            else
            {
                // Defensive default: if role isn't selected, hide group-specific panels.
                pnlSubject1.Visible = false;
                pnlSubject2.Visible = false;
                pnlSubject3.Visible = false;
                pnlSalary.Visible = false;
                pnlJobType.Visible = false;
                pnlWorkingHours.Visible = false;
            }
        }
    }
}
