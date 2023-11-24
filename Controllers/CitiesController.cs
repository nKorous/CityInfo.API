using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")] // Putting [controller] will match the name of the controller
    public class CitiesController: ControllerBase //contains basic info for controllers
    {
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_citiesDataStore.Cities);
        }

        /*** This is how you return resources through a controller with a model for JSON only ***/
        // [HttpGet]
        // public JsonResult GetCities()
        // {
        //     return new JsonResult(CitiesDataStore.Current.Cities);
        // }

        /*** This is how you return a single city by Id***/
        [HttpGet("{id}")] // the {id} will get the param id from the uri
        public ActionResult<CityDto> GetCity(int id)
        {

            // Find the City
            var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);

            return cityToReturn == null ? NotFound() : Ok(cityToReturn);

        }

        /*** This is how you would manually determin what is going to be sent back
        *** Without a controller/model/DTO
        ***/

        // [HttpGet]
        // public JsonResult GetCities()
        // {
        //     return new JsonResult(
        //         new List<object> {
        //             new { id = 1, Name = "Salt Lake City" },
        //             new { id = 2, Name = "Portland"}
        //         }
        //     );
        // }
    }
}