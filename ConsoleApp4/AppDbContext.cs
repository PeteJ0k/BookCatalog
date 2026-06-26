using Microsoft.EntityFrameworkCore;
using System;
using ConsoleApp4;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-3059HVL;Database=BookCatalogDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<PaperBook>("PaperBook")
                .HasValue<EBook>("EBook");
        }
    }
}
    