using Microsoft.EntityFrameworkCore;
using Payment.Core.Models;

namespace Payment.Core.DataAccess;

public class PaymentDbContext: DbContext
{
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PaymentEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        });
    }
}