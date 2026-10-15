using Microsoft.EntityFrameworkCore;
using PlayerEntity;
using SGame.Models.Enemy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace SGame.ViewModels
{
    //класс для работы с базой данных EF
    public class ApplicationContext : DbContext
    {
        public DbSet<PlayerEntity.PlayerEntity> Players => Set<PlayerEntity.PlayerEntity>(); //набор элементов, хранящиеся в базе данных, Players хранит в себе набор данных
        public ApplicationContext() => Database.EnsureCreated(); //создаю базу данных

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SGAME1.db");
        }
    }
}
