using BookReview.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReview.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Chapter> Chapters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Reads>()
            .HasKey(r => new { r.UserId, r.BookId });

            builder.Entity<Reads>()
            .HasOne(r => r.User)
            .WithMany(u => u.Books)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reads>()
            .HasOne(r => r.Book)
            .WithMany(b => b.UsersWhoRead)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Chapter>()
            .HasKey(c => new { c.Id, c.BookId });

            builder.Entity<Picture>()
            .HasKey(p => new { p.Id, p.BookId });

            builder.Entity<Book>()
            .HasOne(b => b.Picture)
            .WithOne(p => p.Book)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
            .HasMany(b => b.Chapters)
            .WithOne(c => c.Book)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
            .HasMany(b => b.UsersWhoRead)
            .WithOne(r => r.Book)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}