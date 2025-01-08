namespace OrderProcessingService.Models
{
    public class Order
    {
        public required Guid Id { get; set; }
        public required string CustomerName { get; set; }
        public required string Product { get; set; }
        public required string Status { get; set; }
        public required DateTime CreatedAt { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            Order other = obj as Order;

            return this.Id == other.Id
                && this.CustomerName == other.CustomerName
                && this.Product == other.Product
                && this.Status == other.Status
                && this.CreatedAt == other.CreatedAt;
        }
    }
}
