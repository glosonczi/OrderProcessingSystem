using System.Text;
using MQTTnet;
using MQTTnet.Exceptions;
using Newtonsoft.Json;
using OrderProcessingService.BusinessLogic;
using OrderProcessingService.Models;

namespace OrderProcessingService.Communication
{
    public interface IMqttOrderListener
    {
        public Task SubscribeToOrders();
    }

    public class MqttOrderListener : IMqttOrderListener
    {
        private readonly IMqttClient _mqttClient;
        private readonly IOrderProcesserService _orderProcesserService;

        public MqttOrderListener(IMqttClient mqttClient, IOrderProcesserService orderProcesserService)
        {
            _mqttClient = mqttClient;
            _orderProcesserService = orderProcesserService;
        }

        public async Task SubscribeToOrders()
        {
            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                Order order = JsonConvert.DeserializeObject<Order>(Encoding.Default.GetString(e.ApplicationMessage.Payload));
                ProcessOrder(order);

                return Task.CompletedTask;
            };

            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.hivemq.com")
                .Build();

            try
            {
                await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                Console.WriteLine("Connected to message queue.");

                var mqttSubscribeOptions = new MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter("assignment/orderprocessingsystem/orders")
                    .Build();

                await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine("Subscribed to topic.");

                await Task.Delay(Timeout.Infinite);
            }
            catch (MqttCommunicationException ex)
            {
                Console.WriteLine($"Error communicating with the message queue: {ex.Message}");
            }
        }

        private void ProcessOrder(Order order)
        {
            if (_orderProcesserService.CanBeProcessed(order))
            {
                Console.WriteLine("Order received!");

                Console.WriteLine($"Processing order: ID = {order.Id}, Customer = {order.CustomerName}, Product = {order.Product}");

                _orderProcesserService.ProcessOrder(order);

                Console.WriteLine($"Order processed: ID = {order.Id}, Status = {order.Status}");
            }
        }
    }
}
