using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Newtonsoft.Json;
using GrandTripAPI.Models;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GrandTripAPI.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var converter = new ValueConverter<double[][], string>(
                arr => JsonConvert.SerializeObject(arr),
                str => JsonConvert.DeserializeObject<double[][]>(str));

            builder.Entity<Line>()
                .Property(l => l.LatLngs)
                .HasConversion(converter);

            builder.Entity<RouteTheme>()
                .HasData(new RouteTheme { Name="Без тематики" });
            
            builder.Entity<RouteSeason>()
                .HasData(new RouteSeason { Name = "Все сезоны" });

            builder.Entity<User>()
                .HasMany(u => u.CreatedRoutes)
                .WithOne(r => r.Creator)
                .IsRequired();

            builder.Entity<User>()
                .HasMany(u => u.FavouriteRoutes)
                .WithMany(r => r.Preferers);
        }
        
        public AppDbContext()
        {
            //ReSharper disable VirtualMemberCallInConstructor
            Database.EnsureCreated();
        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Dot> Dots { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<RouteTheme> Themes { get; set; }
        public DbSet<RouteSeason> Seasons { get; set; }
        public DbSet<User> Users { get; set; }
    }
}