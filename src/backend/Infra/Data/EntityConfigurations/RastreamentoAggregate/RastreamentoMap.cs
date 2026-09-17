using EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;
using EnterpriseFlow.Domain.SeedWork;
using MongoDB.Bson.Serialization;

namespace EnterpriseFlow.Infra.Data.EntityConfigurations.RastreamentoAggregate;

/// <summary>
/// Mapeamento BSON do agregado Rastreamento. Equivalente ao *Map (IEntityTypeConfiguration) usado
/// para SQL Server, adaptado ao driver do MongoDB (que não tem um mecanismo de configuração
/// declarativa por classe — o registro é feito programaticamente via <see cref="BsonClassMap"/>).
/// </summary>
internal static class RastreamentoMap
{
    public static void Configure()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Rastreamento)))
            return;

        // DomainEvents é declarado em DocumentEntity (base); o driver do Mongo exige que o
        // UnmapProperty seja feito no class map da classe que efetivamente declara o membro.
        BsonClassMap.RegisterClassMap<DocumentEntity>(map =>
        {
            map.AutoMap();
            map.UnmapProperty(it => it.DomainEvents);
        });

        BsonClassMap.RegisterClassMap<Rastreamento>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdProperty(it => it.Id);
            map.UnmapProperty(it => it.Historico);
            map.MapField("historico").SetElementName("Historico");
        });

        BsonClassMap.RegisterClassMap<Localizacao>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<PosicaoHistorico>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
        });
    }
}
