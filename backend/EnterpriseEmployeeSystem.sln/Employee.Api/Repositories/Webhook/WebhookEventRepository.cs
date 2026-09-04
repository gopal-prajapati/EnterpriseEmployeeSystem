using EnterpriseEmployeeSystem.Api.Data;
using EnterpriseEmployeeSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeSystem.Api.Repositories.Webhook
{
    public class WebhookEventRepository: IWebhookEventRepository
    {
        private readonly AppDbContext _dbContext;

        public WebhookEventRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(string eventId)
        {
            return await _dbContext.WebhookEvents
                .AnyAsync(x => x.EventId == eventId);
        }

        public async Task AddAsync(WebhookEvent webhookEvent)
        {
            _dbContext.WebhookEvents.Add(webhookEvent);

            await _dbContext.SaveChangesAsync();
        }
    }
}
