namespace asisya.Application.DTOs.Supplier;

public class SupplierDto
{
    public int SupplierID { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? Country { get; set; }
}
