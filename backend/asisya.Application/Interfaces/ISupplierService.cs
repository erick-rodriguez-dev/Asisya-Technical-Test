using asisya.Application.DTOs.Supplier;

namespace asisya.Application.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
}
