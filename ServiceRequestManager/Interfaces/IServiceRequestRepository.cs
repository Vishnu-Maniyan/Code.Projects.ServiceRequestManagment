using ServiceRequestManager.Models;

namespace ServiceRequestManager.Interfaces
{
	public interface IServiceRequestRepository
	{
		public Task<List<ServiceRequestDTO>> GetServiceRequests();
		Task<ServiceRequestDTO> GetServiceRequest(Guid? id);
		Task<Guid?> AddServiceRequest(ServiceRequest serviceRequest);
        Task<ServiceRequestEntity> GetExistingRecord(Guid? id);
        Task<Guid?> UpdateServiceRequest(ServiceRequest serviceRequest, ServiceRequestEntity entity = null);
        Task<bool> RemoveServiceRequest(ServiceRequestEntity entity);
    }
}
