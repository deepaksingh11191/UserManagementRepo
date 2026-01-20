using System.Threading.Tasks;

namespace UserManagement.RestAPI.Models.Misc;

public readonly struct Result<T>
{
    private readonly T? _value;

    public string? Error { get; } = null;
    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Exception e) => new(e);

    public Result()
    {
        _value = default;
        Error = "Invalid Result";
        IsError = true;
    }

    private Result(T value)
    {
        _value = default;
        Error = null;
        IsError = false;
    }

    public Result(string error)
    {
        _value = default;
        Error = error;
        IsError = true;
    }

    public Result(Exception e)
    {
        _value = default;
        Error = e.Message;
        IsError = true;
    }

    public TResult Resolve<TResult>(Func<T, TResult> success, Func<string, TResult> failure) => !IsError ? success(_value!) :
        failure(Error!);

    public Task<TResult> Resolve<TResult>(Func<T, Task<TResult>> success, Func<string, Task<TResult>> failure) => !IsError ? success(_value!) :
        failure(Error!);

    public Result<TOut> Convert<TOut>(Func<T, TOut> valueConverter)
    {
        if (IsError)
            return new Result<TOut>(Error!);
        else
            return new Result<TOut>(valueConverter(_value!));
    }

    public async Task<Result<TOut>> Convert<TOut>(Func<T, Task<TOut>> valueConverter)
    {
        if (IsError)
            return new Result<TOut>(Error!);
        else
            return new Result<TOut>(await valueConverter(_value!));
    }
}

public readonly struct Result<T, TErrCode> where TErrCode : struct
{
    private readonly T? _value;

    public TErrCode ErrorCode { get; }
    public string? Error { get; }
    public bool IsError { get; }
    public bool IsSuccess => !IsError;
    public static implicit operator Result<T, TErrCode>(T value) => new(value);
    public static implicit operator Result<T, TErrCode>((Exception, TErrCode) e) => new(e.Item1, e.Item2);
    public Result()
    {
        _value = default;
        Error = "Invalid Result";
        IsError = true;
        ErrorCode = default;
    }

    private Result(T value)
    {
        _value = value;
        Error = default;
        IsError = false;
        ErrorCode = default;
    }

    public Result(string error, TErrCode errCode)
    {
        _value = default;
        Error = error;
        IsError = true;
        ErrorCode = errCode;
    }

    public Result(Exception e, TErrCode errCode)
    {
        _value = default;
        Error = e.Message;
        IsError = true;
        ErrorCode = errCode;
    }

    public TResult Resolve<TResult>(Func<T, TResult> success, Func<string, TErrCode, TResult> failure) => !IsError ? success(_value!) :
       failure(Error!, ErrorCode);

    public Task<TResult> Resolve<TResult>(Func<T, Task<TResult>> success, Func<string, TErrCode, Task<TResult>> failure) => !IsError ? success(_value!) :
       failure(Error!, ErrorCode);

    public Result<TOut, TErrCode> Convert<TOut>(Func<T, TOut> valueConverter)
    {
        if (IsError)
            return new Result<TOut, TErrCode>(Error!, ErrorCode);
        else
            return new Result<TOut, TErrCode>(valueConverter(_value!));
    }

    public async Task<Result<TOut, TErrCode>> Convert<TOut>(Func<T, Task<TOut>> valueConverter)
    {
        if (IsError)
            return new Result<TOut, TErrCode>(Error!, ErrorCode);
        else
            return new Result<TOut, TErrCode>(await valueConverter(_value!));
    }

    public Result<T, TOutErrCode> Convert<TOutErrCode>(Func<TErrCode, TOutErrCode> errorCodeConverter)
        where TOutErrCode:struct,Enum
    {
        if (IsError)
            return new Result<T, TOutErrCode>(Error!, errorCodeConverter(ErrorCode));
        else
            return new Result<T, TOutErrCode>(_value!);
    }

    public async Task<Result<T, TOutErrCode>> Convert<TOutErrCode>(Func<TErrCode, Task<TOutErrCode>> errorCodeConverter)
      where TOutErrCode : struct, Enum
    {
        if (IsError)
            return new Result<T, TOutErrCode>(Error!, await errorCodeConverter(ErrorCode));
        else
            return new Result<T, TOutErrCode>(_value!);
    }

    public Result<TOut, TOutErrCode> Convert<TOut,TOutErrCode>(Func<T, TOut> valueConverter, Func<TErrCode,TOutErrCode> errorCodeConverter)
        where TOutErrCode : struct, Enum
    {
        if (IsError)
            return new Result<TOut, TOutErrCode>(Error!, errorCodeConverter(ErrorCode));
        else
            return new Result<TOut, TOutErrCode>(valueConverter(_value!));
    }

    public async Task<Result<TOut, TOutErrCode>> Convert<TOut, TOutErrCode>(Func<T, Task<TOut>> valueConverter, Func<TErrCode, Task<TOutErrCode>> errorCodeConverter)
       where TOutErrCode : struct, Enum
    {
        if (IsError)
            return new Result<TOut, TOutErrCode>(Error!, await errorCodeConverter(ErrorCode));
        else
            return new Result<TOut, TOutErrCode>(await valueConverter(_value!));
    }
}
