using System.ComponentModel.DataAnnotations;

namespace MassTransitBB.ApiService.Database
{
    public class Order
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Order()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
        }

        public Guid Id { get; }
        public string ConfirmationEmail { get; }
        public List<OrderItem> OrderItems { get; }
        public PaymentInfo PaymentInfo { get; }

        public Order(string ConfirmationEmail, IEnumerable<OrderItem> OrderItems, PaymentInfo PaymentInfo)
        {
            if (string.IsNullOrWhiteSpace(ConfirmationEmail))
            {
                throw new ArgumentException($"'{nameof(ConfirmationEmail)}' cannot be null or whitespace.", nameof(ConfirmationEmail));
            }

            if (OrderItems is null || !OrderItems.Any())
            {
                throw new ValidationException("Order must have at least one item");
            }
            Id = Guid.CreateVersion7();
            this.ConfirmationEmail = ConfirmationEmail;
            this.OrderItems = OrderItems.ToList();
            this.PaymentInfo = PaymentInfo ?? throw new ArgumentNullException(nameof(PaymentInfo));
        }
    }
}