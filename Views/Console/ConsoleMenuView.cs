using EducationCentreSystem.Controllers;
using EducationCentreSystem.Models;

namespace EducationCentreSystem.Views.Console;

/// <summary>
/// Text-based menu UI (console application).
/// Provides user access to CRUD operations via PersonController.
/// </summary>
/// <remarks>
/// Coursework features implemented through this menu:
/// 1) Add new data (Student/Teacher/Admin)
/// 2) View all existing data
/// 3) View existing data by user group
/// 4) Edit existing data (identified by email)
/// 5) Delete existing data (identified by email)
///
/// Notes:
/// - Email is treated as the identifier for find/edit/delete operations.
/// - Input is validated in request models and the controller; the menu focuses on prompting and display.
/// - Polymorphism is visible when printing records: GetDetails() is overridden in derived classes.
/// </remarks>
public sealed class ConsoleMenuView
{
    private readonly PersonController _controller;

    /// <summary>
    /// Creates the console view with a controller dependency.
    /// </summary>
    public ConsoleMenuView(PersonController controller)
    {
        _controller = controller;
    }

    /// <summary>
    /// Displays the main menu loop until the user chooses to exit.
    /// </summary>
    public void Run()
    {
        // Console UI is implemented as a loop so the user can perform multiple actions in one session.
        var exit = false;
        while (!exit)
        {
            // Main menu options (coursework requirements).
            System.Console.WriteLine("\n--- EDUCATION CENTRE MANAGEMENT SYSTEM ---");
            System.Console.WriteLine("1 Add record");
            System.Console.WriteLine("2 View all records");
            System.Console.WriteLine("3 View records by role");
            System.Console.WriteLine("4 Edit record");
            System.Console.WriteLine("5 Delete record");
            System.Console.WriteLine("6 Exit");
            System.Console.Write("Enter choice (1-6): ");

            // ReadLine returns null on input stream end; null falls into default case below.
            var choice = System.Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Create a new record (Student/Teacher/Admin).
                    AddRecord();
                    break;
                case "2":
                    // List all records across all roles.
                    ViewAll();
                    break;
                case "3":
                    // List records filtered by role.
                    ViewByRole();
                    break;
                case "4":
                    // Update an existing record identified by email.
                    EditRecord();
                    break;
                case "5":
                    // Delete an existing record identified by email.
                    DeleteRecord();
                    break;
                case "6":
                    // Exit ends the loop and returns control to Program.Main.
                    exit = true;
                    break;
                default:
                    // Any non-recognized input returns to the menu.
                    System.Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    /// <summary>
    /// Prints every record currently stored, regardless of role.
    /// </summary>
    private void ViewAll()
    {
        // Controller returns a snapshot list so the UI never manipulates repository state directly.
        var all = _controller.ViewAll();
        if (all.Count == 0)
        {
            System.Console.WriteLine("No records found.");
            return;
        }

        System.Console.WriteLine("\n--- ALL RECORDS ---");
        // Polymorphism: GetDetails() calls the overridden implementation for Student/Teacher/Admin.
        foreach (var p in all) System.Console.WriteLine(p.GetDetails());
    }

    /// <summary>
    /// Prompts the user to choose a role and prints only records of that role.
    /// </summary>
    private void ViewByRole()
    {
        // Simple numeric menu is used to reduce typing and avoid enum parsing issues.
        System.Console.WriteLine("\nFilter by role:");
        System.Console.WriteLine("1. Student");
        System.Console.WriteLine("2. Teacher");
        System.Console.WriteLine("3. Admin");
        System.Console.Write("Enter choice (1/2/3): ");
        var roleChoice = System.Console.ReadLine();

        // Map numeric choice to enum. Null indicates invalid choice.
        PersonRole? role = roleChoice == "1" ? PersonRole.Student :
            roleChoice == "2" ? PersonRole.Teacher :
            roleChoice == "3" ? PersonRole.Admin : null;

        if (role == null)
        {
            System.Console.WriteLine("Invalid role.");
            return;
        }

        // Role filtering is delegated to the controller/repository layer.
        var list = _controller.ViewByRole(role.Value);
        if (list.Count == 0)
        {
            System.Console.WriteLine("No records found for this role.");
            return;
        }

        System.Console.WriteLine($"\n--- {role.Value.ToString().ToUpperInvariant()} LIST ---");
        foreach (var p in list) System.Console.WriteLine(p.GetDetails());
    }

    /// <summary>
    /// Adds a new record by collecting common fields and then role-specific fields.
    /// </summary>
    private void AddRecord()
    {
        // Role selection determines which group-specific fields are collected.
        System.Console.WriteLine("\nSelect role to add:");
        System.Console.WriteLine("1. Student");
        System.Console.WriteLine("2. Teacher");
        System.Console.WriteLine("3. Admin");
        System.Console.Write("Enter choice (1/2/3): ");
        var roleChoice = System.Console.ReadLine();

        // Same mapping approach as ViewByRole() to keep UI consistent.
        PersonRole? role = roleChoice == "1" ? PersonRole.Student :
            roleChoice == "2" ? PersonRole.Teacher :
            roleChoice == "3" ? PersonRole.Admin : null;

        if (role == null)
        {
            System.Console.WriteLine("Invalid role.");
            return;
        }

        // Common fields required for all roles.
        System.Console.Write("Name: ");
        var name = System.Console.ReadLine() ?? string.Empty;
        System.Console.Write("Telephone: ");
        var telephone = System.Console.ReadLine() ?? string.Empty;
        System.Console.Write("Email: ");
        var email = System.Console.ReadLine() ?? string.Empty;

        // Group-specific input according to the coursework specification.
        string subject1 = "";
        string subject2 = "";
        string subject3 = "";
        decimal salary = 0;
        string jobType = "";
        int hours = 0;

        if (role == PersonRole.Student)
        {
            // Students store exactly three subject names.
            System.Console.Write("Subject 1: ");
            subject1 = System.Console.ReadLine() ?? string.Empty;
            System.Console.Write("Subject 2: ");
            subject2 = System.Console.ReadLine() ?? string.Empty;
            System.Console.Write("Subject 3: ");
            subject3 = System.Console.ReadLine() ?? string.Empty;
        }
        else if (role == PersonRole.Teacher)
        {
            // Teachers have salary and two subject names.
            salary = ReadDecimal("Salary");
            System.Console.Write("Subject 1: ");
            subject1 = System.Console.ReadLine() ?? string.Empty;
            System.Console.Write("Subject 2: ");
            subject2 = System.Console.ReadLine() ?? string.Empty;
        }
        else if (role == PersonRole.Admin)
        {
            // Admin staff have salary, job type, and working hours.
            salary = ReadDecimal("Salary");
            System.Console.Write("FullTimeOrPartTime: ");
            jobType = System.Console.ReadLine() ?? string.Empty;
            hours = ReadInt("WorkingHours");
        }

        // Request object is validated inside the controller before persistence.
        var request = CreatePersonRequest.Create(
            role: role.Value,
            name: name,
            telephone: telephone,
            email: email,
            subject1: subject1,
            subject2: subject2,
            subject3: subject3,
            salary: salary,
            fullTimeOrPartTime: jobType,
            workingHours: hours
        );

        // Controller validates input and persists the record.
        var result = _controller.Add(request);
        System.Console.WriteLine(result.Success ? "Added successfully." : result.Error);
    }

    /// <summary>
    /// Updates an existing record identified by email.
    /// Blank input keeps the current value.
    /// </summary>
    private void EditRecord()
    {
        // Email is the identifier used for lookup and update in this project.
        System.Console.Write("\nEnter Email of the record to edit: ");
        var email = System.Console.ReadLine() ?? string.Empty;

        // Preload the record so the UI can show current values.
        var existing = _controller.FindByEmail(email);
        if (existing == null)
        {
            System.Console.WriteLine("Record not found.");
            return;
        }

        // Existing record is used only for displaying current values.
        System.Console.WriteLine("Leave blank to keep current value.");
        System.Console.Write($"Name ({existing.Name}): ");
        var name = System.Console.ReadLine() ?? string.Empty;
        System.Console.Write($"Telephone ({existing.Telephone}): ");
        var telephone = System.Console.ReadLine() ?? string.Empty;

        // Group-specific input variables
        string subject1 = "";
        string subject2 = "";
        string subject3 = "";
        decimal? salary = null;
        string jobType = "";
        int? hours = null;

        // Only prompt for fields that match the existing runtime type.
        if (existing is Student s)
        {
            // Student update supports changing any of the three subjects.
            System.Console.Write($"Subject1 ({s.Subject1}): ");
            subject1 = System.Console.ReadLine() ?? string.Empty;
            System.Console.Write($"Subject2 ({s.Subject2}): ");
            subject2 = System.Console.ReadLine() ?? string.Empty;
            System.Console.Write($"Subject3 ({s.Subject3}): ");
            subject3 = System.Console.ReadLine() ?? string.Empty;
        }
        else if (existing is Teacher t)
        {
            // Teacher update supports salary and two subjects.
            salary = ReadDecimalNullable($"Salary ({t.Salary})");
            System.Console.Write($"Subject1 ({t.Subject1}): ");
            subject1 = System.Console.ReadLine() ?? string.Empty;
            System.Console.Write($"Subject2 ({t.Subject2}): ");
            subject2 = System.Console.ReadLine() ?? string.Empty;
        }
        else if (existing is Admin a)
        {
            // Admin update supports salary, job type, and working hours.
            salary = ReadDecimalNullable($"Salary ({a.Salary})");
            System.Console.Write($"FullTimeOrPartTime ({a.FullTimeOrPartTime}): ");
            jobType = System.Console.ReadLine() ?? string.Empty;
            hours = ReadIntNullable($"WorkingHours ({a.WorkingHours})");
        }

        // Update request supports optional fields (nullable / blank means no change).
        var request = UpdatePersonRequest.Create(
            targetEmail: email,
            name: name,
            telephone: telephone,
            subject1: subject1,
            subject2: subject2,
            subject3: subject3,
            salary: salary,
            fullTimeOrPartTime: jobType,
            workingHours: hours
        );

        // Controller updates only the fields provided in the request.
        var result = _controller.Edit(request);
        System.Console.WriteLine(result.Success ? "Updated successfully." : result.Error);
    }

    /// <summary>
    /// Deletes a record by email.
    /// </summary>
    private void DeleteRecord()
    {
        // Deletion is delegated to the controller to ensure consistent validation and error messages.
        System.Console.Write("\nEnter Email of the record to delete: ");
        var email = System.Console.ReadLine() ?? string.Empty;
        var result = _controller.Delete(email);
        System.Console.WriteLine(result.Success ? "Deleted successfully." : result.Error);
    }

    /// <summary>
    /// Reads an integer from the console, repeating until valid input is provided.
    /// Used for required numeric fields.
    /// </summary>
    private static int ReadInt(string label)
    {
        // Input loop ensures the application keeps prompting until a valid integer is provided.
        while (true)
        {
            System.Console.Write($"{label}: ");
            var text = System.Console.ReadLine();
            if (int.TryParse(text, out var value)) return value;
            // Invalid input: repeat prompt until user enters a valid integer.
            System.Console.WriteLine("Invalid number.");
        }
    }

    /// <summary>
    /// Reads an optional integer from the console.
    /// Blank input returns null, which means "no change" in edit scenarios.
    /// </summary>
    private static int? ReadIntNullable(string label)
    {
        // Nullable input: blank means "no change" in update scenarios.
        while (true)
        {
            System.Console.Write($"{label}: ");
            var text = System.Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text)) return null;
            if (int.TryParse(text, out var value)) return value;
            // Invalid input: repeat prompt until user enters a valid integer or blank.
            System.Console.WriteLine("Invalid number.");
        }
    }

    /// <summary>
    /// Reads a decimal number from the console, repeating until valid input is provided.
    /// Used for required salary fields.
    /// </summary>
    private static decimal ReadDecimal(string label)
    {
        // Input loop ensures the application keeps prompting until a valid decimal is provided.
        while (true)
        {
            System.Console.Write($"{label}: ");
            var text = System.Console.ReadLine();
            if (decimal.TryParse(text, out var value)) return value;
            // Invalid input: repeat prompt until user enters a valid decimal.
            System.Console.WriteLine("Invalid number.");
        }
    }

    /// <summary>
    /// Reads an optional decimal number from the console.
    /// Blank input returns null, which means "no change" in edit scenarios.
    /// </summary>
    private static decimal? ReadDecimalNullable(string label)
    {
        // Nullable input: blank means "no change" in update scenarios.
        while (true)
        {
            System.Console.Write($"{label}: ");
            var text = System.Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text)) return null;
            if (decimal.TryParse(text, out var value)) return value;
            // Invalid input: repeat prompt until user enters a valid decimal or blank.
            System.Console.WriteLine("Invalid number.");
        }
    }
}
