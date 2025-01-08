using OrderProcessingService.Communication;

namespace OrderProcessingService.Presentation
{
    public class OrderProcessingCLI
    {
        private readonly IMqttOrderListener _mqttOrderListener;

        public OrderProcessingCLI(IMqttOrderListener mqttOrderListener)
        {
            _mqttOrderListener = mqttOrderListener;
        }

        public void HandleProcessing()
        {
            Console.WriteLine("Order processing service started.");

            _mqttOrderListener.SubscribeToOrders().Wait();
        }
    }
}
