using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
using AutoMapper;
using TheWorld.ViewModels;
using TheWorld.Services;

namespace TheWorld.Controllers.Api
{
   [Route("api/trips/{tripName}/stops")]
    public class StopController:Controller
    {
        private CoordService _coordService;
        private ILogger<StopController> _logger;
        private IWorldRepository _repository;

        public StopController(IWorldRepository repository, ILogger<StopController> logger, CoordService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = _repository.GetTripByName(tripName);

                if (results == null)
                {
                    return Json(null);
                }

                return Json(Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(t=>t.Id)) );
                               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops for trip {tripName}", ex );
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Error occurred fiding {tripName}");
            }
        }

        [HttpPost("")]
        public async Task<JsonResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Map the entity

                    var newStop = Mapper.Map<Stop>(vm);

                    //Looking up Geocoordinates

                    var coordResult = await  _coordService.Lookup(newStop.Name );

                    if (!coordResult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(coordResult.Message );
                    }

                    newStop.Latitude = coordResult.Latitude;
                    newStop.Longitude = coordResult.Longitude;

                    //Save to the Databse
                    _repository.AddStop(tripName, newStop);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;

                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }
                }

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed: Invalid Data", ModelState = ModelState });

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                _logger.LogError("failed to add new stop", ex);

                return Json("failed to add new stop");
            }
        }
    }
}
