using Microsoft.EntityFrameworkCore;
using Payment.Core.Models;

namespace Payment.Core.DataAccess.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentEntity> GetByIdAsync(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<List<PaymentEntity>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task AddAsync(PaymentEntity payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentEntity payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}