using System;
using EducationCentreSystem.Models;
using EducationCentreSystem.Repositories;
using EducationCentreSystem.Factories;

namespace EducationCentreSystem.Data
{
    /// <summary>
    /// Inserts sample data for demonstration and testing.
    /// Seeding only occurs when the repository is empty so repeated runs do not duplicate records.
    /// </summary>
    /// <remarks>
    /// Sample data supports quick testing of:
    /// - View all records
    /// - View records by role
    /// - Edit and delete by email
    ///
    /// This is especially useful in in-memory mode, where data would otherwise start empty on each run.
    /// </remarks>
    public static class DataSeeder
    {
        /// <summary>
        /// Seeds the repository with random students, teachers, and admins.
        /// </summary>
        public static void Seed(IPersonRepository repo, int studentCount, int teacherCount, int adminCount)
        {
            // Avoid inserting duplicates when the repository already contains data.
            if (repo.GetAll().Count > 0) return;

            // Name and subject pools used to generate simple, readable demo records.
            string[] firstNames = new string[] { "John", "Jane", "Alice", "Bob", "Charlie", "Diana", "Ethan", "Fiona", "George", "Hannah", "Ivan", "Julia", "Kevin", "Laura", "Michael", "Nora", "Oscar", "Paula", "Quinn", "Rachel" };
            string[] lastNames = new string[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };
            string[] subjects = new string[] { "Math", "Physics", "Chemistry", "Biology", "History", "Geography", "English", "Literature", "Computer Science", "Art" };
            Random random = new Random();

            // Students
            for (int i = 0; i < studentCount; i++)
            {
                // Factory returns the correct derived type; cast is safe because role is Student.
                Student s = (Student)PersonFactory.Create(PersonRole.Student);
                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                // Email is constructed to be unique within the seeded dataset.
                s.Name = firstName + " " + lastName;
                s.Email = "student" + (i + 1) + "@" + lastName.ToLower() + ".com";
                s.Telephone = "09" + random.Next(10000000, 99999999);
                s.Subject1 = subjects[random.Next(subjects.Length)];
                s.Subject2 = subjects[random.Next(subjects.Length)];
                s.Subject3 = subjects[random.Next(subjects.Length)];
                repo.Add(s);
            }

            // Teachers
            for (int i = 0; i < teacherCount; i++)
            {
                // Factory returns the correct derived type; cast is safe because role is Teacher.
                Teacher t = (Teacher)PersonFactory.Create(PersonRole.Teacher);
                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                // Salary is generated within a range suitable for demo purposes.
                t.Name = firstName + " " + lastName;
                t.Email = "teacher" + (i + 1) + "@" + lastName.ToLower() + ".com";
                t.Telephone = "08" + random.Next(10000000, 99999999);
                t.Salary = random.Next(2000, 5000);
                t.Subject1 = subjects[random.Next(subjects.Length)];
                t.Subject2 = subjects[random.Next(subjects.Length)];
                repo.Add(t);
            }

            // Admins
            for (int i = 0; i < adminCount; i++)
            {
                // Factory returns the correct derived type; cast is safe because role is Admin.
                Admin a = (Admin)PersonFactory.Create(PersonRole.Admin);
                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                // Job type and working hours demonstrate admin-specific fields.
                a.Name = firstName + " " + lastName;
                a.Email = "admin" + (i + 1) + "@" + lastName.ToLower() + ".com";
                a.Telephone = "07" + random.Next(10000000, 99999999);
                a.Salary = random.Next(3000, 6000);
                a.FullTimeOrPartTime = random.Next(0, 2) == 0 ? "Full-time" : "Part-time";
                a.WorkingHours = random.Next(20, 41);
                repo.Add(a);
            }
        }
    }
}
