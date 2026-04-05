using System;
using System.Linq;
using System.Windows.Forms;
using EducationCentreSystem.Controllers;
using EducationCentreSystem.Data;
using EducationCentreSystem.Repositories;
using EducationCentreSystem.Views.Console;
using EducationCentreSystem.Views.WinForms;

namespace EducationCentreSystem
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            // Repository selection is driven by configuration (environment variable).
            // If no database connection string is provided, the application runs fully in-memory.
            IPersonRepository repo;
            string? connectionString = DbSettings.TryGetConnectionString();
            
            if (connectionString != null)
            {
                // MySQL mode persists data in a database table.
                repo = new MySqlPersonRepository(connectionString);
            }
            else
            {
                // In-memory mode stores data in process memory (data is lost when the application exits).
                repo = new InMemoryPersonRepository();
            }

            // Controller encapsulates all CRUD operations and hides the repository implementation from the UI layer.
            PersonController controller = new PersonController(repo);

            // Seed demo data only when the repository is empty (safe for both storage modes).
            DataSeeder.Seed(repo, 50, 10, 3);

            // Console mode is intended for the coursework "text-based menu" requirement.
            bool isConsoleMode = false;
            foreach (string arg in args)
            {
                if (string.Equals(arg, "--console", StringComparison.OrdinalIgnoreCase))
                {
                    isConsoleMode = true;
                    break;
                }
            }

            if (isConsoleMode == true)
            {
                if (connectionString == null)
                {
                    System.Console.WriteLine("DB disabled. Set " + DbSettings.ConnectionEnvVar + " to enable MySQL mode.");
                }
                ConsoleMenuView view = new ConsoleMenuView(controller);
                view.Run();
                return;
            }

            // WinForms mode provides an optional desktop UI on Windows.
            ApplicationConfiguration.Initialize();

            Application.Run(new Form1(controller));
        }
    }
}
