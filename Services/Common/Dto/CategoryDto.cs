namespace Dto;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public override string ToString() => Name;
}

public class CategoryCreateDto
{
    public string Name { get; set; } = string.Empty;
}

public class CategoryUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}