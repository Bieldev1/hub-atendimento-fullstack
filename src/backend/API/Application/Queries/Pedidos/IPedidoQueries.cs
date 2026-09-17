using EnterpriseFlow.API.Application.Models;

namespace EnterpriseFlow.API.Application.Queries.Pedidos;

/// <summary>
/// Consultas de leitura do Pedido. Não passa por Command/MediatR: implementações concretas
/// (Infra) podem consultar diretamente via Dapper/SQL otimizado para leitura, sem carregar
/// o agregado completo via EF Core.
/// </summary>
public interface IPedidoQueries
{
    Task<PedidoDto?> GetPedidoByIdAsync(long pedidoId);
}
