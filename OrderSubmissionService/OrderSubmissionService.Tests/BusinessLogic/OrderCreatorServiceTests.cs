using Moq;
using OrderSubmissionService.Models;
using OrderSubmissionService.BusinessLogic;

namespace OrderSubmissionService.Tests.BusinessLogic
{
    [TestClass]
    public class OrderCreatorServiceTests
    {
        protected readonly Mock<IGuidProvider> mockGuidProvider = new();
        protected readonly Mock<IDateTimeProvider> mockDateTimeProvider = new();

        [TestMethod]
        public void CreateOrder_WhenValidInputs_ThenOrderCreated()
        {
            // Arrange
            var orderCreatorService = new OrderCreatorService(mockGuidProvider.Object, mockDateTimeProvider.Object);
            var guid = Guid.NewGuid();
            var dateTime = DateTime.UtcNow;
            mockGuidProvider.Setup(gs => gs.NewGuid).Returns(guid);
            mockDateTimeProvider.Setup(dts => dts.UtcNow).Returns(dateTime);
            Order expectedOrder = new Order
            {
                Id = guid,
                CustomerName = "testCustomerName",
                Product = "testProduct",
                Status = "Pending",
                CreatedAt = dateTime,
            };

            // Act
            var actualOrder = orderCreatorService.CreateOrder("testCustomerName", "testProduct");

            // Assert
            Assert.AreEqual(expectedOrder, actualOrder);
        }
    }
}
