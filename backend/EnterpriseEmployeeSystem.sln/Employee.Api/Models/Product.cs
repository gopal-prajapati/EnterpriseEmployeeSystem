namespace EnterpriseEmployeeSystem.Api.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string ItemCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Currency { get; set; } = "INR";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    }
}
