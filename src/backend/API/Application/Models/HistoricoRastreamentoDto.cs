namespace EnterpriseFlow.API.Application.Models;

public class HistoricoRastreamentoDto
{
    public string Id { get; set; } = string.Empty;

    public long PedidoId { get; set; }

    public long EntregadorId { get; set; }

    public string Status { get; set; } = string.Empty;

    public PosicaoHistoricoDto LocalizacaoAtual { get; set; } = new();

    public List<PosicaoHistoricoDto> Historico { get; set; } = new();
}
