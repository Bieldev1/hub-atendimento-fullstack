namespace EnterpriseFlow.Domain.Services;

/// <summary>
/// Abstrai o cache distribuído (Redis) usado para dados de leitura frequente
/// (ex.: localização em tempo real) e para locks distribuídos de concorrência.
/// </summary>
public interface ICacheService
{
    Task<T?> ObterAsync<T>(string chave) where T : class;

    Task DefinirAsync<T>(string chave, T valor, TimeSpan? expiracao = null) where T : class;

    Task RemoverAsync(string chave);

    /// <summary>
    /// Tenta adquirir um lock distribuído. Retorna um token de posse em caso de sucesso,
    /// ou <c>null</c> se o lock já estiver em uso.
    /// </summary>
    Task<string?> TentarAdquirirLockAsync(string chave, TimeSpan duracao);

    /// <summary>
    /// Libera um lock previamente adquirido, desde que o token de posse informado seja o mesmo.
    /// </summary>
    Task LiberarLockAsync(string chave, string tokenPosse);
}
