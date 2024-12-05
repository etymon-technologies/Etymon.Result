using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etymon.Result;

public class Result<T>
{
    private Result(T data)
    {
        Data = data;
        Error = null;
    }

    private Result(Error error)
    {
        Error = error;
        Data = default;
    }
    public T? Data { get; }

    public Error? Error { get; set; }

    public bool IsSuccess => Error == null;

    public bool IsFailure => Error != null;

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