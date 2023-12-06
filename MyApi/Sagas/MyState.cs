/// <summary>
/// Represents a state in the state machine.
/// </summary>
public class MyState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; }
}