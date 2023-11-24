using System.Diagnostics;
using CityInfo.API.Models;
using CityInfo.API.Service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {

        /*** This is constructor injection ***/
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // This can be redundent depending on the container type you are using
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService)); 
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointsOfInterestController>> GetPointsOfIntrest(int cityId)
        {
            try
            {
                var city = GetCityFromDataStore(cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with the id {cityId} wasn\'t found when accessing points of interest");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting points of interest for city with id {cityId}",
                    ex
                );

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")] // The Name allows us to reference it within a CreatedAtRoute() status return
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = GetCityFromDataStore(cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = GetPointOfInterestFromDataStore(city, pointOfInterestId);

            return pointOfInterest == null ? NotFound() : Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest) // You don't have to include [FromBody] here, it will infer it because it's a complex datapoint
        {

            /*** This is not needed because the [ApiController] takes care of it ***/
            /*** When you put in annotations on the Model it will automatically send the BadRequest error with the annotation exception ***/
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest();
            // }

            var city = GetCityFromDataStore(cityId);

            if (city == null)
            {
                return NotFound();
            }

            // this is ugly, will be refactored; Finds the highest index of the Points of Interest
            var newId = _citiesDataStore.Cities.SelectMany(city => city.PointsOfInterest).Max(p => p.Id);

            // Convert PointOfInterestForCreationDto to PointOfInterestDto
            var newPOI = new PointOfInterestDto()
            {
                Id = ++newId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(newPOI);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId,
                    pointOfInterestId = newPOI.Id
                },
                newPOI
            ); // Returns 201 - Created, the location it was created, and what was created
        }

        [HttpPut("{pointOfInterestId}")] // PUT is _ALL_ fields must be updated
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            var city = GetCityFromDataStore(cityId);

            if (city == null)
            {
                return BadRequest();
            }

            var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (poi == null)
            {
                return BadRequest();
            }

            /*** In a PUT _ALL_ properties are updated ***/
            poi.Name = pointOfInterest.Name;
            poi.Description = pointOfInterest.Description;

            return NoContent(); // Returns 204 - NoContent since nothing needs to be returned
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = GetCityFromDataStore(cityId);

            if (city == null)
            {
                return NotFound();
            }

            var poi = GetPointOfInterestFromDataStore(city, pointOfInterestId);

            if (poi == null)
            {
                return NotFound();
            }

            var poiToPatch = new PointOfInterestForUpdateDto()
            {
                Name = poi.Name,
                Description = poi.Description
            };

            /*** Patches the patchDocument data to the variable to patch ***/
            patchDocument.ApplyTo(poiToPatch, ModelState); // pass in ModelState so that it validates any issues with the patching and returns !IsValid if there are errors

            // This must be done manually, it tests to see if there are any validation errors when patching
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // This checks to see if there are any validation errors after the patch has been completed
            if (!TryValidateModel(poiToPatch))
            {
                return BadRequest(ModelState);
            }

            poi.Name = poiToPatch.Name;
            poi.Description = poiToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = GetCityFromDataStore(cityId);

            if (city == null)
            {
                return NotFound();
            }

            var poi = GetPointOfInterestFromDataStore(city, pointOfInterestId);

            if (poi == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(poi);
            _mailService.Send("Point of interest deleted.", $"Point of interest {poi.Name} with id {poi.Id} was deleted");

            return NoContent();
        }

        private CityDto? GetCityFromDataStore(int cityId)
        {
            return _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
        }

        private PointOfInterestDto? GetPointOfInterestFromDataStore(CityDto city, int pointOfInterestId)
        {
            return city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
        }

    }
}