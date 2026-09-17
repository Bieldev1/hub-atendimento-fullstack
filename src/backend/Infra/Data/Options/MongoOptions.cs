namespace EnterpriseFlow.Infra.Data.Options;

/// <summary>
/// Opções de conexão com o MongoDB, usado para o histórico bruto de rastreamento.
/// </summary>
public class MongoOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public string RastreamentoCollectionName { get; set; } = "Rastreamento";
}
