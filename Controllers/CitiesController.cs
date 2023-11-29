using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")] // Putting [controller] will match the name of the controller

    /*** This is for the Entity Framework Core Repository approach ***/
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities([FromQuery] string? name, [FromQuery] string? searchQuery)
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync(name, searchQuery);

            /*** This uses AutoMapper ***/
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));

            /*** This is the long way to map Entities to Dto ***/
            // var results = new List<CityWithoutPointsOfInterestDto>();
            // foreach(var cityEntity in cityEntities)
            // {
            //     results.Add(new CityWithoutPointsOfInterestDto
            //     {
            //         Id = cityEntity.Id,
            //         Description = cityEntity.Description,
            //         Name = cityEntity.Name
            //     });
            // }
        }

        [HttpGet("{id}", Name = "GetCity")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            return includePointsOfInterest
                ? Ok(_mapper.Map<CityDto>(city))
                : Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }

        [HttpPost]
        public async Task<ActionResult<CityDto>> AddCity([FromBody] CityForCreationDto city)
        {
            var finalCity = _mapper.Map<Entities.City>(city);

            _cityInfoRepository.AddCity(finalCity);

            await _cityInfoRepository.SaveChangesAsync();

            var createdCityToReturn = _mapper.Map<Models.CityDto>(finalCity);

            return CreatedAtRoute("GetCity",
            new
            {
                createdCityToReturn.Id,
                createdCityToReturn.Name,
                createdCityToReturn.Description
            },
            createdCityToReturn
            );
        }
    }



    /*** This commented out section is for the in-memory DataStore
    // public class CitiesController: ControllerBase //contains basic info for controllers
    // {
    //     private readonly CitiesDataStore _citiesDataStore;

    //     public CitiesController(CitiesDataStore citiesDataStore)
    //     {
    //         _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
    //     }

    //     [HttpGet]
    //     public ActionResult<IEnumerable<CityDto>> GetCities()
    //     {
    //         return Ok(_citiesDataStore.Cities);
    //     }

    //     /*** This is how you return resources through a controller with a model for JSON only ***/
    //     // [HttpGet]
    //     // public JsonResult GetCities()
    //     // {
    //     //     return new JsonResult(CitiesDataStore.Current.Cities);
    //     // }

    //     /*** This is how you return a single city by Id***/
    //     [HttpGet("{id}")] // the {id} will get the param id from the uri
    //     public ActionResult<CityDto> GetCity(int id)
    //     {

    //         // Find the City
    //         var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);

    //         return cityToReturn == null ? NotFound() : Ok(cityToReturn);

    //     }

    //     /*** This is how you would manually determin what is going to be sent back
    //     *** Without a controller/model/DTO
    //     ***/

    //     // [HttpGet]
    //     // public JsonResult GetCities()
    //     // {
    //     //     return new JsonResult(
    //     //         new List<object> {
    //     //             new { id = 1, Name = "Salt Lake City" },
    //     //             new { id = 2, Name = "Portland"}
    //     //         }
    //     //     );
    //     // }
    // }
}