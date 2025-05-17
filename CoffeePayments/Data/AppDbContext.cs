using Microsoft.EntityFrameworkCore;
using CoffeePaymentSystem.Models;

namespace CoffeePaymentSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Coworker> Coworkers { get; set; }
        public DbSet<PaymentHistoryEntry> PaymentHistory { get; set; }
    }
}