using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WinCordApi.Models;

namespace WinCordApi.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "port=3306;" +
                "user=root;" +
                "password=admin123;" +
                "database=WinCord;",
            ServerVersion.Parse("5.7.33-winx64")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasData(
                new Message { Id = 1, Content = "Test", UserId = null, Username = null},
                new Message { Id = 2, Content = "Test", UserId = 1, Username = "Lorem" });

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Tom", Password = "1234"});
        }

        public DbSet<WinCordApi.Models.User> User { get; set; } = default!;
    }
}
