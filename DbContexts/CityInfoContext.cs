using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;

        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options): base(options)
        {
            
        }

        // /*** This override allows for seeding the database ***/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("Salt Lake City")
                {
                    Id = 1,
                    Description = "The one with the Crazy Liquor laws and Beautiful Mountains."
                },
                new City("Portland")
                {
                    Id = 2,
                    Description = "The one that I\'m moving to!!!"
                },
                new City("Anaheim")
                {
                    Id = 3,
                    Description = "The one with Disneyland!"
                }
            );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Delta Center")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "Home of the Utah Jazz!"
                },
                new PointOfInterest("Portland Zoo")
                {
                    Id = 2,
                    CityId = 2,
                    Description = "Has a lot of animals."
                },
                new PointOfInterest("Bridges")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "All the bridges over the rivers"
                },
                new PointOfInterest("Disneyland")
                {
                    Id = 4,
                    CityId = 3,
                    Description = "The happiest (and most expensive) place on earth."
                },
                new PointOfInterest("California Adventure")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "Right next to Disneyland, has a lot of other rides"
                }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}