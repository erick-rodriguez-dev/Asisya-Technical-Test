namespace asisya.Application.DTOs.Category;

public class CreateCategoryDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PictureBase64 { get; set; }
}
