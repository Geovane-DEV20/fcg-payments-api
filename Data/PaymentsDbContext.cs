using FcgPaymentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FcgPaymentsApi.Data;

public class PaymentsDbContext : DbContext
{
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
    {
    }

    public DbSet<Payment> Payments { get; set; }
}
