using System;
using System.Linq;
using System.Windows.Forms;
using EducationCentreSystem.Controllers;
using EducationCentreSystem.Data;
using EducationCentreSystem.Repositories;
using EducationCentreSystem.Views.Console;
using EducationCentreSystem.Views.WinForms;

namespace EducationCentreSystem;

/// <summary>
/// Application entry point.
/// Selects the persistence strategy (in-memory or MySQL) and launches either a console UI or a WinForms UI.
/// </summary>
/// <remarks>
/// Running modes:
/// - Console UI: pass the command-line argument "--console"
/// - WinForms UI: default when no "--console" argument is present
///
/// Storage selection:
/// - MySQL mode: set the environment variable EDU_DB_CONNECTION to a valid connection string
/// - In-memory mode: leave EDU_DB_CONNECTION unset/blank
///
/// Notes for coursework:
/// - The console UI satisfies the "text-based menu" requirement.
/// - The WinForms UI is an optional extension and reuses the same controller/model layers.
/// </remarks>
internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        // Repository selection is driven by configuration (environment variable).
        // If no database connection string is provided, the application runs fully in-memory.
        IPersonRepository repo;
        var connectionString = DbSettings.TryGetConnectionString();
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
        var controller = new PersonController(repo);

        // Seed demo data only when the repository is empty (safe for both storage modes).
        DataSeeder.Seed(repo, 50, 10, 3);

        // Console mode is intended for the coursework "text-based menu" requirement.
        if (args.Any(a => string.Equals(a, "--console", StringComparison.OrdinalIgnoreCase)))
        {
            if (connectionString == null)
                System.Console.WriteLine($"DB disabled. Set {DbSettings.ConnectionEnvVar} to enable MySQL mode.");
            var view = new ConsoleMenuView(controller);
            view.Run();
            return;
        }

        // WinForms mode provides an optional desktop UI on Windows.
        ApplicationConfiguration.Initialize();

        Application.Run(new Form1(controller));
    }
}
