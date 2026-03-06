using asisya.Application.DTOs.Supplier;
using asisya.Application.Interfaces;
using asisya.Domain.Interfaces;

namespace asisya.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;

    public SupplierService(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _repository.GetAllAsync();
        return suppliers.Select(s => new SupplierDto
        {
            SupplierID = s.SupplierID,
            CompanyName = s.CompanyName,
            ContactName = s.ContactName,
            Country = s.Country
        });
    }
}
