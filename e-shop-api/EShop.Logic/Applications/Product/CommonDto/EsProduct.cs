namespace EShop.Logic.Applications.Product.CommonDto;

public class EsProduct
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int CategoryId { get; set; }
    public string Category { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public string? Content { get; set; }

    public bool IsEnabled { get; set; }
}