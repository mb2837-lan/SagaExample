namespace MyApi.Sagas;

public interface ProcessingCompleted
{
    Guid CorrelationId { get; set; }
}