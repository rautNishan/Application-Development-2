using CourseWork.Modules.User.Entity;
using Microsoft.EntityFrameworkCore;

public class MyAppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}