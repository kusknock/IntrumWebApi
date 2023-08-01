using IntrumWebApi.Models;
using IntrumWebApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ItrumWebApi.Models
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Token> Tokens { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
