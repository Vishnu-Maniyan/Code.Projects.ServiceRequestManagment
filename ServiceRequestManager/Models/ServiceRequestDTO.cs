namespace ServiceRequestManager.Models
{
	public class ServiceRequestDTO
	{
        public Guid? Id { get; set; }
        public string? BuildingCode { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
