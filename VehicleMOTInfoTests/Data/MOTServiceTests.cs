using Microsoft.VisualStudio.TestTools.UnitTesting;
using VehicleMOTInfo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Moq;
using Moq.Protected;

namespace VehicleMOTInfo.Data.Tests
{
    [TestClass()]
    public class MOTServiceTests
    {
        private Mock<HttpMessageHandler> mockHandler;
        private MOTService motService;

        [TestInitialize]
        public void SetUp()
        {
            // Mock HttpMessageHandler
            mockHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHandler.Object);

            motService = new MOTService(httpClient);
        }

        [TestMethod()]
        public async Task GetVehicleInfoTest_ReturnsCorrectInfo()
        {
            // Arrange
            var registrationNumber = "REG123";
            var expectedResponse = "[{\"registration\":\"REG123\",\"make\":\"HONDA\",\"model\":\"HR-V\",\"primaryColour\":\"White\",\"motTests\":[{\"expiryDate\":\"2024-08-10\",\"odometerValue\":\"57859\",\"odometerUnit\":\"mi\"}]}]";
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse),
                })
                .Verifiable();

            // Act
            var result = await motService.GetVehicleInfo(registrationNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("HONDA", result.First().Make);
            Assert.AreEqual("HR-V", result.First().Model);
            Assert.AreEqual("White", result.First().PrimaryColour);
            Assert.AreEqual("57859", result.First().GetLatestMotTest().OdometerValue);
            Assert.AreEqual("mi", result.First().GetLatestMotTest().OdometerUnit);

        }
    }
}