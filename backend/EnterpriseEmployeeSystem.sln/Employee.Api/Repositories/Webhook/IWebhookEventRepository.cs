using EnterpriseEmployeeSystem.Api.Models;

namespace EnterpriseEmployeeSystem.Api.Repositories.Webhook
{
    public interface IWebhookEventRepository
    {
        Task<bool> ExistsAsync(string eventId);

        Task AddAsync(WebhookEvent webhookEvent);
    }
}
