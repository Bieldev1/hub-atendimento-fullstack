namespace EnterpriseFlow.API.Application.Models;

/// <summary>
/// Dados de entrada de um item ao criar um pedido (sem o Subtotal, que é calculado pelo domínio).
/// </summary>
public class ItemPedidoInput
{
    public long ProdutoId { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }
}
