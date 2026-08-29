namespace EnterpriseEmployeeSystem.Api.DTOs
{
    public class CreatePurchaseRequest
    {
        public int EmployeeId { get; set; }

        public string ItemCode { get; set; } = string.Empty;

    }
}
