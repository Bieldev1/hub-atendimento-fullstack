using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;
using EnterpriseFlow.Domain.Common.Results;
using EnterpriseFlow.Domain.SeedWork;

namespace EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;

/// <summary>
/// Agregado raiz do ciclo de vida do pedido: criação, aprovação de pagamento, despacho e entrega.
/// </summary>
public class Pedido : Entity, IAggregateRoot
{
    private readonly List<ItemPedido> itens = new();

    protected Pedido() { }

    private Pedido(long clienteId, IEnumerable<ItemPedido> itens)
    {
        ClienteId = clienteId;
        Status = StatusPedido.Criado;
        DataCriacao = DateTime.UtcNow;
        this.itens.AddRange(itens);
    }

    public long Id { get; private set; }

    public long ClienteId { get; private set; }

    public StatusPedido Status { get; private set; }

    public IReadOnlyCollection<ItemPedido> Itens => itens.AsReadOnly();

    public decimal ValorTotal => itens.Sum(item => item.Subtotal);

    public DateTime DataCriacao { get; private set; }

    public DateTime? DataAtualizacao { get; private set; }

    /// <summary>
    /// Cria um novo pedido a partir do cliente e dos itens informados, disparando o evento de domínio
    /// <see cref="PedidoCriadoEvent"/>. Retorna erro de negócio (não lança exception) se os itens
    /// forem inválidos.
    /// </summary>
    public static Result<Pedido> Criar(long clienteId, IEnumerable<ItemPedido> itens)
    {
        Result<Pedido> result = new();

        var listaItens = itens?.ToList() ?? new List<ItemPedido>();

        if (listaItens.Count == 0)
            return result.SetBusinessMessage("Um pedido precisa conter ao menos um item.");

        var pedido = new Pedido(clienteId, listaItens);
        pedido.AddDomainEvent(new PedidoCriadoEvent(pedido));

        result.Value = pedido;
        return result;
    }

    /// <summary>
    /// Marca o pedido como pago. Só é permitido a partir do status <see cref="StatusPedido.Criado"/>.
    /// </summary>
    public Result MarcarComoPago()
    {
        var resultTransicao = GarantirTransicao(StatusPedido.Criado, StatusPedido.PagamentoAprovado);

        if (!resultTransicao.Valid)
            return resultTransicao;

        Status = StatusPedido.PagamentoAprovado;
        DataAtualizacao = DateTime.UtcNow;

        AddDomainEvent(new PedidoPagamentoAprovadoEvent(this));

        return Result.Ok();
    }

    /// <summary>
    /// Despacha o pedido para entrega. Só é permitido após o pagamento ser aprovado.
    /// </summary>
    public Result Despachar()
    {
        var resultTransicao = GarantirTransicao(StatusPedido.PagamentoAprovado, StatusPedido.Despachado);

        if (!resultTransicao.Valid)
            return resultTransicao;

        Status = StatusPedido.Despachado;
        DataAtualizacao = DateTime.UtcNow;

        AddDomainEvent(new PedidoDespachadoEvent(this));

        return Result.Ok();
    }

    /// <summary>
    /// Confirma a entrega do pedido. Só é permitido após o despacho.
    /// </summary>
    public Result Entregar()
    {
        var resultTransicao = GarantirTransicao(StatusPedido.Despachado, StatusPedido.Entregue);

        if (!resultTransicao.Valid)
            return resultTransicao;

        Status = StatusPedido.Entregue;
        DataAtualizacao = DateTime.UtcNow;

        AddDomainEvent(new PedidoEntregueEvent(this));

        return Result.Ok();
    }

    private Result GarantirTransicao(StatusPedido statusEsperado, StatusPedido statusDestino)
    {
        if (Status != statusEsperado)
            return Result.Fail(
                ResultCode.BusinessError,
                $"Não é possível transicionar o pedido de '{Status}' para '{statusDestino}'. " +
                $"Status esperado: '{statusEsperado}'.");

        return Result.Ok();
    }
}
