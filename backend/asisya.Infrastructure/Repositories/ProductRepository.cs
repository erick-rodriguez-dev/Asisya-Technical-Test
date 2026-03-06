using asisya.Domain.Entities;
using asisya.Domain.Interfaces;
using asisya.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace asisya.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private const int BatchSize = 1000;
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
        => await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductID == id);

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null, int? categoryId = null, bool? discontinued = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => EF.Functions.ILike(p.ProductName, $"%{search}%"));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryID == categoryId);

        if (discontinued.HasValue)
            query = query.Where(p => p.Discontinued == discontinued.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.ProductID)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task AddRangeAsync(IEnumerable<Product> products)
    {
        var batches = products
            .Select((p, i) => new { Product = p, Index = i })
            .GroupBy(x => x.Index / BatchSize, x => x.Product);

        foreach (var batch in batches)
        {
            await _context.Products.AddRangeAsync(batch);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Products
            .Where(p => p.ProductID == id)
            .ExecuteDeleteAsync();
    }
}
