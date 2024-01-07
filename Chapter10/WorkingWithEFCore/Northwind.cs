using Microsoft.EntityFrameworkCore;

namespace Shared
{
    public class Northwind : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            if (ProjectConstants.DatabaseProvider == "SQLite")
            {
                string path = Path.Combine(Environment.CurrentDirectory, "Northwind.db");

                Console.WriteLine($"Using {path} database file.");

                optionsBuilder.UseSqlite($"Filename={path}");
            }
            else
            {
                string connection = "Data Source=.;" +
                    "Initial Catalog=Northwind;" +
                    "Integrated Security=true" +
                    "MultipleActiveResultSets=true;";

                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(category => category.CategoryName)
                .IsRequired()
                .HasMaxLength(15);

            if (ProjectConstants.DatabaseProvider == "SQLite")
            {
                // added to "fix" the lack of decimal support in SQLite
                modelBuilder.Entity<Product>()
                    .Property(product => product.Cost)
                    .HasConversion<double>();
            }

            // global filter to remove discontinued products
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.Discontinued);
        }

    }
}
