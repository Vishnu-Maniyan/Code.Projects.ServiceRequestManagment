using ServiceRequestManager.Models;
using ServiceRequestManager.Repository;

namespace ServiceRequestManager.Interfaces
{
	public interface IServiceRequestProvider
	{
		Task<List<ServiceRequestDTO>> GetAllServiceRequest();
        Task<ServiceRequestDTO> GetServiceRequest(Guid? id);
        Task<Guid?> AddServiceRequest(ServiceRequest serviceRequest);
        Task<Guid?> UpdateServiceRequest(Guid? id, ServiceRequest serviceRequest);
        Task<string?> RemoveServiceRequest(Guid? id);
    }
}
