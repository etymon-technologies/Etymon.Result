﻿namespace Etymon.Result;

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