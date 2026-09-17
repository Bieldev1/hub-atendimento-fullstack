using EnterpriseFlow.API.Application.Mappings;
using EnterpriseFlow.API.Application.Models;
using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;
using EnterpriseFlow.Domain.Common.Results;
using EnterpriseFlow.Domain.SeedWork;
using MediatR;

namespace EnterpriseFlow.API.Application.Commands.Pedidos;

public class CriarPedidoCommandHandle : IRequestHandler<CriarPedidoCommand, Result<PedidoDto>>
{
    private readonly IPedidoRepository pedidoRepository;
    private readonly IUnitOfWork unitOfWork;

    public CriarPedidoCommandHandle(IPedidoRepository pedidoRepository, IUnitOfWork unitOfWork)
    {
        this.pedidoRepository = pedidoRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<PedidoDto>> Handle(CriarPedidoCommand command, CancellationToken cancellationToken)
    {
        Result<PedidoDto> result = new();

        var itens = command.Itens.Select(item =>
            new ItemPedido(item.ProdutoId, item.Descricao, item.Quantidade, item.PrecoUnitario));

        Result<Pedido> resultCreate = Pedido.Criar(command.ClienteId, itens);

        if (!resultCreate.Valid)
            return result.SetFromAnother(resultCreate);

        Pedido pedido = resultCreate.Value!;

        pedidoRepository.Add(pedido);

        Result resultSave = await unitOfWork.SaveChangesAsync(cancellationToken);

        if (!resultSave.Valid)
            return result.SetFromAnother(resultSave);

        result.Value = pedido.ParaDto();
        return result;
    }
}
