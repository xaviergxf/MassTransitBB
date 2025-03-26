namespace MassTransitBB.ApiService.Database
{
    public class OrderItem
    {
        private Guid _id;
        public Guid ProductId { get; }
        public int Quantity { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private OrderItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
        }

        public OrderItem(Guid ProductId, int Quantity)
        {
            if(Quantity <=0 )
                throw new ArgumentNullException(nameof(Quantity), "Cannot be zero or negative");
            _id = Guid.CreateVersion7();
            this.ProductId = ProductId;
            this.Quantity = Quantity;
        }
    }
}