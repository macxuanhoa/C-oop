namespace EducationCentreSystem.Views.WinForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvPersons = new System.Windows.Forms.DataGridView();
            this.cmbFilterRole = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.flpInputs = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlRole = new System.Windows.Forms.Panel();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.pnlName = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.pnlEmail = new System.Windows.Forms.Panel();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.pnlTelephone = new System.Windows.Forms.Panel();
            this.lblTelephone = new System.Windows.Forms.Label();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.pnlSubject1 = new System.Windows.Forms.Panel();
            this.lblSubject1 = new System.Windows.Forms.Label();
            this.txtSubject1 = new System.Windows.Forms.TextBox();
            this.pnlSubject2 = new System.Windows.Forms.Panel();
            this.lblSubject2 = new System.Windows.Forms.Label();
            this.txtSubject2 = new System.Windows.Forms.TextBox();
            this.pnlSubject3 = new System.Windows.Forms.Panel();
            this.lblSubject3 = new System.Windows.Forms.Label();
            this.txtSubject3 = new System.Windows.Forms.TextBox();
            this.pnlSalary = new System.Windows.Forms.Panel();
            this.lblSalary = new System.Windows.Forms.Label();
            this.txtSalary = new System.Windows.Forms.TextBox();
            this.pnlJobType = new System.Windows.Forms.Panel();
            this.lblJobType = new System.Windows.Forms.Label();
            this.cmbJobType = new System.Windows.Forms.ComboBox();
            this.pnlWorkingHours = new System.Windows.Forms.Panel();
            this.lblWorkingHours = new System.Windows.Forms.Label();
            this.numWorkingHours = new System.Windows.Forms.NumericUpDown();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbSearchBy = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSort = new System.Windows.Forms.Label();
            this.cmbSortBy = new System.Windows.Forms.ComboBox();
            this.pnlMidActions = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersons)).BeginInit();
            this.grpDetails.SuspendLayout();
            this.flpInputs.SuspendLayout();
            this.pnlRole.SuspendLayout();
            this.pnlName.SuspendLayout();
            this.pnlEmail.SuspendLayout();
            this.pnlTelephone.SuspendLayout();
            this.pnlSubject1.SuspendLayout();
            this.pnlSubject2.SuspendLayout();
            this.pnlSubject3.SuspendLayout();
            this.pnlSalary.SuspendLayout();
            this.pnlJobType.SuspendLayout();
            this.pnlWorkingHours.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWorkingHours)).BeginInit();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tlpMain.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlMidActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPersons
            // 
            this.dgvPersons.AllowUserToAddRows = false;
            this.dgvPersons.AllowUserToDeleteRows = false;
            this.dgvPersons.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPersons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPersons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPersons.Location = new System.Drawing.Point(3, 38);
            this.dgvPersons.Name = "dgvPersons";
            this.dgvPersons.ReadOnly = true;
            this.dgvPersons.RowTemplate.Height = 25;
            this.dgvPersons.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPersons.Size = new System.Drawing.Size(828, 292);
            this.dgvPersons.TabIndex = 1;
            // 
            // cmbFilterRole
            // 
            this.cmbFilterRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterRole.FormattingEnabled = true;
            this.cmbFilterRole.Location = new System.Drawing.Point(88, 3);
            this.cmbFilterRole.Name = "cmbFilterRole";
            this.cmbFilterRole.Size = new System.Drawing.Size(150, 23);
            this.cmbFilterRole.TabIndex = 1;
            this.cmbFilterRole.SelectedIndexChanged += new System.EventHandler(this.cmbFilterRole_SelectedIndexChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(3, 7);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(79, 15);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "Filter by Role:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(110, 36);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "&Add New";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(119, 5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(110, 36);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "&Edit Selected";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(235, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 36);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // grpDetails
            // 
            this.grpDetails.Controls.Add(this.flpInputs);
            this.grpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetails.Location = new System.Drawing.Point(3, 372);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(828, 226);
            this.grpDetails.TabIndex = 3;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Person Information";
            // 
            // flpInputs
            // 
            this.flpInputs.AutoScroll = true;
            this.flpInputs.Controls.Add(this.pnlRole);
            this.flpInputs.Controls.Add(this.pnlName);
            this.flpInputs.Controls.Add(this.pnlEmail);
            this.flpInputs.Controls.Add(this.pnlTelephone);
            this.flpInputs.Controls.Add(this.pnlSubject1);
            this.flpInputs.Controls.Add(this.pnlSubject2);
            this.flpInputs.Controls.Add(this.pnlSubject3);
            this.flpInputs.Controls.Add(this.pnlSalary);
            this.flpInputs.Controls.Add(this.pnlJobType);
            this.flpInputs.Controls.Add(this.pnlWorkingHours);
            this.flpInputs.Controls.Add(this.pnlActions);
            this.flpInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpInputs.Location = new System.Drawing.Point(3, 19);
            this.flpInputs.Name = "flpInputs";
            this.flpInputs.Size = new System.Drawing.Size(822, 204);
            this.flpInputs.TabIndex = 0;
            // 
            // pnlRole
            // 
            this.pnlRole.Controls.Add(this.lblRole);
            this.pnlRole.Controls.Add(this.cmbRole);
            this.pnlRole.Location = new System.Drawing.Point(3, 3);
            this.pnlRole.Name = "pnlRole";
            this.pnlRole.Size = new System.Drawing.Size(280, 40);
            this.pnlRole.TabIndex = 0;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(3, 13);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(33, 15);
            this.lblRole.TabIndex = 0;
            this.lblRole.Text = "Role:";
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(95, 10);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(170, 23);
            this.cmbRole.TabIndex = 1;
            this.cmbRole.SelectedIndexChanged += new System.EventHandler(this.cmbRole_SelectedIndexChanged);
            // 
            // pnlName
            // 
            this.pnlName.Controls.Add(this.lblName);
            this.pnlName.Controls.Add(this.txtName);
            this.pnlName.Location = new System.Drawing.Point(289, 3);
            this.pnlName.Name = "pnlName";
            this.pnlName.Size = new System.Drawing.Size(280, 40);
            this.pnlName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(42, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(95, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(170, 23);
            this.txtName.TabIndex = 1;
            // 
            // pnlEmail
            // 
            this.pnlEmail.Controls.Add(this.lblEmail);
            this.pnlEmail.Controls.Add(this.txtEmail);
            this.pnlEmail.Location = new System.Drawing.Point(575, 3);
            this.pnlEmail.Name = "pnlEmail";
            this.pnlEmail.Size = new System.Drawing.Size(280, 40);
            this.pnlEmail.TabIndex = 2;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(3, 13);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(39, 15);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(95, 10);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(170, 23);
            this.txtEmail.TabIndex = 1;
            // 
            // pnlTelephone
            // 
            this.pnlTelephone.Controls.Add(this.lblTelephone);
            this.pnlTelephone.Controls.Add(this.txtTelephone);
            this.pnlTelephone.Location = new System.Drawing.Point(3, 49);
            this.pnlTelephone.Name = "pnlTelephone";
            this.pnlTelephone.Size = new System.Drawing.Size(280, 40);
            this.pnlTelephone.TabIndex = 3;
            // 
            // lblTelephone
            // 
            this.lblTelephone.AutoSize = true;
            this.lblTelephone.Location = new System.Drawing.Point(3, 13);
            this.lblTelephone.Name = "lblTelephone";
            this.lblTelephone.Size = new System.Drawing.Size(64, 15);
            this.lblTelephone.TabIndex = 0;
            this.lblTelephone.Text = "Telephone:";
            // 
            // txtTelephone
            // 
            this.txtTelephone.Location = new System.Drawing.Point(95, 10);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(170, 23);
            this.txtTelephone.TabIndex = 1;
            // 
            // pnlSubject1
            // 
            this.pnlSubject1.Controls.Add(this.lblSubject1);
            this.pnlSubject1.Controls.Add(this.txtSubject1);
            this.pnlSubject1.Location = new System.Drawing.Point(269, 49);
            this.pnlSubject1.Name = "pnlSubject1";
            this.pnlSubject1.Size = new System.Drawing.Size(280, 40);
            this.pnlSubject1.TabIndex = 4;
            // 
            // lblSubject1
            // 
            this.lblSubject1.AutoSize = true;
            this.lblSubject1.Location = new System.Drawing.Point(3, 13);
            this.lblSubject1.Name = "lblSubject1";
            this.lblSubject1.Size = new System.Drawing.Size(58, 15);
            this.lblSubject1.TabIndex = 0;
            this.lblSubject1.Text = "Subject 1:";
            // 
            // txtSubject1
            // 
            this.txtSubject1.Location = new System.Drawing.Point(95, 10);
            this.txtSubject1.Name = "txtSubject1";
            this.txtSubject1.Size = new System.Drawing.Size(170, 23);
            this.txtSubject1.TabIndex = 1;
            // 
            // pnlSubject2
            // 
            this.pnlSubject2.Controls.Add(this.lblSubject2);
            this.pnlSubject2.Controls.Add(this.txtSubject2);
            this.pnlSubject2.Location = new System.Drawing.Point(535, 49);
            this.pnlSubject2.Name = "pnlSubject2";
            this.pnlSubject2.Size = new System.Drawing.Size(280, 40);
            this.pnlSubject2.TabIndex = 5;
            // 
            // lblSubject2
            // 
            this.lblSubject2.AutoSize = true;
            this.lblSubject2.Location = new System.Drawing.Point(3, 13);
            this.lblSubject2.Name = "lblSubject2";
            this.lblSubject2.Size = new System.Drawing.Size(58, 15);
            this.lblSubject2.TabIndex = 0;
            this.lblSubject2.Text = "Subject 2:";
            // 
            // txtSubject2
            // 
            this.txtSubject2.Location = new System.Drawing.Point(95, 10);
            this.txtSubject2.Name = "txtSubject2";
            this.txtSubject2.Size = new System.Drawing.Size(170, 23);
            this.txtSubject2.TabIndex = 1;
            // 
            // pnlSubject3
            // 
            this.pnlSubject3.Controls.Add(this.lblSubject3);
            this.pnlSubject3.Controls.Add(this.txtSubject3);
            this.pnlSubject3.Location = new System.Drawing.Point(3, 95);
            this.pnlSubject3.Name = "pnlSubject3";
            this.pnlSubject3.Size = new System.Drawing.Size(280, 40);
            this.pnlSubject3.TabIndex = 6;
            // 
            // lblSubject3
            // 
            this.lblSubject3.AutoSize = true;
            this.lblSubject3.Location = new System.Drawing.Point(3, 13);
            this.lblSubject3.Name = "lblSubject3";
            this.lblSubject3.Size = new System.Drawing.Size(58, 15);
            this.lblSubject3.TabIndex = 0;
            this.lblSubject3.Text = "Subject 3:";
            // 
            // txtSubject3
            // 
            this.txtSubject3.Location = new System.Drawing.Point(95, 10);
            this.txtSubject3.Name = "txtSubject3";
            this.txtSubject3.Size = new System.Drawing.Size(170, 23);
            this.txtSubject3.TabIndex = 1;
            // 
            // pnlSalary
            // 
            this.pnlSalary.Controls.Add(this.lblSalary);
            this.pnlSalary.Controls.Add(this.txtSalary);
            this.pnlSalary.Location = new System.Drawing.Point(269, 95);
            this.pnlSalary.Name = "pnlSalary";
            this.pnlSalary.Size = new System.Drawing.Size(280, 40);
            this.pnlSalary.TabIndex = 7;
            // 
            // lblSalary
            // 
            this.lblSalary.AutoSize = true;
            this.lblSalary.Location = new System.Drawing.Point(3, 13);
            this.lblSalary.Name = "lblSalary";
            this.lblSalary.Size = new System.Drawing.Size(41, 15);
            this.lblSalary.TabIndex = 0;
            this.lblSalary.Text = "Salary:";
            // 
            // txtSalary
            // 
            this.txtSalary.Location = new System.Drawing.Point(95, 10);
            this.txtSalary.Name = "txtSalary";
            this.txtSalary.Size = new System.Drawing.Size(170, 23);
            this.txtSalary.TabIndex = 1;
            // 
            // pnlJobType
            // 
            this.pnlJobType.Controls.Add(this.lblJobType);
            this.pnlJobType.Controls.Add(this.cmbJobType);
            this.pnlJobType.Location = new System.Drawing.Point(535, 95);
            this.pnlJobType.Name = "pnlJobType";
            this.pnlJobType.Size = new System.Drawing.Size(280, 40);
            this.pnlJobType.TabIndex = 8;
            // 
            // lblJobType
            // 
            this.lblJobType.AutoSize = true;
            this.lblJobType.Location = new System.Drawing.Point(3, 13);
            this.lblJobType.Name = "lblJobType";
            this.lblJobType.Size = new System.Drawing.Size(56, 15);
            this.lblJobType.TabIndex = 0;
            this.lblJobType.Text = "Job Type:";
            // 
            // cmbJobType
            // 
            this.cmbJobType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJobType.FormattingEnabled = true;
            this.cmbJobType.Location = new System.Drawing.Point(95, 10);
            this.cmbJobType.Name = "cmbJobType";
            this.cmbJobType.Size = new System.Drawing.Size(170, 23);
            this.cmbJobType.TabIndex = 1;
            // 
            // pnlWorkingHours
            // 
            this.pnlWorkingHours.Controls.Add(this.lblWorkingHours);
            this.pnlWorkingHours.Controls.Add(this.numWorkingHours);
            this.pnlWorkingHours.Location = new System.Drawing.Point(3, 141);
            this.pnlWorkingHours.Name = "pnlWorkingHours";
            this.pnlWorkingHours.Size = new System.Drawing.Size(280, 40);
            this.pnlWorkingHours.TabIndex = 9;
            // 
            // lblWorkingHours
            // 
            this.lblWorkingHours.AutoSize = true;
            this.lblWorkingHours.Location = new System.Drawing.Point(3, 13);
            this.lblWorkingHours.Name = "lblWorkingHours";
            this.lblWorkingHours.Size = new System.Drawing.Size(42, 15);
            this.lblWorkingHours.TabIndex = 0;
            this.lblWorkingHours.Text = "Hours:";
            // 
            // numWorkingHours
            // 
            this.numWorkingHours.Location = new System.Drawing.Point(95, 11);
            this.numWorkingHours.Maximum = new decimal(new int[] {
            168,
            0,
            0,
            0});
            this.numWorkingHours.Name = "numWorkingHours";
            this.numWorkingHours.Size = new System.Drawing.Size(170, 23);
            this.numWorkingHours.TabIndex = 1;
            // 
            // pnlActions
            // 
            this.flpInputs.SetFlowBreak(this.pnlActions, true);
            this.pnlActions.Controls.Add(this.btnSave);
            this.pnlActions.Controls.Add(this.btnCancel);
            this.pnlActions.Location = new System.Drawing.Point(269, 141);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(526, 40);
            this.pnlActions.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(310, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 34);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save Changes";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(436, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 34);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.pnlTop, 0, 0);
            this.tlpMain.Controls.Add(this.dgvPersons, 0, 1);
            this.tlpMain.Controls.Add(this.pnlMidActions, 0, 2);
            this.tlpMain.Controls.Add(this.grpDetails, 0, 3);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 232F));
            this.tlpMain.Size = new System.Drawing.Size(934, 641);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblFilter);
            this.pnlTop.Controls.Add(this.cmbFilterRole);
            this.pnlTop.Controls.Add(this.cmbSearchBy);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblSort);
            this.pnlTop.Controls.Add(this.cmbSortBy);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(3, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(928, 39);
            this.pnlTop.TabIndex = 0;
            // 
            // cmbSearchBy
            // 
            this.cmbSearchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchBy.FormattingEnabled = true;
            this.cmbSearchBy.Items.AddRange(new object[] {
            "Search by Name",
            "Search by Email"});
            this.cmbSearchBy.Location = new System.Drawing.Point(215, 3);
            this.cmbSearchBy.Name = "cmbSearchBy";
            this.cmbSearchBy.Size = new System.Drawing.Size(120, 23);
            this.cmbSearchBy.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(341, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 23);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSort
            // 
            this.lblSort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSort.AutoSize = true;
            this.lblSort.Location = new System.Drawing.Point(497, 7);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(47, 15);
            this.lblSort.TabIndex = 4;
            this.lblSort.Text = "Sort by:";
            // 
            // cmbSortBy
            // 
            this.cmbSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortBy.FormattingEnabled = true;
            this.cmbSortBy.Items.AddRange(new object[] {
            "None",
            "Name (A-Z)",
            "Role"});
            this.cmbSortBy.Location = new System.Drawing.Point(550, 3);
            this.cmbSortBy.Name = "cmbSortBy";
            this.cmbSortBy.Size = new System.Drawing.Size(120, 23);
            this.cmbSortBy.TabIndex = 5;
            this.cmbSortBy.SelectedIndexChanged += new System.EventHandler(this.cmbSortBy_SelectedIndexChanged);
            // 
            // pnlMidActions
            // 
            this.pnlMidActions.Controls.Add(this.btnAdd);
            this.pnlMidActions.Controls.Add(this.btnEdit);
            this.pnlMidActions.Controls.Add(this.btnDelete);
            this.pnlMidActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMidActions.Location = new System.Drawing.Point(3, 336);
            this.pnlMidActions.Name = "pnlMidActions";
            this.pnlMidActions.Size = new System.Drawing.Size(928, 44);
            this.pnlMidActions.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 641);
            this.Controls.Add(this.tlpMain);
            this.MinimumSize = new System.Drawing.Size(950, 680);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Education Centre System - Advanced Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersons)).EndInit();
            this.grpDetails.ResumeLayout(false);
            this.flpInputs.ResumeLayout(false);
            this.pnlRole.ResumeLayout(false);
            this.pnlRole.PerformLayout();
            this.pnlName.ResumeLayout(false);
            this.pnlName.PerformLayout();
            this.pnlEmail.ResumeLayout(false);
            this.pnlEmail.PerformLayout();
            this.pnlTelephone.ResumeLayout(false);
            this.pnlTelephone.PerformLayout();
            this.pnlSubject1.ResumeLayout(false);
            this.pnlSubject1.PerformLayout();
            this.pnlSubject2.ResumeLayout(false);
            this.pnlSubject2.PerformLayout();
            this.pnlSubject3.ResumeLayout(false);
            this.pnlSubject3.PerformLayout();
            this.pnlSalary.ResumeLayout(false);
            this.pnlSalary.PerformLayout();
            this.pnlJobType.ResumeLayout(false);
            this.pnlJobType.PerformLayout();
            this.pnlWorkingHours.ResumeLayout(false);
            this.pnlWorkingHours.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWorkingHours)).EndInit();
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tlpMain.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMidActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.DataGridView dgvPersons;
        private System.Windows.Forms.ComboBox cmbFilterRole;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.TextBox txtSubject3;
        private System.Windows.Forms.Label lblSubject3;
        private System.Windows.Forms.TextBox txtSubject2;
        private System.Windows.Forms.Label lblSubject2;
        private System.Windows.Forms.TextBox txtSubject1;
        private System.Windows.Forms.Label lblSubject1;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.Label lblTelephone;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblSalary;
        private System.Windows.Forms.NumericUpDown numWorkingHours;
        private System.Windows.Forms.Label lblWorkingHours;
        private System.Windows.Forms.ComboBox cmbJobType;
        private System.Windows.Forms.Label lblJobType;
        private System.Windows.Forms.TextBox txtSalary;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.FlowLayoutPanel pnlTop;
        private System.Windows.Forms.ComboBox cmbSearchBy;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSort;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.FlowLayoutPanel pnlMidActions;
        private System.Windows.Forms.FlowLayoutPanel flpInputs;
        private System.Windows.Forms.Panel pnlRole;
        private System.Windows.Forms.Panel pnlName;
        private System.Windows.Forms.Panel pnlEmail;
        private System.Windows.Forms.Panel pnlTelephone;
        private System.Windows.Forms.Panel pnlSubject1;
        private System.Windows.Forms.Panel pnlSubject2;
        private System.Windows.Forms.Panel pnlSubject3;
        private System.Windows.Forms.Panel pnlSalary;
        private System.Windows.Forms.Panel pnlJobType;
        private System.Windows.Forms.Panel pnlWorkingHours;
        private System.Windows.Forms.Panel pnlActions;
    }
}
