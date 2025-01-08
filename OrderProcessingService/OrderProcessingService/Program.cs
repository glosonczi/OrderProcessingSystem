using MQTTnet;
using OrderProcessingService.BusinessLogic;
using OrderProcessingService.Communication;
using OrderProcessingService.Presentation;

namespace OrderProcessingService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mqttFactory = new MqttClientFactory();
            var mqttClient = mqttFactory.CreateMqttClient();
            var orderProcesserService = new OrderProcesserService();
            var mqttOrderListener = new MqttOrderListener(mqttClient, orderProcesserService);
            var orderProcessingCLI = new OrderProcessingCLI(mqttOrderListener);

            orderProcessingCLI.HandleProcessing();
        }
    }
}
