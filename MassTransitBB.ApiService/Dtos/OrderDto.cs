using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class OrderDto
{
    public OrderDto(string ConfirmationEmail, OrderItemDto[] OrderItems, PaymentInfoDto PaymentInfo)
    {
        this.ConfirmationEmail = ConfirmationEmail;
        this.OrderItems = OrderItems;
        this.PaymentInfo = PaymentInfo;
    }

    [Required]
    [MinLength(10)]
    [DefaultValue("test@microsoft.com")]
    public string ConfirmationEmail { get; set; }
    [Required]
    [MinLength(1)]
    public OrderItemDto[] OrderItems { get; set; }
    [Required]
    public PaymentInfoDto PaymentInfo { get; set; }
}
