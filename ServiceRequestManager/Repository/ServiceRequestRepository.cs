using Microsoft.EntityFrameworkCore;
using ServiceRequestManager.Context;
using ServiceRequestManager.Interfaces;
using ServiceRequestManager.Models;

namespace ServiceRequestManager.Repository
{
	public class ServiceRequestRepository : IServiceRequestRepository
	{
		private readonly ServiceRequestContext _context;
        public ServiceRequestRepository(ILogger<ServiceRequestRepository> logger,
			ServiceRequestContext context) 
		{
			_context = context;
        }

		public async Task<List<ServiceRequestDTO>> GetServiceRequests()
		{
			var serviceRequests = await _context.ServiceRequests.Select(
				x => 
				new ServiceRequestDTO
				{
					Id = x.Id,
					BuildingCode = x.BuildingCode,
					Description	= x.Description,
					Status = x.Status
				}).ToListAsync();
			return serviceRequests;
        }
        public async Task<ServiceRequestDTO> GetServiceRequest(Guid? id)
        {
            var serviceRequests = await _context.ServiceRequests.Where(x => x.Id == id).Select(
                x =>
                new ServiceRequestDTO
                {
                    Id = x.Id,
                    BuildingCode = x.BuildingCode,
                    Description = x.Description,
                    Status = x.Status,
                }).SingleOrDefaultAsync();
            return serviceRequests;
        }
        public async Task<Guid?> AddServiceRequest(ServiceRequest serviceRequest)
        {
            var result = await CreateOrUpdateServiceRequestEntity(serviceRequest);
            await _context.AddAsync(result);
            await _context.SaveChangesAsync();
            return await Task.FromResult(result.Id);
        }

        public async Task<Guid?> UpdateServiceRequest(ServiceRequest serviceRequest, ServiceRequestEntity existingRecord = null)
        {
            var result = await CreateOrUpdateServiceRequestEntity(serviceRequest, existingRecord);
            _context.Update(result);
            await _context.SaveChangesAsync();
            return await Task.FromResult(result.Id);
        }

        public async Task<bool> RemoveServiceRequest(ServiceRequestEntity record)
        {
            _context.Remove(record);
            var status = ( await _context.SaveChangesAsync() ==  1 ) ? true : false;
            return await Task.FromResult(status);
        }

        public async Task<ServiceRequestEntity> GetExistingRecord(Guid? id) 
            => await _context.ServiceRequests.Where(x => x.Id == id).Select(
                x => x).FirstOrDefaultAsync();

        internal async Task<ServiceRequestEntity> CreateOrUpdateServiceRequestEntity(ServiceRequest serviceRequest,
            ServiceRequestEntity existingRecord = null)
        {
            if (existingRecord == null)
            {
                return new ServiceRequestEntity()
                {
                    Id = serviceRequest.id,
                    BuildingCode = serviceRequest.buildingCode,
                    Description = serviceRequest.description,
                    Status = serviceRequest.currentStatus.ToString(),
                    CreatedBy = serviceRequest.createdBy,
                    CreatedDate = serviceRequest.createdDate ?? DateTime.Now,
                    LastModifiedBy = serviceRequest.lastModifiedBy,
                    LastModifiedDate = serviceRequest.lastModifiedDate ?? DateTime.Now
                };
            }
            else
            {
                existingRecord.BuildingCode = serviceRequest.buildingCode;
                existingRecord.Description = serviceRequest.description;
                existingRecord.Status = serviceRequest.currentStatus.ToString();
                existingRecord.LastModifiedBy = serviceRequest.lastModifiedBy;
                existingRecord.LastModifiedDate = serviceRequest.lastModifiedDate;
                return existingRecord;
            }           
        }        
    }
}
