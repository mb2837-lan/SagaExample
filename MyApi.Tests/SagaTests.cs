using MyApi.Sagas;

public class SagaTests
{
    [Test]
    public async Task ShouldBeAbleToStartSaga()
    {
        // arrange
        var logger = new LoggerConfiguration()
            .WriteTo.NUnitOutput()
            .CreateLogger();

        Log.Logger = logger;

        await using var provider = new ServiceCollection()
            .AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog(logger);
            })
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddSagaStateMachine<MyStateMachine, MyState>();
                cfg.AddSingleton(Log.Logger);
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();

        var sagaId = NewId.NextGuid();

        // act
        await harness.Bus.Publish<StartProcessing>(new
        {
            CorrelationId = sagaId
        });

        (await harness.Consumed.Any<StartProcessing>()).Should().BeTrue();

        var sagaHarness = harness.GetSagaStateMachineHarness<MyStateMachine, MyState>();

        // assert
        (await sagaHarness.Created.Any(x => x.CorrelationId.Equals(sagaId))).Should().BeTrue();
        var instance = sagaHarness.Created.ContainsInState(sagaId, sagaHarness.StateMachine, sagaHarness.StateMachine.Processing);
        instance.Should().NotBeNull("Saga not found");

        await harness.Stop();
    }
}