namespace EducationCentreSystem.Common;

/// <summary>
/// Represents the outcome of an operation without a return value.
/// Used to communicate success/failure and an error message to the UI layer.
/// </summary>
public sealed class OperationResult
{
    /// <summary>
    /// True when the operation succeeded.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Error message when the operation failed; null when successful.
    /// </summary>
    public string? Error { get; }

    private OperationResult(bool success, string? error)
    {
        Success = success;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static OperationResult Ok()
    {
        return new OperationResult(true, null);
    }

    /// <summary>
    /// Creates a failed result with an error message.
    /// </summary>
    public static OperationResult Fail(string error)
    {
        return new OperationResult(false, error);
    }
}

/// <summary>
/// Represents the outcome of an operation that returns data.
/// </summary>
public sealed class OperationResult<T>
{
    /// <summary>
    /// True when the operation succeeded.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Error message when the operation failed; null when successful.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Returned data when successful; null/default when failed.
    /// </summary>
    public T? Data { get; }

    private OperationResult(bool success, T? data, string? error)
    {
        Success = success;
        Data = data;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result wrapping the returned data.
    /// </summary>
    public static OperationResult<T> Ok(T data)
    {
        return new OperationResult<T>(true, data, null);
    }

    /// <summary>
    /// Creates a failed result with an error message.
    /// </summary>
    public static OperationResult<T> Fail(string error)
    {
        return new OperationResult<T>(false, default, error);
    }
}
