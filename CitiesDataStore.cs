using System.Drawing;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        //public static CitiesDataStore Current { get; } = new CitiesDataStore(); // Commenting out once we know about DI so that we can inject the service

        public CitiesDataStore()
        {
            // Dummy data
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Salt Lake City",
                    Description = "The one with the crazy liquor laws and beautiful mountains",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto() {
                            Id = 1,
                            Name = "Lagoon",
                            Description = "A Themepark with rollercoasters"
                        },
                        new PointOfInterestDto() {
                            Id = 2,
                            Name = "Great Salt Lake",
                            Description = "A Lake with no outlets and is salty as all hell"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Portland",
                    Description = "The one that I\'m moving to!!!",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto() {
                            Id = 3,
                            Name = "Portland Zoo",
                            Description = "Has a lot of animals"
                        },
                        new PointOfInterestDto() {
                            Id = 4,
                            Name = "Bridges",
                            Description = "All the bridges over the rivers"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Anaheim",
                    Description = "The one with Disneyland",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto() {
                            Id = 5,
                            Name = "Disneyland",
                            Description = "The happiest (and most expensive) place on earth"
                        },
                        new PointOfInterestDto() {
                            Id = 6,
                            Name = "California Adventure",
                            Description = "Right next to Disneyland, has a lot of other rides"
                        }
                    }
                }
            };
        }
    }
}