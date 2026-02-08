namespace ECommerce.Models.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public decimal SubTotal { get; set; }
}

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
}

public class AddCartItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateCartItemDto
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}
