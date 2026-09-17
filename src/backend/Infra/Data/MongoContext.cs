using EnterpriseFlow.Infra.Data.EntityConfigurations.RastreamentoAggregate;
using EnterpriseFlow.Infra.Data.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EnterpriseFlow.Infra.Data;

/// <summary>
/// Encapsula a conexão com o MongoDB. Equivalente ao <see cref="EnterpriseFlowDbContext"/> para
/// os agregados persistidos como documento (ex.: Rastreamento).
/// </summary>
public class MongoContext
{
    public MongoContext(IOptions<MongoOptions> mongoOptions)
    {
        RastreamentoMap.Configure();

        var options = mongoOptions.Value;
        var client = new MongoClient(options.ConnectionString);

        Database = client.GetDatabase(options.DatabaseName);
    }

    public IMongoDatabase Database { get; }
}
