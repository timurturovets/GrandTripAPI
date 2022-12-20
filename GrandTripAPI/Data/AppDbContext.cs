using Microsoft.EntityFrameworkCore;
using GrandTripAPI.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GrandTripAPI.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureConversionsAndSeedData(builder);
            ConfigureRelations(builder);
            ConfigureAutoIncludes(builder);
        }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //ReSharper disable VirtualMemberCallInConstructor
            //Database.EnsureCreated();
        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Dot> Dots { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<RouteTheme> Themes { get; set; }
        public DbSet<RouteSeason> Seasons { get; set; }
        public DbSet<User> Users { get; set; }

        private static void ConfigureAutoIncludes(ModelBuilder builder)
        {
            builder.Entity<Route>()
                .Navigation(r => r.Creator).AutoInclude();

            builder.Entity<Route>()
                .Navigation(r => r.Theme).AutoInclude();
            
            builder.Entity<Route>()
                .Navigation(r=>r.Season).AutoInclude();
        }

        private static void ConfigureRelations(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.CreatedRoutes)
                .WithOne(r => r.Creator)
                .IsRequired();

            builder.Entity<Route>()
                .HasMany(r => r.Dots)
                .WithOne(d => d.Route)
                .IsRequired();
        }

        private static void ConfigureConversionsAndSeedData(ModelBuilder builder)
        {
            builder.Entity<RouteTheme>()
                .HasData(new RouteTheme { Id = 1, Key="none", Name="Без тематики" });
            
            builder.Entity<RouteSeason>()
                .HasData(new RouteSeason { Id = 1, Key="none", Name = "Все сезоны" });

            var user = User.CreateNew("teletraan", "123456a");
            user.Id = 1; user.Role = "Admin";
            
            builder.Entity<User>()
                .HasData(user);
        }
        
    }
}