namespace Etymon.Result;

/// <summary>
/// Represents the result codes for various outcomes.
/// </summary>
public enum ResultCode
{
    /// <summary>
    /// Indicates a successful operation.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the requested resource was not found.
    /// </summary>
    NotFound,

    /// <summary>
    /// Indicates a validation error occurred.
    /// </summary>
    ValidationError,

    /// <summary>
    /// Indicates that the user is not authorized to perform the operation.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Indicates a conflict occurred, such as a duplicate resource.
    /// </summary>
    Conflict,

    /// <summary>
    /// Indicates an internal server error occurred.
    /// </summary>
    InternalError
}
