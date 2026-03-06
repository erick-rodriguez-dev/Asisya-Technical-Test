using asisya.Application.DTOs.Product;
using asisya.Domain.Entities;

namespace asisya.Application.Mappers;

public static class ProductMapper
{
    public static ProductDto ToDto(Product entity) => new()
    {
        ProductID = entity.ProductID,
        ProductName = entity.ProductName,
        QuantityPerUnit = entity.QuantityPerUnit,
        UnitPrice = entity.UnitPrice,
        UnitsInStock = entity.UnitsInStock,
        UnitsOnOrder = entity.UnitsOnOrder,
        ReorderLevel = entity.ReorderLevel,
        Discontinued = entity.Discontinued,
        CategoryID = entity.CategoryID,
        CategoryName = entity.Category?.CategoryName,
        CategoryPictureBase64 = entity.Category?.Picture is not null
            ? Convert.ToBase64String(entity.Category.Picture)
            : null,
        SupplierID = entity.SupplierID,
        SupplierName = entity.Supplier?.CompanyName
    };

    public static ProductSummaryDto ToSummaryDto(Product entity) => new()
    {
        ProductID = entity.ProductID,
        ProductName = entity.ProductName,
        UnitPrice = entity.UnitPrice,
        UnitsInStock = entity.UnitsInStock,
        Discontinued = entity.Discontinued,
        CategoryName = entity.Category?.CategoryName,
        SupplierName = entity.Supplier?.CompanyName
    };

    public static void ApplyUpdate(Product entity, UpdateProductDto dto)
    {
        if (dto.ProductName is not null) entity.ProductName = dto.ProductName;
        if (dto.QuantityPerUnit is not null) entity.QuantityPerUnit = dto.QuantityPerUnit;
        if (dto.UnitPrice is not null) entity.UnitPrice = dto.UnitPrice;
        if (dto.UnitsInStock is not null) entity.UnitsInStock = dto.UnitsInStock;
        if (dto.UnitsOnOrder is not null) entity.UnitsOnOrder = dto.UnitsOnOrder;
        if (dto.ReorderLevel is not null) entity.ReorderLevel = dto.ReorderLevel;
        if (dto.Discontinued is not null) entity.Discontinued = dto.Discontinued.Value;
        if (dto.CategoryID is not null) entity.CategoryID = dto.CategoryID;
        if (dto.SupplierID is not null) entity.SupplierID = dto.SupplierID;
    }
}
