using OrderSubmissionService.Communication;

namespace OrderSubmissionService.Presentation
{
    public class OrderSubmissionCLI
    {
        private readonly IMqttOrderPublisher _mqttOrderPublisher;

        public OrderSubmissionCLI(IMqttOrderPublisher mqttOrderPublisher)
        {
            _mqttOrderPublisher = mqttOrderPublisher;
        }

        public void HandleSubmission(string[] args)
        {
            if (args.Length < 3 || args[0] != "order")
            {
                Console.WriteLine("Usage: dotnet run -- order \"CustomerName\" \"Product\"");

                return;
            }

            var customerName = args[1];
            var product = args[2];

            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(product))
            {
                Console.WriteLine("Customer name and product cannot be empty!");

                return;
            }

            _mqttOrderPublisher.PublishOrder(customerName, product).Wait();
        }
    }
}
