namespace Dto;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public CategoryDto Category { get; set; }
    public int CategoryId { get; set; }
}

public class ProductCreateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal Quantity { get; set; }
}

public class ProductUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public int CategoryId { get; set; }
}