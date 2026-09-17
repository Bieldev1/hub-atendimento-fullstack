using EnterpriseFlow.Domain.SeedWork;

namespace EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;

/// <summary>
/// Item que compõe um <see cref="Pedido"/>. Não é um agregado próprio: seu ciclo de vida
/// é controlado inteiramente pelo agregado Pedido.
/// </summary>
public class ItemPedido : Entity
{
    protected ItemPedido() { }

    public ItemPedido(long produtoId, string descricao, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade do item deve ser maior que zero.", nameof(quantidade));

        if (precoUnitario < 0)
            throw new ArgumentException("O preço unitário não pode ser negativo.", nameof(precoUnitario));

        ProdutoId = produtoId;
        Descricao = descricao;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public long Id { get; private set; }

    public long PedidoId { get; private set; }

    public long ProdutoId { get; private set; }

    public string Descricao { get; private set; } = string.Empty;

    public int Quantidade { get; private set; }

    public decimal PrecoUnitario { get; private set; }

    public decimal Subtotal => Quantidade * PrecoUnitario;
}
