using MQTTnet;
using OrderSubmissionService.BusinessLogic;
using OrderSubmissionService.Communication;
using OrderSubmissionService.Presentation;

namespace OrderSubmissionService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mqttFactory = new MqttClientFactory();
            var mqttClient = mqttFactory.CreateMqttClient();
            var guidProvider = new GuidProvider();
            var dateTimeProvider = new DateTimeProvider();
            var orderCreatorService = new OrderCreatorService(guidProvider, dateTimeProvider);
            var mqttOrderPublisher = new MqttOrderPublisher(mqttClient, orderCreatorService);
            var orderSubmissionCLI = new OrderSubmissionCLI(mqttOrderPublisher);

            orderSubmissionCLI.HandleSubmission(args);
        }
    }
}
