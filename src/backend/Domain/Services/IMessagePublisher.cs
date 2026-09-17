namespace EnterpriseFlow.Domain.Services;

/// <summary>
/// Abstrai a publicação de mensagens/eventos nos barramentos de mensageria e streaming
/// (RabbitMQ para filas de integração, Kafka para streaming de eventos do ciclo de vida do pedido).
/// A camada de Infra decide, por implementação concreta, para qual broker cada mensagem vai.
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publica uma mensagem no destino informado (fila do RabbitMQ ou tópico do Kafka,
    /// dependendo da implementação registrada).
    /// </summary>
    Task PublicarAsync<TMensagem>(string destino, TMensagem mensagem, CancellationToken cancellationToken = default)
        where TMensagem : class;
}
