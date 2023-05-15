using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Led_Screen.MVVM.Models.EFCore
{
    public class MyDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Server=localhost;Database=led_screen;User=visualstudio;Password=visualstudio");
        }
    }
}
