using Microsoft.EntityFrameworkCore;
using Transaction.Core.Models;

namespace Transaction.Core.DataAccess;

public class TransactionDbContext: DbContext
{
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TransactionEntity>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Status).HasMaxLength(20);
        });
    }
}