namespace OcrProcessor.Models
{
    public class PubSubPushEnvelope
    {
        public PubSubMessage? Message { get; set; }
        public string? Subscription { get; set; }
    }

    public class PubSubMessage
    {
        public string? Data { get; set; }        
        public string? MessageId { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
    }
}