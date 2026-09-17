using EnterpriseFlow.API.Application.Models;
using EnterpriseFlow.Domain.Common.Results;
using MediatR;

namespace EnterpriseFlow.API.Application.Commands.Pedidos;

public class CriarPedidoCommand : IRequest<Result<PedidoDto>>
{
    public long ClienteId { get; set; }

    public List<ItemPedidoInput> Itens { get; set; } = new();
}
