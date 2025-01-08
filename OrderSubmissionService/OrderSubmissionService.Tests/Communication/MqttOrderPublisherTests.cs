using Moq;
using MQTTnet;
using MQTTnet.Exceptions;
using OrderSubmissionService.BusinessLogic;
using OrderSubmissionService.Communication;
using OrderSubmissionService.Models;

namespace OrderSubmissionService.Tests.Communication
{
    [TestClass]
    public class MqttOrderPublisherTests
    {
        private readonly string _catchMessage = "Error communicating with the message queue: {0}{1}";
        private readonly Mock<IMqttClient> mockMqttClient = new();
        private readonly Mock<IOrderCreatorService> mockOrderCreatorService = new();
        private readonly Mock<Order> mockOrder = new();

        [TestMethod]
        public void PublishOrder_WhenConnectionProblems_ThenThrowMqttCommunicationException()
        {
            // Arrange
            var mqttOrderPublisher = new MqttOrderPublisher(mockMqttClient.Object, mockOrderCreatorService.Object);
            var mqttClientOptions = new MqttClientOptions();
            var customerName = "testCustomerName";
            var product = "testProduct";
            var exceptionMessage = "Communication error.";
            mockOrderCreatorService.Setup(ocs => ocs.CreateOrder(It.IsAny<string>(), It.IsAny<string>())).Returns(mockOrder.Object);
            mockMqttClient.Setup(mc => mc.ConnectAsync(It.IsAny<MqttClientOptions>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new MqttCommunicationException(exceptionMessage));
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            mqttOrderPublisher.PublishOrder(customerName, product).Wait();

            // Assert
            Assert.AreEqual(string.Format(_catchMessage, exceptionMessage, Environment.NewLine), output.ToString());
        }

        [TestMethod]
        public void PublishOrder_WhenCanConnect_ThenPublishOrder()
        {
            // Arrange
            var mqttOrderPublisher = new MqttOrderPublisher(mockMqttClient.Object, mockOrderCreatorService.Object);
            var mqttClientOptions = new MqttClientOptions();
            var customerName = "testCustomerName";
            var product = "testProduct";
            var guid = Guid.NewGuid();
            mockOrderCreatorService.Setup(ocs => ocs.CreateOrder(It.IsAny<string>(), It.IsAny<string>())).Returns(new Order
            {
                Id = guid,
                CustomerName = customerName,
                Product = product,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
            });

            // Act
            Task task = mqttOrderPublisher.PublishOrder(customerName, product);
            task.Wait();

            // Assert
            Assert.AreEqual(Task.CompletedTask, task);
        }
    }
}
