namespace OrderSubmissionService.Models
{
    public class Order
    {
        public required Guid Id { get; init; }
        public required string CustomerName { get; init; }
        public required string Product { get; init; }
        public required string Status { get; init; }
        public required DateTime CreatedAt { get; init; }

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;

            Order other = obj as Order;

            return this.Id == other.Id
                && this.CustomerName == other.CustomerName
                && this.Product == other.Product
                && this.Status == other.Status
                && this.CreatedAt == other.CreatedAt;
        }
    }
}
