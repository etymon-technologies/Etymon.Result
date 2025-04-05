namespace Etymon.Result;

/// <summary>
/// Represents an error that describes a failure in an operation.
/// </summary>
/// <param name="code">A unique identifier for the error.</param>
/// <param name="message">A human-readable description of the error.</param>
public class Error(string code, string message)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class using a <see cref="ResultCode"/> as the code.
    /// </summary>
    /// <param name="resultCode">The <see cref="ResultCode"/> representing the error type.</param>
    /// <param name="message">A human-readable error message.</param>
    public Error(ResultCode resultCode, string message)
        : this(resultCode.ToString(), message)
    {
    }

    /// <summary>
    /// Gets the unique identifier for the error.
    /// </summary>
    public string Code { get; } = code;

    /// <summary>
    /// Gets a human-readable error message describing the failure.
    /// </summary>
    public string Message { get; } = message;

    /// <summary>
    /// Returns a string representation of the error, combining the error code and message.
    /// </summary>
    /// <returns>A formatted string containing the error code and message.</returns>
    public override string ToString() => $"{Code}: {Message}";
}