using OrderSubmissionService.Models;

namespace OrderSubmissionService.BusinessLogic
{
    public interface IOrderCreatorService
    {
        public Order CreateOrder(string customerName, string product);
    }

    public class OrderCreatorService : IOrderCreatorService
    {
        private readonly IGuidProvider _guidProvider;
        private readonly IDateTimeProvider _dateTimeProvider;

        public OrderCreatorService(IGuidProvider guidProvider, IDateTimeProvider dateTimeProvider)
        {
            _guidProvider = guidProvider;
            _dateTimeProvider = dateTimeProvider;
        }

        public Order CreateOrder(string customerName, string product)
        {
            return new Order
            {
                Id = _guidProvider.NewGuid,
                CustomerName = customerName,
                Product = product,
                Status = "Pending",
                CreatedAt = _dateTimeProvider.UtcNow,
            };
        }
    }
}
