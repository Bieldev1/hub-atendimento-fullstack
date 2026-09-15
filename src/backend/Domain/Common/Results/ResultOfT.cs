namespace EnterpriseFlow.Domain.Common.Results;

/// <summary>
/// Representa o resultado de uma operação com valor de retorno.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Result<T> : Result
{
    public Result(T? value = default)
        : base() => Value = value;

    public T? Value { get; set; }

    public static Result<T> Ok(T value, string? message = null) => (Result<T>)new Result<T>(value).Set(ResultCode.Ok, message);

    public static new Result<T> Fail(ResultCode code, string message) => (Result<T>)new Result<T>().Set(code, message);
}
