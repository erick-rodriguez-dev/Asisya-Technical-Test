namespace asisya.Application.DTOs.Product;

public class ProductFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string? Search { get; set; }
    public int? CategoryID { get; set; }
    public bool? Discontinued { get; set; }
}
