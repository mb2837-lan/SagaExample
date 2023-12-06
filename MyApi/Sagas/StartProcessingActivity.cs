using MyApi.Sagas;

public class StartProcessingActivity : IStateMachineActivity<MyState, StartProcessing>
{
    public void Accept(MassTransit.StateMachineVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public async Task Execute(MassTransit.BehaviorContext<MyState, StartProcessing> context, IBehavior<MyState, StartProcessing> next)
    {
        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(MassTransit.BehaviorExceptionContext<MyState, StartProcessing, TException> context, IBehavior<MyState, StartProcessing> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("start-processing");
    }
}