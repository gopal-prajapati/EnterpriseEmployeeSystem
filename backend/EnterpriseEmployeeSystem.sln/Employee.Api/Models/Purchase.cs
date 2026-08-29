using EnterpriseEmployeeSystem.Api.Enum;

namespace EnterpriseEmployeeSystem.Api.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string ItemCode { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public Employee Employee { get; set; } = null!;

        public ICollection<Payment> Payments { get; set; }
            = new List<Payment>();

    }
}
