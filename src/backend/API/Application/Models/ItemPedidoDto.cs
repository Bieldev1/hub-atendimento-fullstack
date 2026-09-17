namespace EnterpriseFlow.API.Application.Models;

public class ItemPedidoDto
{
    public long ProdutoId { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public decimal Subtotal { get; set; }
}
