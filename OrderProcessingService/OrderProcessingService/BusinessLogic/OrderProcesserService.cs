using OrderProcessingService.Models;

namespace OrderProcessingService.BusinessLogic
{
    public interface IOrderProcesserService
    {
        public void ProcessOrder(Order order);
        public Boolean CanBeProcessed(Order order);
    }

    public class OrderProcesserService : IOrderProcesserService
    {
        public void ProcessOrder(Order order)
        {
            Random random = new Random();
            int processingTime = 1000 + random.Next(2000);

            Thread.Sleep(processingTime);

            order.Status = "Processed";
        }

        public Boolean CanBeProcessed(Order order)
        {
            return order != null
                && !string.IsNullOrWhiteSpace(order.CustomerName)
                && !string.IsNullOrWhiteSpace(order.Product)
                && order.Status == "Pending";
        }
    }
}
