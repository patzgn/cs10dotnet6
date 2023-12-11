using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Common.DataContext.Sqlite
{
    public static class NorthwindContextExtensions
    {
        /// <summary>
        /// Adds NorthwindContext to the specified IServiceCollection. Uses the Sqlite database provider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="relativePath">Set to override the default of ".."</param>
        /// <returns>An IServiceCollection that can be used to add more services.</returns>
        public static IServiceCollection AddNorthwindContext(this IServiceCollection services, string relativePath = "..")
        {
            string databasePath = Path.Combine(relativePath, "Northwind.db");

            services.AddDbContext<NorthwindContext>(options =>
                options.UseSqlite($"Data Source={databasePath}")
            );

            return services;
        }
    }
}
