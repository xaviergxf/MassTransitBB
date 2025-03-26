namespace MassTransitBB.ApiService.Database
{
    public class PaymentInfo
    {
        public string CardNumber { get; }
        public string ExpiryDate { get; }
        public string CVV { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private PaymentInfo()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
        }

        public PaymentInfo(string CardNumber, string ExpiryDate, string CVV)
        {
            if (string.IsNullOrWhiteSpace(CardNumber))
            {
                throw new ArgumentException($"'{nameof(CardNumber)}' cannot be null or whitespace.", nameof(CardNumber));
            }

            if (string.IsNullOrWhiteSpace(ExpiryDate))
            {
                throw new ArgumentException($"'{nameof(ExpiryDate)}' cannot be null or whitespace.", nameof(ExpiryDate));
            }

            if (string.IsNullOrWhiteSpace(CVV))
            {
                throw new ArgumentException($"'{nameof(CVV)}' cannot be null or whitespace.", nameof(CVV));
            }

            this.CardNumber = CardNumber;
            this.ExpiryDate = ExpiryDate;
            this.CVV = CVV;
        }
    }
}