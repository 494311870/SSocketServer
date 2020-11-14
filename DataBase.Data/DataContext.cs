using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataBase.Domain;

namespace DataBase.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(ConsoleLoggerFactory)
                //.EnableSensitiveDataLogging()
                .UseMySQL("Database=db_tree_wars;Data Source=127.0.0.1;port=3306;User Id=syk;Password=suoyike;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
        }


        public DbSet<User> Users { get; set; }


        public static readonly ILoggerFactory ConsoleLoggerFactory =
            LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information);
            });
    }
}
