namespace MyApi.Sagas;

public interface ProcessingFaulted
{
    Guid CorrelationId { get; set; }
}