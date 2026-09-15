namespace EnterpriseFlow.Domain.Common.Results;

/// <summary>
/// Segue a numeração do HTTP para facilitar conversões.
/// </summary>
public enum ResultCode
{
    Ok = 200,
    BadRequest = 400,
    Unauthorized = 401,
    BusinessError = 422,
    GenericError = 500
}
