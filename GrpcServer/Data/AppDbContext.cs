using Microsoft.EntityFrameworkCore;

namespace GrpcServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // This property represents the actual SQL table
    public DbSet<ChatMessageRecord> ChatMessages { get; set; }
}