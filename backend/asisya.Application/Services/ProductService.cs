using asisya.Application.DTOs;
using asisya.Application.DTOs.Product;
using asisya.Application.Interfaces;
using asisya.Application.Mappers;
using asisya.Domain.Entities;
using asisya.Domain.Interfaces;

namespace asisya.Application.Services;

public class ProductService : IProductService
{
    private static readonly string[] ProductPrefixes = ["Alpha", "Beta", "Delta", "Omega", "Ultra", "Prime", "Nano", "Turbo", "Hyper", "Mega"];
    private static readonly string[] ProductSuffixes = ["Server", "Cloud", "Node", "Core", "Edge", "Hub", "Rack", "Blade", "Cluster", "Stack"];

    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<ProductSummaryDto>> GetPagedAsync(ProductFilterDto filter)
    {
        var (items, total) = await _repository.GetPagedAsync(
            filter.Page, filter.PageSize, filter.Search, filter.CategoryID, filter.Discontinued);

        return new PagedResultDto<ProductSummaryDto>
        {
            Items = items.Select(ProductMapper.ToSummaryDto),
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? null : ProductMapper.ToDto(product);
    }

    public async Task<int> GenerateAsync(CreateProductsDto dto)
    {
        var random = new Random();
        var products = Enumerable.Range(0, dto.Count).Select(_ => new Product
        {
            ProductName = $"{ProductPrefixes[random.Next(ProductPrefixes.Length)]} {ProductSuffixes[random.Next(ProductSuffixes.Length)]} {random.Next(1000, 9999)}",
            CategoryID = dto.CategoryID,
            UnitPrice = Math.Round((decimal)(random.NextDouble() * 9900 + 100), 2),
            UnitsInStock = (short)random.Next(0, 500),
            UnitsOnOrder = (short)random.Next(0, 100),
            ReorderLevel = (short)random.Next(0, 50),
            QuantityPerUnit = $"{random.Next(1, 100)} units",
            Discontinued = false
        });

        await _repository.AddRangeAsync(products);
        return dto.Count;
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return null;

        ProductMapper.ApplyUpdate(product, dto);
        await _repository.UpdateAsync(product);
        return ProductMapper.ToDto(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return false;

        await _repository.DeleteAsync(id);
        return true;
    }
}
