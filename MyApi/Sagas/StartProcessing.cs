namespace MyApi.Sagas;

public interface StartProcessing
{
    Guid CorrelationId { get; set; }
}