using asisya.Domain.Entities;

namespace asisya.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null, int? categoryId = null, bool? discontinued = null);
    Task<Product> AddAsync(Product product);
    Task AddRangeAsync(IEnumerable<Product> products);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
