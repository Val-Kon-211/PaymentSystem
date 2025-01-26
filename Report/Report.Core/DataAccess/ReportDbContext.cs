using Microsoft.EntityFrameworkCore;
using Report.Core.Models;

namespace Report.Core.DataAccess;

public class ReportDbContext: DbContext
{
    public DbSet<ReportEntity> Reports { get; set; }
    
    public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ReportEntity>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Title).IsRequired().HasMaxLength(100);
            entity.Property(r => r.Content).IsRequired();
        });
    }
}