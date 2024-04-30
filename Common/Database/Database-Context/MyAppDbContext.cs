using CourseWork.Modules.Admin.Entity;
using CourseWork.Modules.User.Entity;
using Microsoft.EntityFrameworkCore;

public class MyAppDbContext : DbContext
{
    //Ensure to add the DbSet for each entity
    //For Migration to work
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<AdminEntity> Admin { get; set; }

    public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}