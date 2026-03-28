using System;
using System.Collections.Generic;
using System.Linq;

// ===== MODELS =====
namespace EducationCentreSystem.Models
{
    public enum PersonRole
    {
        Student,
        Teacher,
        Admin
    }

    public class Person
    {
        // Core identity information shared by all roles in the system.
        public string Name { get; set; } = string.Empty;

        // Contact information used for user records (may be blank if not provided).
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public PersonRole Role { get; }

        protected Person(PersonRole role)
        {
            Role = role;
        }

        public virtual string GetDetails()
        {
            return $"Role: {Role} | Name: {Name} | Telephone: {Telephone} | Email: {Email}";
        }
    }

    public class Student : Person
    {
        // System-managed identifier assigned by StudentManager.
        public int Id { get; set; }

        // Coursework requirement: students store exactly three subjects.
        public string Subject1 { get; set; } = string.Empty;
        public string Subject2 { get; set; } = string.Empty;
        public string Subject3 { get; set; } = string.Empty;

        public Student()
            : base(PersonRole.Student)
        {
        }

        public override string GetDetails()
        {
            return $"{base.GetDetails()}\n  StudentId: {Id} | Subjects: {Subject1}, {Subject2}, {Subject3}";
        }

        public override string ToString()
        {
            return $"{Id} - {Name}";
        }
    }

    public class Teacher : Person
    {
        // System-managed identifier assigned by TeacherManager.
        public int Id { get; set; }

        // Compensation details for teaching staff.
        public decimal Salary { get; set; }

        // Coursework requirement: teachers store exactly two subjects.
        public string Subject1 { get; set; } = string.Empty;
        public string Subject2 { get; set; } = string.Empty;

        public Teacher()
            : base(PersonRole.Teacher)
        {
        }

        public override string GetDetails()
        {
            return $"{base.GetDetails()}\n  TeacherId: {Id} | Salary: {Salary} | Subjects: {Subject1}, {Subject2}";
        }

        public override string ToString()
        {
            return $"{Id} - {Name}";
        }
    }

    public class Admin : Person
    {
        // Compensation details for Admin staff.
        public decimal Salary { get; set; }

        // Employment type, e.g. "FullTime" or "PartTime".
        public string FullTimeOrPartTime { get; set; } = string.Empty;

        // Total working hours for the Admin record (meaning depends on employment type).
        public int WorkingHours { get; set; }

        public Admin()
            : base(PersonRole.Admin)
        {
        }

        public override string GetDetails()
        {
            return $"{base.GetDetails()}\n  Salary: {Salary} | Type: {FullTimeOrPartTime} | WorkingHours: {WorkingHours}";
        }
    }
}

// ===== MANAGERS =====
namespace EducationCentreSystem.Managers
{
    using EducationCentreSystem.Models;

    public class PersonManager
    {
        // Polymorphic storage for all user roles (Student / Teacher / Admin).
        private readonly List<Person> people = new List<Person>();

        private static string? NormalizeEmail(string? email)
        {
            // Email normalization ensures consistent lookups and uniqueness checks.
            // The coursework uses Email as the unique identifier, so we standardize:
            // - Trim whitespace
            // - Compare in a case-insensitive way (UpperInvariant)
            var value = (email ?? string.Empty).Trim();
            return value.Length == 0 ? null : value.ToUpperInvariant();
        }

        public void AddPerson(Person person)
        {
            // Add a person to the polymorphic list only if the Email is unique.
            // This supports the "Email as identifier" requirement across roles.
            var email = NormalizeEmail(person.Email);
            if (email == null)
            {
                if (!people.Contains(person)) people.Add(person);
                return;
            }

            if (people.Any(p => NormalizeEmail(p.Email) == email)) return;
            people.Add(person);
        }

        public IReadOnlyList<Person> GetAllPeople()
        {
            return people.AsReadOnly();
        }

        public List<Person> GetPeopleByRole(PersonRole role)
        {
            return people.Where(p => p.Role == role).ToList();
        }

        public Person? FindByEmail(string email)
        {
            // Find uses normalized email to ensure reliable matching for Edit/Delete flows.
            var target = NormalizeEmail(email);
            if (target == null) return null;
            return people.FirstOrDefault(p => NormalizeEmail(p.Email) == target);
        }

        public void DeleteByEmail(string email)
        {
            // Delete uses normalized email to remove the correct record and avoid case issues.
            var target = NormalizeEmail(email);
            if (target == null) return;

            var person = people.FirstOrDefault(p => NormalizeEmail(p.Email) == target);
            if (person != null)
            {
                people.Remove(person);
            }
        }
    }

