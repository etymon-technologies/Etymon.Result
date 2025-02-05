using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etymon.Result;
/// <summary>
/// Represents the result of an operation, encapsulating success or failure states.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class, indicating success.
    /// </summary>
    public Result()
    {
        Error = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class, indicating failure.
    /// </summary>
    /// <param name="error">The error representing the failure reason.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="error"/> is null.</exception>
    public Result(Error error)
    {
        Error = error ?? throw new ArgumentNullException(nameof(error));
    }

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess => Error == null;

    /// <summary>
    /// Gets the error associated with a failed operation, or <c>null</c> if the operation was successful.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    /// Creates a successful <see cref="Result"/>.
    /// </summary>
    /// <returns>A success result.</returns>
    public static Result Success() => new();


    /// <summary>
    /// Creates a failed <see cref="Result"/> with the specified error.
    /// </summary>
    /// <param name="error">The error representing the failure reason.</param>
    /// <returns>A failure result.</returns>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// Returns a string representation of the result.
    /// </summary>
    /// <returns>"Success" if successful; otherwise, the error details.</returns>

    public override string ToString() => IsSuccess ? "Success" : $"Failure: {Error}";
    /// <summary>
    /// Deconstructs the result into its success state and error.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result is successful.</param>
    /// <param name="error">The error, if any.</param>
    public void Deconstruct(out bool isSuccess, out Error? error)
    {
        isSuccess = IsSuccess;
        error = Error;
    }

}

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

    private Result(Error error): base(error)
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

/// <summary>
/// Represents an error that describes a failure.
/// </summary>
/// <param name="Code">A unique identifier for the error.</param>
/// <param name="Message">A human-readable error message.</param>
/// 

/// <summary>
/// Represents an error that describes a failure in an operation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Error"/> class.
/// </remarks>
/// <param name="code">A unique identifier for the error.</param>
/// <param name="message">A human-readable description of the error.</param>
public class Error(string code, string message)
{
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