using CalculatorNS.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatorNS.Context;

public class ApplicationContext : DbContext
{
    public DbSet<History> Histories {get;set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=db.sqlite");
    }
}