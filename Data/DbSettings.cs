namespace EducationCentreSystem.Data;

/// <summary>
/// Database configuration utilities.
/// Uses an environment variable to keep connection details out of source code.
/// </summary>
/// <remarks>
/// Using an environment variable makes it easy to run the same executable in two modes:
/// - With a database (persistent storage)
/// - Without a database (in-memory storage)
/// </remarks>
public static class DbSettings
{
    /// <summary>
    /// Environment variable name that enables MySQL mode when set.
    /// </summary>
    public const string ConnectionEnvVar = "EDU_DB_CONNECTION";

    /// <summary>
    /// Returns the configured connection string or null when not provided.
    /// Null indicates the application should fall back to in-memory mode.
    /// </summary>
    public static string? TryGetConnectionString()
    {
        string? value = Environment.GetEnvironmentVariable(ConnectionEnvVar);
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
