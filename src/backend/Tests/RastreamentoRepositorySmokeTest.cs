using EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;
using EnterpriseFlow.Infra.Data;
using EnterpriseFlow.Infra.Data.Options;
using EnterpriseFlow.Infra.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace EnterpriseFlow.Tests;

// Teste de integração manual contra o Mongo real do docker-compose. Requer os containers no ar
// (docker compose up) e a variável de ambiente MONGO_TEST_CONNECTION_STRING apontando pra ele
// (ex.: "mongodb://admin:<MONGO_INITDB_ROOT_PASSWORD>@localhost:27017/?authSource=admin", usando
// a senha do seu .env local). Sem essa variável, o teste é pulado — não faz parte do CI ainda
// (sem testcontainers configurado) e por isso nunca deve ter credencial hardcoded aqui.
public class RastreamentoRepositorySmokeTest
{
    private const string ConnectionStringEnvVar = "MONGO_TEST_CONNECTION_STRING";

    private static RastreamentoRepository CriarRepository(string connectionString)
    {
        var options = Options.Create(new MongoOptions
        {
            ConnectionString = connectionString,
            DatabaseName = "EnterpriseFlow",
            RastreamentoCollectionName = "Rastreamento"
        });

        var context = new MongoContext(options);
        return new RastreamentoRepository(context, options, new NoOpMediator());
    }

    [Fact]
    public async Task DeveGravarERecuperarRastreamentoComHistorico()
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvVar);

        if (string.IsNullOrWhiteSpace(connectionString))
            return; // Teste manual: só roda se MONGO_TEST_CONNECTION_STRING estiver definida.

        var repository = CriarRepository(connectionString);
        var pedidoId = DateTime.UtcNow.Ticks;

        var rastreamento = Rastreamento.Iniciar(pedidoId, entregadorId: 42, new Localizacao(-23.55, -46.63));
        await repository.AdicionarAsync(rastreamento);

        var recuperado = await repository.ObterPorPedidoIdAsync(pedidoId);

        Assert.NotNull(recuperado);
        Assert.Equal(pedidoId, recuperado!.PedidoId);
        Assert.Equal(42, recuperado.EntregadorId);
        Assert.Single(recuperado.Historico);

        var resultAtualizar = recuperado.AtualizarLocalizacao(new Localizacao(-23.56, -46.64));
        Assert.True(resultAtualizar.Valid);

        await repository.AtualizarAsync(recuperado);

        var reRecuperado = await repository.ObterPorPedidoIdAsync(pedidoId);

        Assert.NotNull(reRecuperado);
        Assert.Equal(2, reRecuperado!.Historico.Count);
        Assert.Equal(-23.56, reRecuperado.LocalizacaoAtual.Latitude);
    }

    private class NoOpMediator : IMediator
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification => Task.CompletedTask;
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<object?> Send(object request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest => throw new NotImplementedException();
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
