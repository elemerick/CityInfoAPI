
using Microsoft.EntityFrameworkCore;

namespace PizzaStore.Models
{
    public class PizzaContext : DbContext
    {
        public PizzaContext(DbContextOptions option) : base(option) { }

        public DbSet<Pizza> Pizzas { get; set; }
    }
}
