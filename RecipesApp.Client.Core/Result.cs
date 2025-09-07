using System.Diagnostics.CodeAnalysis;

namespace RecipesApp.Client.Core;

public sealed class Result<TSuccess>
{
    private Result(TSuccess? data, string? errorCode, string? errorData, Exception? exception, bool isSuccess)
    {
        Data = data;
        ErrorCode = errorCode;
        ErrorData = errorData;
        Exception = exception;
        IsSuccess = isSuccess;
    }

    [MemberNotNullWhen(true, nameof(Data))]
    [MemberNotNullWhen(false, nameof(ErrorCode))]
    [MemberNotNullWhen(false, nameof(ErrorData))]
    public bool IsSuccess { get; }

    public TSuccess? Data { get; }
    public string? ErrorCode { get; }
    public string? ErrorData { get; }
    public Exception? Exception { get; }

    public static Result<TSuccess> Success(TSuccess data) { return new Result<TSuccess>(data, null, null, null, true); }

    public static Result<TSuccess> Success() { return new Result<TSuccess>(default, null, null, null, true); }

    public static Result<TSuccess> Fail(string errorCode, string? errorData = null, Exception? exception = null)
    {
        return new Result<TSuccess>(default, errorCode, errorData, exception, false);
    }

    public static Result<TSuccess> Fail(Exception exception)
    {
        return new Result<TSuccess>(default, nameof(exception), exception.Message, exception, false);
    }
}