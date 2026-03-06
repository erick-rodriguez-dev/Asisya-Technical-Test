using asisya.Domain.Entities;

namespace asisya.Domain.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(int id);
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier> AddAsync(Supplier supplier);
}
