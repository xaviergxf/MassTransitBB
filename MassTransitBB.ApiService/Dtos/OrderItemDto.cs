using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class OrderItemDto
{
    public OrderItemDto(Guid ProductId, int Quantity = 1)
    {
        this.ProductId = ProductId;
        this.Quantity = Quantity;
    }

    [DefaultValue(typeof(Guid), "883D3128-6A05-4712-BEE6-C4E2A260097B")]
    public Guid ProductId { get; }
    [Range(1, int.MaxValue)]
    public int Quantity { get; }
}
