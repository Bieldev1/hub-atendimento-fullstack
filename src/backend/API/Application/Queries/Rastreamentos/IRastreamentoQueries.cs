using EnterpriseFlow.API.Application.Models;

namespace EnterpriseFlow.API.Application.Queries.Rastreamentos;

/// <summary>
/// Consultas de leitura do Rastreamento. Não passa por Command/MediatR: implementações concretas
/// (Infra) consultam diretamente o MongoDB, sem passar pelo agregado de domínio.
/// </summary>
public interface IRastreamentoQueries
{
    Task<HistoricoRastreamentoDto?> GetHistoricoByPedidoIdAsync(long pedidoId);
}
