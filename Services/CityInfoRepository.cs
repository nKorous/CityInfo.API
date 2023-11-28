using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {

        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync(); // Returns the cities async in a list, ordered by Name
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if(includePointsOfInterest)
            {
                // IF includePointsOfInterest, then return it with the city that matches the cityId
                return await _context.Cities.Include(c => c.PointsOfIntrest).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            // IF !includePointsOfInterest, only return the city
            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointsOfInterest.Where(p => p.Id == pointOfInterestId && p.CityId == cityId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }
    }
}