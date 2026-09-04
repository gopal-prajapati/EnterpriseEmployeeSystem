namespace EnterpriseEmployeeSystem.Api.Models
{
    public class WebhookEvent
    {
        public int Id { get; set; }

        public string EventId { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public DateTime ProcessedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
