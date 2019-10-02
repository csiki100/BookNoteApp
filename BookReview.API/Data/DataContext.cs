using System.ComponentModel.DataAnnotations.Schema;
using BookReview.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReview.API.Data
{
    ///<summary>
    /// Class that is used to communicate with the database
    ///</summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        ///<summary>
        /// Property that represents the books in the database
        ///</summary>
        public DbSet<Book> Books { get; set; }

        ///<summary>
        /// Property that represents the users in the database
        ///</summary>
        public DbSet<User> Users { get; set; }

        ///<summary>
        /// Property that represents the pictures in the database
        ///</summary>
        public DbSet<Picture> Pictures { get; set; }

        ///<summary>
        /// Property that represents the chapters in the database
        ///</summary>
        public DbSet<Chapter> Chapters { get; set; }

        ///<summary>
        /// Property that represents the Book-User Many-Many Relations in the database
        ///</summary>
        public DbSet<Read> Reads { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Read entity Primary Key
            builder.Entity<Read>()
            .HasKey(r => new { r.UserId, r.BookId });


            //Read and User relationship
            builder.Entity<Read>()
            .HasOne(r => r.User)
            .WithMany(u => u.Books)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
            .HasMany(u => u.Books)
            .WithOne(r => r.User)
            .OnDelete(DeleteBehavior.Cascade);


            //Read and Book relationship
            builder.Entity<Read>()
            .HasOne(r => r.Book)
            .WithMany(b => b.UsersWhoRead)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Book>()
            .HasMany(b => b.UsersWhoRead)
            .WithOne(r => r.Book)
            .OnDelete(DeleteBehavior.Cascade);


            //Book and Picture relationship
            builder.Entity<Book>()
            .HasOne(b => b.Picture)
            .WithOne(p => p.Book)
            .OnDelete(DeleteBehavior.Cascade);

            //Book and Chapter relationship
            builder.Entity<Book>()
            .HasMany(b => b.Chapters)
            .WithOne(c => c.Book)
            .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}