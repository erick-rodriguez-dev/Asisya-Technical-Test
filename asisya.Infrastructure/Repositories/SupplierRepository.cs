using asisya.Domain.Entities;
using asisya.Domain.Interfaces;
using asisya.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace asisya.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(int id)
        => await _context.Suppliers.FindAsync(id);

    public async Task<IEnumerable<Supplier>> GetAllAsync()
        => await _context.Suppliers.AsNoTracking().ToListAsync();

    public async Task<Supplier> AddAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }
}
