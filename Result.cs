using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etymon.Result;

public class Result
{
    public Result()
    {
        Error = null;
    }

    public Result(Error error)
    {
        Error = error;
    }

    public bool IsSuccess => Error == null;

    public bool IsFailure => Error != null;

    public Error? Error { get; }

    public static Result Success() => new();

    
    public static Result Failure(Error error) => new(error);

}

public class Result<T> : Result
{
    private Result(T data)
    {
        Data = data;
    }

    private Result(Error error): base(error)
    {
        
        Data = default;
    }
    public T? Data { get; }

    
    public static Result<T> Success(T data) => new(data);
    public static Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(T data) => Success(data);

   
   

    public static implicit operator Result<T>(Error error) => new(error);

    // Add a Match method for functional-style handling
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Data!) : onFailure(Error!);
    }

}
public class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}