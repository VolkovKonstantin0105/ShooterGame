using Microsoft.EntityFrameworkCore;
using SGame.Models.Enemy;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace SGame.ViewModels
{
    //класс для работы с базой данных EF
    public class ApplicationContext : DbContext
    {
        public DbSet<PlayerEntity> Players => Set<PlayerEntity>(); //набор элементов, хранящиеся в базе данных, Players хранит в себе набор данных
        public ApplicationContext() => Database.EnsureCreated(); //создаю базу данных

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SGame");
            Directory.CreateDirectory(folder);
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(folder, "SGAME1.db")}");
        }
    }
}
