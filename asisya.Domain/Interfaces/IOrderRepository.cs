using asisya.Domain.Entities;

namespace asisya.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByCustomerAsync(string customerId);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
}
