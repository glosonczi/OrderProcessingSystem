using MQTTnet;
using MQTTnet.Exceptions;
using Newtonsoft.Json;
using OrderSubmissionService.BusinessLogic;

namespace OrderSubmissionService.Communication
{
    public interface IMqttOrderPublisher
    {
        public Task PublishOrder(string customerName, string product);
    }

    public class MqttOrderPublisher : IMqttOrderPublisher
    {
        private readonly IMqttClient _mqttClient;
        private readonly IOrderCreatorService _orderCreatorService;

        public MqttOrderPublisher(IMqttClient mqttClient, IOrderCreatorService orderCreatorService)
        {
            _mqttClient = mqttClient;
            _orderCreatorService = orderCreatorService;
        }

        public async Task PublishOrder(string customerName, string product)
        {
            var order = _orderCreatorService.CreateOrder(customerName, product);
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.hivemq.com")
                .Build();

            try
            {
                await _mqttClient.ConnectAsync (mqttClientOptions, CancellationToken.None);

                Console.WriteLine("Connected to message queue.");

                var messagePayload = JsonConvert.SerializeObject(order);
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("assignment/orderprocessingsystem/orders")
                    .WithPayload(messagePayload)
                    .Build();

                await _mqttClient.PublishAsync (applicationMessage, CancellationToken.None);

                Console.WriteLine("Order submitted!");
                Console.WriteLine($"Order ID: {order.Id}");

            }
            catch (MqttCommunicationException ex)
            {
                Console.WriteLine($"Error communicating with the message queue: {ex.Message}");
            }
            finally
            {
                if (_mqttClient.IsConnected)
                {
                    await _mqttClient.DisconnectAsync();
                }
            }
        }
    }
}
