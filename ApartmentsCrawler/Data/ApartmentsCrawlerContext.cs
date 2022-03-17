using ApartmentsCrawler.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentsCrawler.Data;

public class ApartmentsCrawlerContext : DbContext
{
    public ApartmentsCrawlerContext()
    {
    }

    public ApartmentsCrawlerContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    public DbSet<Page> Pages { get; set; }
}