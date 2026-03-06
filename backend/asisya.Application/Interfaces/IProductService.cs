using asisya.Application.DTOs;
using asisya.Application.DTOs.Product;

namespace asisya.Application.Interfaces;

public interface IProductService
{
    Task<PagedResultDto<ProductSummaryDto>> GetPagedAsync(ProductFilterDto filter);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<int> GenerateAsync(CreateProductsDto dto);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}
