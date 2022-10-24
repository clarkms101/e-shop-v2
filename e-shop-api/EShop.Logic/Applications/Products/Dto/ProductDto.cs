namespace EShop.Logic.Applications.Products.Dto;

public class ProductDto
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string? Category { get; set; }

    public string? Content { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsEnabled { get; set; }

    public decimal OriginPrice { get; set; }

    public decimal Price { get; set; }

    public string? Title { get; set; }

    public string? Unit { get; set; }

    public int Num { get; set; }
}