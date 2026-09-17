using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseFlow.Infra.Data.EntityConfigurations.PedidoAggregate;

internal class ItemPedidoMap : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("ItemPedido");

        builder.HasKey(it => it.Id);
        builder.Ignore(it => it.DomainEvents);
        builder.Ignore(it => it.Subtotal);

        builder.Property(it => it.Id).HasColumnName("Id").IsRequired();
        builder.Property(it => it.PedidoId).HasColumnName("PedidoId").IsRequired();
        builder.Property(it => it.ProdutoId).HasColumnName("ProdutoId").IsRequired();
        builder.Property(it => it.Descricao).HasColumnName("Descricao").HasColumnType("varchar(300)").IsRequired();
        builder.Property(it => it.Quantidade).HasColumnName("Quantidade").IsRequired();
        builder.Property(it => it.PrecoUnitario).HasColumnName("PrecoUnitario").HasColumnType("decimal(18,2)").IsRequired();
    }
}
