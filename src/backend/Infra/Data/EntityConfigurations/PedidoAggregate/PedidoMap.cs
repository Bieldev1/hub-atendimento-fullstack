using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseFlow.Infra.Data.EntityConfigurations.PedidoAggregate;

internal class PedidoMap : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedido");

        builder.HasKey(it => it.Id);
        builder.Ignore(it => it.DomainEvents);

        builder.Property(it => it.Id).HasColumnName("Id").IsRequired();
        builder.Property(it => it.ClienteId).HasColumnName("ClienteId").IsRequired();
        builder.Property(it => it.Status).HasColumnName("Status").IsRequired();
        builder.Property(it => it.DataCriacao).HasColumnName("DataCriacao").IsRequired();
        builder.Property(it => it.DataAtualizacao).HasColumnName("DataAtualizacao").IsRequired(false);

        builder.Ignore(it => it.ValorTotal);

        builder.HasMany(it => it.Itens)
            .WithOne()
            .HasForeignKey(it => it.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
