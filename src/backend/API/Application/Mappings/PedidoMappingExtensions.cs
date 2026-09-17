using EnterpriseFlow.API.Application.Models;
using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;

namespace EnterpriseFlow.API.Application.Mappings;

public static class PedidoMappingExtensions
{
    public static PedidoDto ParaDto(this Pedido pedido) => new()
    {
        Id = pedido.Id,
        ClienteId = pedido.ClienteId,
        Status = pedido.Status.ToString(),
        ValorTotal = pedido.ValorTotal,
        DataCriacao = pedido.DataCriacao,
        DataAtualizacao = pedido.DataAtualizacao,
        Itens = pedido.Itens.Select(item => new ItemPedidoDto
        {
            ProdutoId = item.ProdutoId,
            Descricao = item.Descricao,
            Quantidade = item.Quantidade,
            PrecoUnitario = item.PrecoUnitario,
            Subtotal = item.Subtotal
        }).ToList()
    };
}
