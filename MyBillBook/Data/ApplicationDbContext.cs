using Microsoft.EntityFrameworkCore;
using MyBillBook.Models;
using MyBillBook.Users;

namespace MyBillBook.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base( options) 
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Sales> sales { get; set; }
    }
}
