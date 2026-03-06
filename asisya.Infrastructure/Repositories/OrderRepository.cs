using asisya.Domain.Entities;
using asisya.Domain.Interfaces;
using asisya.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace asisya.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id)
        => await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.Shipper)
            .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderID == id);

    public async Task<IEnumerable<Order>> GetByCustomerAsync(string customerId)
        => await _context.Orders
            .Where(o => o.CustomerID == customerId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Order> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}
