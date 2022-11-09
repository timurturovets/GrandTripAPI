using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using GrandTripAPI.Models;

namespace GrandTripAPI.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var converter = new ValueConverter<double[], string>(
                arr=>"["+string.Join(",", arr)+"]",
                str=>str    
                    .TrimStart('[')
                    .TrimEnd(']')
                    .Split("s", StringSplitOptions.RemoveEmptyEntries)
                    .Select(double.Parse)
                    .ToArray());
            
            builder.Entity<Line>()
                .Property(l => l.LatLngs)
                .HasConversion(converter);
        }
        
        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Route> Routes { get; set; }
    }
}