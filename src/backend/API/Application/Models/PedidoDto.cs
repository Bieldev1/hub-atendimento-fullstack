namespace EnterpriseFlow.API.Application.Models;

public class PedidoDto
{
    public long Id { get; set; }

    public long ClienteId { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<ItemPedidoDto> Itens { get; set; } = new();

    public decimal ValorTotal { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAtualizacao { get; set; }
}
