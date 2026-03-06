namespace asisya.Application.DTOs.Product;

public class ProductSummaryDto
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public bool Discontinued { get; set; }
    public string? CategoryName { get; set; }
    public string? SupplierName { get; set; }
}