    public class StudentManager
    {
        private List<Student> students = new List<Student>();
        private int nextId = 1;

        public void AddStudent(Student student)
        {
            student.Id = nextId++;
            students.Add(student);
        }

        public List<Student> GetAllStudents()
        {
            return students;
        }

        public void UpdateStudent(Student updatedStudent)
        {
            var student = students.FirstOrDefault(s => s.Id == updatedStudent.Id);
            if (student != null)
            {
                student.Name = updatedStudent.Name;
                student.Telephone = updatedStudent.Telephone;
                student.Email = updatedStudent.Email;
                student.Subject1 = updatedStudent.Subject1;
                student.Subject2 = updatedStudent.Subject2;
                student.Subject3 = updatedStudent.Subject3;
            }
        }

        public void DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                students.Remove(student);
            }
        }
    }

    public class TeacherManager
    {
        private List<Teacher> teachers = new List<Teacher>();
        private int nextId = 1;

        public void AddTeacher(Teacher teacher)
        {
            teacher.Id = nextId++;
            teachers.Add(teacher);
        }

        public List<Teacher> GetAllTeachers()
        {
            return teachers;
        }

        public void UpdateTeacher(Teacher updatedTeacher)
        {
            var teacher = teachers.FirstOrDefault(t => t.Id == updatedTeacher.Id);
            if (teacher != null)
            {
                teacher.Name = updatedTeacher.Name;
                teacher.Telephone = updatedTeacher.Telephone;
                teacher.Email = updatedTeacher.Email;
                teacher.Salary = updatedTeacher.Salary;
                teacher.Subject1 = updatedTeacher.Subject1;
                teacher.Subject2 = updatedTeacher.Subject2;
            }
        }

        public void DeleteTeacher(int id)
        {
            var teacher = teachers.FirstOrDefault(t => t.Id == id);
            if (teacher != null)
            {
                teachers.Remove(teacher);
            }
        }
    }

}

// ===== GLOBAL DATA =====
namespace EducationCentreSystem
{
    using EducationCentreSystem.Managers;

    public static class GlobalData
    {
        public static StudentManager StudentManager { get; } = new StudentManager();
        public static TeacherManager TeacherManager { get; } = new TeacherManager();
        public static PersonManager PersonManager { get; } = new PersonManager();
    }
}

// ===== PROGRAM =====
namespace EducationCentreSystem
{
    using EducationCentreSystem.Managers;
    using EducationCentreSystem.Models;

    static class Program
    {
        static StudentManager studentManager = GlobalData.StudentManager;
        static TeacherManager teacherManager = GlobalData.TeacherManager;
        static PersonManager personManager = GlobalData.PersonManager;

        static void Main(string[] args)
        {
            RunConsoleMode();
        }

        // --- CODE CONSOLE MODE ---
        static void RunConsoleMode()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- EDUCATION CENTRE MANAGEMENT SYSTEM (CONSOLE) ---");
                Console.WriteLine("1 Add record");
                Console.WriteLine("2 View all records");
                Console.WriteLine("3 View records by role");
                Console.WriteLine("4 Edit record");
                Console.WriteLine("5 Delete record");
                Console.WriteLine("6 Exit");
                Console.Write("Enter choice (1-6): ");

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddRecord(); break;
                    case "2": ViewAllRecords(); break;
                    case "3": ViewRecordsByRole(); break;
                    case "4": EditRecord(); break;
                    case "5": DeleteRecord(); break;
                    case "6": exit = true; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        static void SyncPeopleFromManagers()
        {
            // Keep PersonManager aligned with role-specific managers.
            // This ensures "View", "Edit", and "Delete" work consistently using the Email identifier.
            foreach (var s in studentManager.GetAllStudents()) personManager.AddPerson(s);
            foreach (var t in teacherManager.GetAllTeachers()) personManager.AddPerson(t);
        }

