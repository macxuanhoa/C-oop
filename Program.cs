using System;
using System.Windows.Forms;
using EducationCentreSystem.Controllers;
using EducationCentreSystem.Data;
using EducationCentreSystem.Repositories;
using EducationCentreSystem.Views.WinForms;

namespace EducationCentreSystem
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // Set the connection string directly or via environment variable.
            string connectionString = DbSettings.TryGetConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback connection string if env var is not set.
                connectionString = "Server=localhost;Database=oop_edu;User=root;Password=;";
            }
            
            IPersonRepository repo = new MySqlPersonRepository(connectionString);

            // Controller encapsulates all CRUD operations.
            PersonController controller = new PersonController(repo);

            try
            {
                // Seed demo data only when the repository is empty.
                DataSeeder.Seed(repo, 50, 10, 3);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect to the MySQL database. Please ensure your database is running and check the connection string.\n\n" + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // WinForms UI
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(controller));
        }
    }
}
