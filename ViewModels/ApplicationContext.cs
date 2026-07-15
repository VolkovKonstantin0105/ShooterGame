using Microsoft.EntityFrameworkCore;
using SGame.Models;
namespace SGame.ViewModels
{
    //класс для работы с базой данных EF
    public class ApplicationContext : DbContext
    {
        public DbSet<Player> Players => Set<Player>(); //набор элементов, хранящиеся в базе данных, Players хранит в себе набор данных
        public ApplicationContext() => Database.EnsureCreated(); //создаю базу данных

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SGAME.db");
        }
    }
}
