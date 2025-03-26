using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class PaymentInfoDto
{
    [Required]
    [CreditCard]
    [DefaultValue("378282246310005")]
    public required string CardNumber { get; set; }
    [Required]
    [StringLength(7, MinimumLength = 7)]
    [DefaultValue("10/2028")]
    public required string ExpiryDate { get; set; } = DateTime.Now.ToString("MM/yy");
    [Required]
    [StringLength(3, MinimumLength = 3)]
    [DefaultValue("123")]
    public required string CVV { get; set; }
}
