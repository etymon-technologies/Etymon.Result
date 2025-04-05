namespace Etymon.Result;

/// <summary>
/// Represents the result of an operation with a return value.
/// </summary>
/// <typeparam name="T">The type of the result data.</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with a successful value.
    /// </summary>
    /// <param name="data">The successful result data.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null.</exception>
    private Result(T data) : base()
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with an error.
    /// </summary>
    /// <param name="error">The error representing the failure reason.</param>
    private Result(Error error) : base(error)
    {
        Data = default;
    }

    /// <summary>
    /// Gets the result data if the operation was successful; otherwise, <c>null</c>.
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> with the specified data.
    /// </summary>
    /// <param name="data">The result data.</param>
    /// <returns>A success result containing the data.</returns>
    public static Result<T> Success(T data) => new(data);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> with the specified error.
    /// </summary>
    /// <param name="error">The error representing the failure reason.</param>
    /// <returns>A failure result.</returns>
    public static new Result<T> Failure(Error error) => new(error);

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> to a successful <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="data">The result data.</param>
    public static implicit operator Result<T>(T data) => Success(data);

    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a failed <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="error">The error.</param>
    public static implicit operator Result<T>(Error error) => new(error);

    /// <summary>
    /// Matches the result to either a success or failure handler.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="onSuccess">Function to execute if the result is successful.</param>
    /// <param name="onFailure">Function to execute if the result is a failure.</param>
    /// <returns>The result of either <paramref name="onSuccess"/> or <paramref name="onFailure"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="onSuccess"/> or <paramref name="onFailure"/> is null.</exception>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess ? onSuccess(Data!) : onFailure(Error!);
    }

    /// <summary>
    /// Returns a string representation of the result.
    /// </summary>
    /// <returns>"Success: {Data}" if successful; otherwise, the error details.</returns>

    public override string ToString() => IsSuccess ? $"Success: {Data}" : $"Failure: {Error}";

    /// <summary>
    /// Deconstructs the result into its success state, data, and error.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result is successful.</param>
    /// <param name="data">The result data, if any.</param>
    /// <param name="error">The error, if any.</param>
    public void Deconstruct(out bool isSuccess, out T? data, out Error? error)
    {
        isSuccess = IsSuccess;
        data = Data;
        error = Error;
    }
}