namespace MyApi.Sagas;

/// <summary>
/// Represents a state machine that models a long-running and fault-tolerant workflow.
/// </summary>
public class MyStateMachine : MassTransitStateMachine<MyState>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyStateMachine"/> class.
    /// </summary>
    public MyStateMachine(Serilog.ILogger logger)
    {
        logger.Information("Entered the state machine {@DateTime}", DateTimeOffset.UtcNow);

        InstanceState(x => x.CurrentState);

        Event(() => StartProcessing);
        Event(() => ProcessingFaulted);
        Event(() => ProcessingCompleted);

        Initially(
            When(StartProcessing)
                .Activity(x => x.OfType<StartProcessingActivity>())
                .TransitionTo(Processing)
        );

        During(Processing,
            When(ProcessingFaulted)
                .TransitionTo(Faulted),
            When(ProcessingCompleted)
                .TransitionTo(Completed)
        );
    }

    /// <summary>
    /// Gets the processing state.
    /// </summary>
    public State Processing { get; private set; }

    /// <summary>
    /// Gets the completed state.
    /// </summary>
    public State Completed { get; private set; }

    /// <summary>
    /// Gets the faulted state.
    /// </summary>
    public State Faulted { get; private set; }

    /// <summary>
    /// Gets the start processing event.
    /// </summary>
    public Event<StartProcessing> StartProcessing { get; private set; }

    /// <summary>
    /// Gets the processing failed event.
    /// </summary>
    public Event<ProcessingFaulted> ProcessingFaulted { get; private set; }

    /// <summary>
    /// Gets the processing completed event.
    /// </summary>
    public Event<ProcessingCompleted> ProcessingCompleted { get; private set; }
}