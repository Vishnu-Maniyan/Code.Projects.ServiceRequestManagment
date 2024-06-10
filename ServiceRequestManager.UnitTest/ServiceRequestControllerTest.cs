using Microsoft.Extensions.Logging;
using Moq;
using ServiceRequestManager.Controllers;
using ServiceRequestManager.Interfaces;
using AutoFixture;
using ServiceRequestManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace ServiceRequesManager.UnitTest
{
	public class ServiceRequestControllerTest
	{
		ServiceRequestController serviceRequestController;
		Mock<ILogger<ServiceRequestController>> logger;
		Mock<IServiceRequestProvider> serviceRequestProvider;

		[SetUp]
		public void Setup()
		{
			logger = new Mock<ILogger<ServiceRequestController>>();
			serviceRequestProvider =  new Mock<IServiceRequestProvider>();
			serviceRequestController = new ServiceRequestController(logger.Object, serviceRequestProvider.Object);
		}

		[Test]
		public void Get_When_Response_Is_Valid()
		{
			var fixture = new Fixture();
			var serviceResponse = fixture.Create<List<ServiceRequestDTO>>();
			serviceRequestProvider.Setup(x => x.GetAllServiceRequest()).ReturnsAsync(serviceResponse);
			var response = serviceRequestController.Get().Result;
			Assert.IsNotNull(response);
		}
		[Test]
		public void Get_When_Response_Is_Null()
		{
			serviceRequestProvider.Setup(x => x.GetAllServiceRequest()).ReturnsAsync((List<ServiceRequestDTO>)null);
			var response = (NoContentResult)serviceRequestController.Get().Result;
			Assert.AreEqual(204, response.StatusCode);
		}

		[Test]
		public void Get_When_Exception()
		{
			serviceRequestProvider.Setup(x => x.GetAllServiceRequest()).ThrowsAsync(new Exception("This is a Unit Test Case Exception"));
			var response = (BadRequestObjectResult)serviceRequestController.Get().Result;
			Assert.AreEqual(400, response.StatusCode);
		}
	}
}