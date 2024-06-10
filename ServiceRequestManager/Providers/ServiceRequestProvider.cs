using ServiceRequestManager.Constants;
using ServiceRequestManager.Interfaces;
using ServiceRequestManager.Models;

namespace ServiceRequestManager.Providers
{
	public class ServiceRequestProvider: IServiceRequestProvider
	{
		private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IEmailClient _emailClient;
		public ServiceRequestProvider(IServiceRequestRepository serviceRequestRepository,
            IEmailClient emailClient) 
		{ 
			_serviceRequestRepository = serviceRequestRepository;
            _emailClient = emailClient;

        }

		public async Task<List<ServiceRequestDTO>> GetAllServiceRequest()
		{
			return await _serviceRequestRepository.GetServiceRequests();
		}

		public async Task<ServiceRequestDTO> GetServiceRequest(Guid? id)
		{
			return await _serviceRequestRepository.GetServiceRequest(id);
        }
        public async Task<Guid?> AddServiceRequest(ServiceRequest serviceRequest)
        {
			var existingRecord = await _serviceRequestRepository.GetExistingRecord(serviceRequest.id);
			if (existingRecord != null)
				return null;

            var result = await _serviceRequestRepository.AddServiceRequest(serviceRequest);
			
            if (serviceRequest.currentStatus == CurrentStatus.Created)
			{
				await _emailClient.SendEmail($"Service Request With Id {result} has been Created");
			}
            return result;
		}
        public async Task<Guid?> UpdateServiceRequest(Guid? id, ServiceRequest serviceRequest)
        {
            var existingRecord = await _serviceRequestRepository.GetExistingRecord(serviceRequest.id);
            if (existingRecord == null)
                return null;

            var result = await _serviceRequestRepository.UpdateServiceRequest(serviceRequest, existingRecord);
            //Send Email if Request Status has Changed
            if(serviceRequest.currentStatus == CurrentStatus.Complete )
            {
                await _emailClient.SendEmail($"Service Request With Id {result} has been {serviceRequest.currentStatus.ToString()}");
            }
            else
            {
				await _emailClient.SendEmail($"Service Request With Id {result} Status has Changed {serviceRequest.currentStatus.ToString()}");
			}
			return result;
        }

        public async Task<string?> RemoveServiceRequest(Guid? id)
        {
            var existingRecord = await _serviceRequestRepository.GetExistingRecord(id);
            if (existingRecord == null)
                return null;

            var result = await _serviceRequestRepository.RemoveServiceRequest(existingRecord) ? ResponseStatus.Successful : ResponseStatus.UnSuccessful;
			//Send Email With Service Request Delete Status
			await _emailClient.SendEmail($"Service Request With Id {id} removal was {result}");
			return result;
        }
    }
}