        static void AddRecord()
        {
            // AddRecord is the single entry point for creating new Student/Teacher/Admin records.
            // Minimal fixes applied:
            // 1) Enforce unique Email before adding to any manager.
            // 2) Validate numeric input by reprompting until valid.
            Console.WriteLine("\nSelect role to add:");
            Console.WriteLine("1. Student");
            Console.WriteLine("2. Teacher");
            Console.WriteLine("3. Admin");
            Console.Write("Enter choice (1/2/3): ");
            string? roleChoice = Console.ReadLine();

            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Telephone: ");
            string telephone = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            // Ensure Email uniqueness across the entire system before adding any record.
            SyncPeopleFromManagers();
            if (personManager.FindByEmail(email) != null)
            {
                Console.WriteLine("Email already exists. Record was not added.");
                return;
            }

            if (roleChoice == "1")
            {
                // Student: three subjects.
                Console.Write("Subject 1: ");
                string subject1 = Console.ReadLine() ?? "";
                Console.Write("Subject 2: ");
                string subject2 = Console.ReadLine() ?? "";
                Console.Write("Subject 3: ");
                string subject3 = Console.ReadLine() ?? "";

                var student = new Student
                {
                    Name = name,
                    Telephone = telephone,
                    Email = email,
                    Subject1 = subject1,
                    Subject2 = subject2,
                    Subject3 = subject3
                };

                // Preserve existing behavior: StudentManager assigns Id and owns Student list.
                studentManager.AddStudent(student);
                personManager.AddPerson(student);
                Console.WriteLine("Student added successfully.");
                return;
            }

            if (roleChoice == "2")
            {
                // Teacher: salary + two subjects.
                decimal salary;
                while (true)
                {
                    // Input validation: do not default invalid numeric input to 0.
                    Console.Write("Salary: ");
                    string? salaryText = Console.ReadLine();
                    if (decimal.TryParse(salaryText, out salary)) break;
                    Console.WriteLine("Invalid salary. Please enter a valid number.");
                }

                Console.Write("Subject 1: ");
                string subject1 = Console.ReadLine() ?? "";
                Console.Write("Subject 2: ");
                string subject2 = Console.ReadLine() ?? "";

                var teacher = new Teacher
                {
                    Name = name,
                    Telephone = telephone,
                    Email = email,
                    Salary = salary,
                    Subject1 = subject1,
                    Subject2 = subject2
                };

                teacherManager.AddTeacher(teacher);
                personManager.AddPerson(teacher);
                Console.WriteLine("Teacher added successfully.");
                return;
            }

            if (roleChoice == "3")
            {
                // Admin: salary + employment type + working hours.
                decimal salary;
                while (true)
                {
                    // Input validation: do not default invalid numeric input to 0.
                    Console.Write("Salary: ");
                    string? salaryText = Console.ReadLine();
                    if (decimal.TryParse(salaryText, out salary)) break;
                    Console.WriteLine("Invalid salary. Please enter a valid number.");
                }

                Console.Write("FullTimeOrPartTime: ");
                string fullTimeOrPartTime = Console.ReadLine() ?? "";

                int workingHours;
                while (true)
                {
                    // Input validation: require a valid whole number for working hours.
                    Console.Write("WorkingHours: ");
                    string? workingHoursText = Console.ReadLine();
                    if (int.TryParse(workingHoursText, out workingHours)) break;
                    Console.WriteLine("Invalid working hours. Please enter a valid whole number.");
                }

                var admin = new Admin
                {
                    Name = name,
                    Telephone = telephone,
                    Email = email,
                    Salary = salary,
                    FullTimeOrPartTime = fullTimeOrPartTime,
                    WorkingHours = workingHours
                };

                personManager.AddPerson(admin);
                Console.WriteLine("Admin added successfully.");
                return;
            }

            Console.WriteLine("Invalid role.");
        }

