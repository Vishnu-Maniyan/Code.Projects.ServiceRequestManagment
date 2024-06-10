using Microsoft.AspNetCore.Mvc;
using ServiceRequestManager.Constants;
using ServiceRequestManager.Interfaces;
using ServiceRequestManager.Models;

namespace ServiceRequestManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceRequestController : ControllerBase
	{
		private ILogger<ServiceRequestController> _logger;
		private readonly IServiceRequestProvider _serviceRequestProvider;

        public ServiceRequestController(ILogger<ServiceRequestController> logger,
			IServiceRequestProvider serviceRequestProvider) 
		{
            _logger = logger;
			_serviceRequestProvider = serviceRequestProvider;
        }
        [HttpGet]
		public async Task<IActionResult> Get()
		{
			try 
			{
                _logger.LogInformation("Get All Service Request Started");
				var result = await _serviceRequestProvider.GetAllServiceRequest();
                if (result == null)
				{
                    _logger.LogInformation("No Records Reterived");
                    return NoContent();
                }
                _logger.LogInformation($"Total Records Reterived {result.Count}");
                _logger.LogInformation("Get All Service Request Completed");
                return Ok(result);
            }
            catch (Exception ex)
			{
				_logger.LogInformation($"Some error occurred while processing : {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

        [HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid? id)
		{
            try
            {
                _logger.LogInformation("Get Service Request Started");

				if (id == null || id == Guid.Empty)
					return BadRequest("Id Cannot be Empty/ Null");
				
                var result = await _serviceRequestProvider.GetServiceRequest(id);
                if (result == null)
                {
                    _logger.LogInformation("No Records Reterived");
                    return NotFound();
                }
                _logger.LogInformation($"Record Reterived {result.Id}");
                _logger.LogInformation("Get Service Request Completed");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Some error occurred while processing : {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
		public async Task<IActionResult> Post([FromBody] ServiceRequest serviceRequest)
		{
            try
            {
                _logger.LogInformation("Add Service Request Started");
                var result = await _serviceRequestProvider.AddServiceRequest(serviceRequest);
                if (result == null)
                    return BadRequest("Record Already Exist with Same Id");

                _logger.LogInformation($"Record Added {result}");
                _logger.LogInformation("Add Service Request Completed");

                return Created($"api/ServiceRequest/{result}", result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Some error occurred while processing : {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
		public async Task<IActionResult> Put(Guid? id, [FromBody] ServiceRequest serviceRequest)
		{
            try
            {
                _logger.LogInformation("Update Service Request Started");

				if (id == null || id == Guid.Empty)
					return BadRequest("Id Cannot be Empty/ Null");

				var result = await _serviceRequestProvider.UpdateServiceRequest(id, serviceRequest);

                if (result == null)
                    return NotFound($"No Record Exists With the Id {id}");

                _logger.LogInformation($"Record Updated {result}");
                _logger.LogInformation("Update Service Request Completed");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Some error occurred while processing : {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid? id)
		{
            try
            {
                _logger.LogInformation("Delete Service Request Started");

				if (id == null || id == Guid.Empty)
					return BadRequest("Id Cannot be Empty/ Null");

				var result = await _serviceRequestProvider.RemoveServiceRequest(id);
                if (result == ResponseStatus.UnSuccessful)
                    return NotFound($"No Record Exists With the Id {id}");

                _logger.LogInformation("Delete Service Request Completed");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Some error occurred while processing : {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
	}
}
