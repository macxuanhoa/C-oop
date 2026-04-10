using System;
using System.Windows.Forms;
using EducationCentreSystem.Controllers;
using EducationCentreSystem.Data;
using EducationCentreSystem.Repositories;
using EducationCentreSystem.Views.WinForms;
using MySqlConnector;

namespace EducationCentreSystem
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // ── Step 1: Resolve and log the connection string ────────────────
            // DbSettings checks ENV var first, then appsettings.json.
            // If neither is configured, it throws with a clear diagnostic message.
            string connectionString;
            try
            {
                connectionString = DbSettings.GetConnectionString();
                DbSettings.LogStartupDiagnostics();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Configuration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // ── Step 2: Validate database connectivity ───────────────────────
            // Attempt a real connection before wiring up the rest of the app.
            // This catches wrong credentials, missing DB, or MySQL not running.
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    Console.WriteLine($"[Startup] Connection test PASSED — server version: {conn.ServerVersion}");
                }
            }
            catch (MySqlException ex)
            {
                string dbName = DbSettings.GetDatabaseName();
                string source = DbSettings.GetSource() == DbSettings.ConfigSource.EnvironmentVariable
                    ? $"Environment variable ({DbSettings.ConnectionEnvVar})"
                    : "appsettings.json";

                MessageBox.Show(
                    $"Could not connect to the MySQL database.\n\n" +
                    $"Database : {dbName}\n" +
                    $"Source   : {source}\n\n" +
                    $"Possible causes:\n" +
                    $"• MySQL server is not running (start XAMPP/WAMP)\n" +
                    $"• Database '{dbName}' does not exist in phpMyAdmin\n" +
                    $"• Incorrect username or password\n\n" +
                    $"Technical details:\n{ex.Message}",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // ── Step 3: Wire up repository, controller, and seed data ────────
            IPersonRepository repo = new MySqlPersonRepository(connectionString);
            PersonController controller = new PersonController(repo);

            try
            {
                // Seed demo data only when the repository is empty.
                DataSeeder.Seed(repo, 50, 10, 3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database is reachable but data seeding failed.\n\n" + ex.Message,
                    "Seeding Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // ── Step 4: Launch WinForms UI ───────────────────────────────────
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(controller));
        }
    }
}
