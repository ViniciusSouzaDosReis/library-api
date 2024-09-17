using library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace library.Infrastructure.DataAccess;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext() : base() { }
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        
        options.UseMySql(Environment.GetEnvironmentVariable("ConnectionStrings__Connection"), new MySqlServerVersion(new Version(8, 0, 33)));
    }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookImage> BookImages { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Token> Tokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>()
            .HasOne(b => b.BookImage)
            .WithOne(bi => bi.Book)
            .HasForeignKey<BookImage>(bi => bi.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Reservations)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Reservations)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.CurrentBook)
            .WithMany()
            .HasForeignKey(u => u.CurrentBookId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasData(
                // user1234567890
                new User()
                {
                    Id = Guid.Parse("9ea510f4-16e1-452d-a1be-da46858c4806"),
                    Email = "user@user.com",
                    FirstName = "User",
                    LastName = "User",
                    Password = "$2a$12$aA14dj/vrVmzzbHHdWVtNuUdUIoedJuy31YmjU8QA1dbTEtUKqBXi",
                    Role = Domain.Enums.RoleType.User,
                },

                // admin1234567890
                new User()
                {
                    Id = Guid.Parse("f4b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),
                    Email = "admin@admin.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Password = "$2a$12$qPfMvRgwmnCTA25sZpcIv.VTkUYhYr9Em562J5G0RiPJ2q5fjTBeW",
                    Role = Domain.Enums.RoleType.Admin,
                }
            );
    }
}
