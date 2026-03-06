using asisya.Domain.Entities;
using asisya.Domain.Interfaces;
using asisya.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace asisya.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id)
        => await _context.Categories.FindAsync(id);

    public async Task<IEnumerable<Category>> GetAllAsync()
        => await _context.Categories.AsNoTracking().ToListAsync();

    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
}
