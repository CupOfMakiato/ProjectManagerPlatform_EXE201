using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System.Reflection.Metadata;

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
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Checklist> Checklists { get; set; }
    public DbSet<Subcription> Subcriptions { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<Subcribe> Subcribes { get; set; }
    public DbSet<Payment> Payments { get; set; }

    #endregion
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>()
        .ToTable(tb => tb.UseSqlOutputClause(false));
        modelBuilder.Entity<Board>()
        .ToTable(tb => tb.UseSqlOutputClause(false));
        modelBuilder.Entity<Attachment>()
        .ToTable(tb => tb.UseSqlOutputClause(false));
        modelBuilder.Entity<Column>()
        .ToTable(tb => tb.UseSqlOutputClause(false));
        modelBuilder.Entity<Notification>()
        .ToTable(tb => tb.UseSqlOutputClause(false));

        modelBuilder.Entity<Role>().HasData(
           new Role { Id = 1, RoleName = "Admin" },
           new Role { Id = 2, RoleName = "User" }
           //new Role { Id = 3, RoleName = "Staff" }
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
            .HasOne(c => c.BoardCreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict


        modelBuilder.Entity<Column>()
            .HasOne(c => c.ColumnCreatedByUser)
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
            .Property(s => s.AssignedCompletion)
            .HasConversion(v => v.ToString(), v => (AssignedCompletion)Enum.Parse(typeof(AssignedCompletion), v));

        modelBuilder.Entity<Card>()
            .Property(s => s.Reminder)
            .HasConversion(v => v.ToString(), v => (ReminderType)Enum.Parse(typeof(ReminderType), v));

        modelBuilder.Entity<Card>()
            .HasOne(c => c.CardCreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict

        modelBuilder.Entity<Activity>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .Property(s => s.EntityType)
            .HasConversion(v => v.ToString(), v => (EntityType)Enum.Parse(typeof(EntityType), v));

        modelBuilder.Entity<Notification>()
            .Property(s => s.MessageType)
            .HasConversion(v => v.ToString(), v => (NotificationType)Enum.Parse(typeof(NotificationType), v));

        modelBuilder.Entity<Notification>()
            .HasOne(c => c.NotificationCreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict

        modelBuilder.Entity<Subcription>()
            .Property(s => s.SubcriptionName)
            .HasConversion(v => v.ToString(), v => (SubcriptionType)Enum.Parse(typeof(SubcriptionType), v));

        modelBuilder.Entity<Subcription>()
            .HasOne(c => c.SubcriptionCreatedBy)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict
    }
}
