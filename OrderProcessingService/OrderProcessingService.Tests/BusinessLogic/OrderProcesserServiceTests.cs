using OrderProcessingService.BusinessLogic;
using OrderProcessingService.Models;

namespace OrderProcessingService.Tests.BusinessLogic
{
    [TestClass]
    public class OrderProcesserServiceTests
    {
        [TestMethod]
        public void CanBeProcessed_IfOrderIsNull_ThenReturnFalse()
        {
            // Arrange
            var orderProcesserService = new OrderProcesserService();

            // Act
            var result = orderProcesserService.CanBeProcessed(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeProcessed_IfCustomerNameOrProductIsWhitespace_ThenReturnFalse()
        {
            // Arrange
            var orderProcesserService = new OrderProcesserService();
            var product = "testProduct";
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "   ",
                Product = product,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
            };

            // Act
            var result = orderProcesserService.CanBeProcessed(order);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeProcessed_IfNotPending_ThenReturnFalse()
        {
            // Arrange
            var orderProcesserService = new OrderProcesserService();
            var customerName = "testCustomerName";
            var product = "testProduct";
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = customerName,
                Product = product,
                Status = "Processed",
                CreatedAt = DateTime.UtcNow,
            };

            // Act
            var result = orderProcesserService.CanBeProcessed(order);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeProcessed_IfValidOrder_ThenReturnTrue()
        {
            // Arrange
            var orderProcesserService = new OrderProcesserService();
            var customerName = "testCustomerName";
            var product = "testProduct";
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = customerName,
                Product = product,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
            };

            // Act
            var result = orderProcesserService.CanBeProcessed(order);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProcessOrder_WhenProcessableOrder_ThenProcessIt()
        {
            // Arrange
            var orderProcesserService = new OrderProcesserService();
            var guid = Guid.NewGuid();
            var dateTime = DateTime.UtcNow;
            var customerName = "testCustomerName";
            var product = "testProduct";
            var order = new Order
            {
                Id = guid,
                CustomerName = customerName,
                Product = product,
                Status = "Pending",
                CreatedAt = dateTime,
            };
            var expectedOrder = new Order
            {
                Id = guid,
                CustomerName = customerName,
                Product = product,
                Status = "Processed",
                CreatedAt = dateTime,
            };

            // Act
            orderProcesserService.ProcessOrder(order);

            // Assert
            Assert.AreEqual(expectedOrder, order);
        }
    }
}
