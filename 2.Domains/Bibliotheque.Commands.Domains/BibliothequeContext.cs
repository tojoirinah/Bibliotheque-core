
using System.IO;

using Bibliotheque.Commands.Domains.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bibliotheque.Commands.Domains
{
    public interface IBibliothequeContext { }

    public class BibliothequeContext : DbContext, IBibliothequeContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }


        static public IConfigurationRoot Configuration { get; set; }

        public BibliothequeContext(DbContextOptions<BibliothequeContext> options) : base(options)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("BibliothequeConnection"));
            }
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class WebApiCoreContextDbFactory : IDesignTimeDbContextFactory<BibliothequeContext>
    {
        BibliothequeContext IDesignTimeDbContextFactory<BibliothequeContext>.CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<BibliothequeContext>();
            optionsBuilder.UseSqlServer<BibliothequeContext>(configuration.GetConnectionString("BibliothequeConnection"));

            return new BibliothequeContext(optionsBuilder.Options);

        }
    }
}
