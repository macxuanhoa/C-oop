using Microsoft.Extensions.Configuration;

namespace EducationCentreSystem.Data;

/// <summary>
/// Centralized database configuration loader.
/// Resolves the connection string from exactly two sources, in priority order:
///   1. Environment variable (EDU_DB_CONNECTION)
///   2. appsettings.json (ConnectionStrings:DefaultConnection)
///
/// No hard-coded fallback exists. If neither source provides a value, the
/// application fails fast with a clear diagnostic message.
/// </summary>
/// <remarks>
/// Design decisions:
/// - Environment variable takes priority so CI/CD and Docker deployments can
///   override appsettings.json without modifying files.
/// - appsettings.json is the everyday developer default — edit once, no need
///   to remember to set an OS-level variable on every machine.
/// - Fail-fast (InvalidOperationException) ensures configuration errors are
///   caught at startup rather than causing cryptic SQL errors later.
/// </remarks>
public static class DbSettings
{
    /// <summary>
    /// Environment variable name checked first when resolving the connection string.
    /// </summary>
    public const string ConnectionEnvVar = "EDU_DB_CONNECTION";

    /// <summary>
    /// Describes where the connection string was loaded from.
    /// Used for startup diagnostics so developers can verify which source is active.
    /// </summary>
    public enum ConfigSource
    {
        EnvironmentVariable,
        AppSettingsJson
    }

    // Cached after first resolution so repeated calls are cheap and consistent.
    private static string? _cachedConnectionString;
    private static ConfigSource? _cachedSource;

    /// <summary>
    /// Returns the resolved connection string.
    /// Throws <see cref="InvalidOperationException"/> if no source provides a value.
    /// </summary>
    public static string GetConnectionString()
    {
        if (_cachedConnectionString != null)
            return _cachedConnectionString;

        Resolve();
        return _cachedConnectionString!;
    }

    /// <summary>
    /// Returns where the connection string was loaded from.
    /// Call <see cref="GetConnectionString"/> first to trigger resolution.
    /// </summary>
    public static ConfigSource GetSource()
    {
        if (_cachedSource == null)
            Resolve();

        return _cachedSource!.Value;
    }

    /// <summary>
    /// Extracts the Database name from the resolved connection string for diagnostic logging.
    /// Returns "(unknown)" if parsing fails.
    /// </summary>
    public static string GetDatabaseName()
    {
        string connStr = GetConnectionString();

        // Parse "Database=xxx;" from the connection string.
        foreach (string part in connStr.Split(';'))
        {
            string trimmed = part.Trim();
            if (trimmed.StartsWith("Database=", StringComparison.OrdinalIgnoreCase))
            {
                return trimmed.Substring("Database=".Length).Trim();
            }
        }

        return "(unknown)";
    }

    /// <summary>
    /// Logs a startup diagnostic summary to the console.
    /// Called once at application startup so the developer can verify the active configuration.
    /// </summary>
    public static void LogStartupDiagnostics()
    {
        string source = GetSource() == ConfigSource.EnvironmentVariable
            ? $"Environment variable ({ConnectionEnvVar})"
            : "appsettings.json";

        string dbName = GetDatabaseName();

        Console.WriteLine("========================================");
        Console.WriteLine(" DATABASE CONFIGURATION");
        Console.WriteLine("========================================");
        Console.WriteLine($" Source : {source}");
        Console.WriteLine($" Database : {dbName}");
        Console.WriteLine("========================================");
    }

    /// <summary>
    /// Core resolution logic. Checks ENV first, then appsettings.json.
    /// Throws if neither source provides a non-empty connection string.
    /// </summary>
    private static void Resolve()
    {
        // Priority 1: Environment variable
        string? envValue = Environment.GetEnvironmentVariable(ConnectionEnvVar);
        if (!string.IsNullOrWhiteSpace(envValue))
        {
            _cachedConnectionString = envValue.Trim();
            _cachedSource = ConfigSource.EnvironmentVariable;
            return;
        }

        // Priority 2: appsettings.json
        string appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (File.Exists(appSettingsPath))
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string? jsonValue = config.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrWhiteSpace(jsonValue))
            {
                _cachedConnectionString = jsonValue.Trim();
                _cachedSource = ConfigSource.AppSettingsJson;
                return;
            }
        }

        // Neither source provided a value — fail fast with clear diagnostics.
        throw new InvalidOperationException(
            "No database connection string configured.\n\n" +
            "Provide one of the following:\n" +
            $"  1. Set environment variable: {ConnectionEnvVar}\n" +
            "  2. Add to appsettings.json: ConnectionStrings:DefaultConnection\n\n" +
            $"Searched appsettings.json at: {appSettingsPath}");
    }

    /// <summary>
    /// Resets cached state. Intended for unit testing only.
    /// </summary>
    internal static void ResetForTesting()
    {
        _cachedConnectionString = null;
        _cachedSource = null;
    }
}
