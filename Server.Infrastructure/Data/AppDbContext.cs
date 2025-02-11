using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Infrastructure.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    #region DbSet
    public DbSet<Category> Category { get; set; }
    public DbSet<SubCategory> SubCategory { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<CardActivity> CardActivities { get; set; }

    #endregion
    //public string? GetConnectionString()
    //{
    //    IConfiguration configuration = new ConfigurationBuilder()
    //        .SetBasePath(Directory.GetCurrentDirectory())
    //        .AddJsonFile("appsettings.json", true, true)
    //        .Build();
    //    return configuration["ConnectionStrings:DefaultConnection"];
    //}
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{

    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlServer(GetConnectionString(), optionsBuilder => optionsBuilder.EnableRetryOnFailure());
    //        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    //    }

    //}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Role>().HasData(
           new Role { Id = 1, RoleName = "Admin" },
           new Role { Id = 2, RoleName = "User" },
           new Role { Id = 3, RoleName = "Staff" }
        );

        //User
        modelBuilder.Entity<User>()
        .Property(u => u.Status)
        .HasConversion(
            v => v.ToString(),
            v => (StatusEnum)Enum.Parse(typeof(StatusEnum), v)
        );

        modelBuilder.Entity<Board>()
            .Property(s => s.Type)
            .HasConversion(v => v.ToString(), v => (BoardType)Enum.Parse(typeof(BoardType), v));
        modelBuilder.Entity<Board>()
            .Property(s => s.Status)
            .HasConversion(v => v.ToString(), v => (BoardStatus)Enum.Parse(typeof(BoardStatus), v));
        modelBuilder.Entity<Board>()
            .HasOne(c => c.CreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict


        modelBuilder.Entity<Column>()
            .HasOne(c => c.CreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict
        modelBuilder.Entity<Column>()
            .Property(s => s.Status)
            .HasConversion(v => v.ToString(), v => (ColumnStatus)Enum.Parse(typeof(ColumnStatus), v));

        modelBuilder.Entity<Card>()
            .Property(s => s.Status)
            .HasConversion(v => v.ToString(), v => (CardStatus)Enum.Parse(typeof(CardStatus), v));
        modelBuilder.Entity<Card>()
            .HasOne(c => c.CreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict
    }
}
