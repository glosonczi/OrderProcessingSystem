using Moq;
using MQTTnet;
using MQTTnet.Exceptions;
using OrderProcessingService.BusinessLogic;
using OrderProcessingService.Communication;

namespace OrderProcessingService.Tests.Communication
{
    [TestClass]
    public class MqttOrderListenerTests
    {
        private readonly string _catchMessage = "Error communicating with the message queue: {0}{1}";
        private readonly Mock<IMqttClient> mockMqttClient = new();
        private readonly Mock<IOrderProcesserService> mockOrderProcesserService = new();

        [TestMethod]
        public void SubscribeToOrders_WhenConnectionProblems_ThenThrowMqttCommunicationException()
        {
            // Arrange
            var mqttOrderListener = new MqttOrderListener(mockMqttClient.Object, mockOrderProcesserService.Object);
            var exceptionMessage = "Communication error.";
            mockMqttClient.Setup(mc => mc.ConnectAsync(It.IsAny<MqttClientOptions>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new MqttCommunicationException(exceptionMessage));
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            mqttOrderListener.SubscribeToOrders().Wait();

            // Assert
            Assert.AreEqual(string.Format(_catchMessage, exceptionMessage, Environment.NewLine), output.ToString());
        }
        /*
        [TestMethod]
        public void SubscribeToOrders_WhenCanConnectAndOrderIsValid_ThenProcessOrder()
        {           

        }*/
    }
}
