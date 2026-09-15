namespace EnterpriseFlow.Domain.Common.Results;

/// <summary>
/// Representa o resultado de uma operação. Por padrão é assumido como válido ao ser instanciado.
/// </summary>
public class Result
{
    public Result() => SetToOk();

    public string Message { get; private set; } = string.Empty;

    public Guid Protocol { get; } = Guid.NewGuid();

    public ResultCode ResultCode { get; private set; }

    public bool Valid => (int)ResultCode < 400;

    public virtual Result Set(ResultCode code, string? message)
    {
        if (code == ResultCode.Ok)
        {
            SetToOk(message);
            return this;
        }

        Message = message ?? string.Empty;
        ResultCode = code;
        return this;
    }

    public virtual Result SetBusinessMessage(string message) => Set(ResultCode.BusinessError, message);

    public virtual Result AddValidation(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException($"O parâmetro {nameof(message)} é obrigatório.", nameof(message));

        if (ResultCode == ResultCode.Ok)
            Set(ResultCode.BusinessError, message);
        else
            Message += "\r\n" + message;

        return this;
    }

    public static Result Ok(string? message = null) => new Result().Set(ResultCode.Ok, message);

    public static Result Fail(ResultCode code, string message) => new Result().Set(code, message);

    private void SetToOk(string? message = null)
    {
        Message = string.IsNullOrWhiteSpace(message) ? "Operação realizada com sucesso." : message;
        ResultCode = ResultCode.Ok;
    }
}