        static void ViewAllRecords()
        {
            // Display all stored records using a single polymorphic list.
            SyncPeopleFromManagers();
            var all = personManager.GetAllPeople();

            if (all.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            Console.WriteLine("\n--- ALL RECORDS ---");
            foreach (var p in all) PrintPerson(p);
        }

        static void ViewRecordsByRole()
        {
            // Filter records by Role without changing the underlying design.
            SyncPeopleFromManagers();
            Console.WriteLine("\nFilter by role:");
            Console.WriteLine("1. Student");
            Console.WriteLine("2. Teacher");
            Console.WriteLine("3. Admin");
            Console.Write("Enter choice (1/2/3): ");
            string? roleChoice = Console.ReadLine();

            PersonRole? role = roleChoice == "1" ? PersonRole.Student :
                roleChoice == "2" ? PersonRole.Teacher :
                roleChoice == "3" ? PersonRole.Admin : null;

            if (role == null)
            {
                Console.WriteLine("Invalid role.");
                return;
            }

            var list = personManager.GetPeopleByRole(role.Value);
            if (list.Count == 0)
            {
                Console.WriteLine("No records found for this role.");
                return;
            }

            Console.WriteLine($"\n--- {role.Value.ToString().ToUpper()} LIST ---");
            foreach (var p in list) PrintPerson(p);
        }

        static void EditRecord()
        {
            // Edit flow:
            // - Identify record by Email (unique identifier)
            // - Allow updating shared fields and role-specific fields
            // - Validate numeric edits (Salary/WorkingHours) with reprompt if user enters invalid values
            // Simple edit flow: identify by Email, then update common + role-specific fields.
            SyncPeopleFromManagers();
            Console.Write("\nEnter Email of the record to edit: ");
            string email = Console.ReadLine() ?? "";

            var person = personManager.FindByEmail(email);
            if (person == null)
            {
                Console.WriteLine("Record not found.");
                return;
            }

            Console.WriteLine("Leave blank to keep current value.");

            Console.Write($"Name ({person.Name}): ");
            string newName = Console.ReadLine() ?? "";
            if (newName != "") person.Name = newName;

            Console.Write($"Telephone ({person.Telephone}): ");
            string newTelephone = Console.ReadLine() ?? "";
            if (newTelephone != "") person.Telephone = newTelephone;

            Console.WriteLine($"Role: {person.Role}");

            if (person is Student student)
            {
                Console.Write($"Subject1 ({student.Subject1}): ");
                string v1 = Console.ReadLine() ?? "";
                if (v1 != "") student.Subject1 = v1;

                Console.Write($"Subject2 ({student.Subject2}): ");
                string v2 = Console.ReadLine() ?? "";
                if (v2 != "") student.Subject2 = v2;

                Console.Write($"Subject3 ({student.Subject3}): ");
                string v3 = Console.ReadLine() ?? "";
                if (v3 != "") student.Subject3 = v3;

                studentManager.UpdateStudent(student);
                Console.WriteLine("Student updated successfully.");
                return;
            }

            if (person is Teacher teacher)
            {
                while (true)
                {
                    // Input validation: if user enters a value, it must be numeric; blank keeps current value.
                    Console.Write($"Salary ({teacher.Salary}): ");
                    string salaryText = Console.ReadLine() ?? "";
                    if (salaryText == "") break;
                    if (decimal.TryParse(salaryText, out var salary))
                    {
                        teacher.Salary = salary;
                        break;
                    }
                    Console.WriteLine("Invalid salary. Please enter a valid number or leave blank.");
                }

                Console.Write($"Subject1 ({teacher.Subject1}): ");
                string v1 = Console.ReadLine() ?? "";
                if (v1 != "") teacher.Subject1 = v1;

                Console.Write($"Subject2 ({teacher.Subject2}): ");
                string v2 = Console.ReadLine() ?? "";
                if (v2 != "") teacher.Subject2 = v2;

                teacherManager.UpdateTeacher(teacher);
                Console.WriteLine("Teacher updated successfully.");
                return;
            }

            if (person is Admin admin)
            {
                while (true)
                {
                    // Input validation: if user enters a value, it must be numeric; blank keeps current value.
                    Console.Write($"Salary ({admin.Salary}): ");
                    string salaryText = Console.ReadLine() ?? "";
                    if (salaryText == "") break;
                    if (decimal.TryParse(salaryText, out var salary))
                    {
                        admin.Salary = salary;
                        break;
                    }
                    Console.WriteLine("Invalid salary. Please enter a valid number or leave blank.");
                }

                Console.Write($"FullTimeOrPartTime ({admin.FullTimeOrPartTime}): ");
                string ftpt = Console.ReadLine() ?? "";
                if (ftpt != "") admin.FullTimeOrPartTime = ftpt;

                while (true)
                {
                    // Input validation: if user enters a value, it must be an integer; blank keeps current value.
                    Console.Write($"WorkingHours ({admin.WorkingHours}): ");
                    string whText = Console.ReadLine() ?? "";
                    if (whText == "") break;
                    if (int.TryParse(whText, out var wh))
                    {
                        admin.WorkingHours = wh;
                        break;
                    }
                    Console.WriteLine("Invalid working hours. Please enter a valid whole number or leave blank.");
                }

                Console.WriteLine("Admin updated successfully.");
                return;
            }
        }

        static void DeleteRecord()
        {
            // Delete flow:
            // - Identify record by Email (unique identifier)
            // - Remove from role-specific manager (Student/Teacher) if applicable
            // - Remove from PersonManager (polymorphic list)
            SyncPeopleFromManagers();
            Console.Write("\nEnter Email of the record to delete: ");
            string email = Console.ReadLine() ?? "";

            var person = personManager.FindByEmail(email);
            if (person == null)
            {
                Console.WriteLine("Record not found.");
                return;
            }

            if (person is Student student)
            {
                studentManager.DeleteStudent(student.Id);
            }
            else if (person is Teacher teacher)
            {
                teacherManager.DeleteTeacher(teacher.Id);
            }

            personManager.DeleteByEmail(email);
            Console.WriteLine("Record deleted successfully.");
        }

        static void PrintPerson(Person person)
        {
            Console.WriteLine(person.GetDetails());
        }
    }
}
