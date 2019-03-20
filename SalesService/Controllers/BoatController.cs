using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using SalesService.Entities;
using SalesService.Model;
using SalesService.Model.DomainModel;


namespace SalesService.Controllers
{
    [RoutePrefix("api/Boat")]
    public class BoatController : ApiController
    {
        /// <summary>
        /// Get all the boats
        /// </summary>
        [Route("")]
        public async Task<IHttpActionResult> GetBoat()
        {
            var result = await Task<Boats>.Run(() => BoatData.GetBoats());
            if(result!=null)
            return Ok(result);
            else
            return Content(HttpStatusCode.NotFound, $"No Boats available");
        }

        /// <summary>
        /// Get a boat based on Id 
        /// </summary>
        /// <param name="boatId">The ID of the boat</param>
        [Route("{boatId:int}")]
        public async Task<IHttpActionResult> GetBoat(int boatId)
        {
            var result = await Task<Boat>.Run(() => BoatData.GetBoatbyId(boatId));
            if (result != null)
                return Ok(result);
            else
                return Content(HttpStatusCode.NotFound, $"No boat is available for the boatId provided");
        }


        /// <summary>
        /// Create a boat
        /// </summary>
        /// <param name="boat">The boat object to be added</param>
        [Route("")]
        public async Task<IHttpActionResult> PostBoat(Boat boat)
        {
            if(boat.Identifier!=0)
                return Content(HttpStatusCode.BadRequest, $"Identifier should be null");

            var result = await Task<Boat>.Run(() => BoatData.CreateBoat(boat));
            if (result != null)
                return Ok(result);
            else          
                return Content(HttpStatusCode.BadRequest, $"Not able to add the boat");
         
          
        }

        /// <summary>
        /// Update a boat 
        /// </summary>
        /// <param name="boat">The boat object to be updated</param>
        [Route("")]
        public async Task<IHttpActionResult> PutBoat(Boat boat)
        {
            if (boat.Identifier == 0)
                return Content(HttpStatusCode.BadRequest, $"Identifier should be provided");
            var result = await Task<Boat>.Run(() => BoatData.UpdateBoat(boat));
            if (result != null)
                return Ok(result);
            else
                return Content(HttpStatusCode.NotFound, $"No boat is available for the Identifier provided");
        }
 

        /// <summary>
        /// Delete a boat based on Id 
        /// </summary>
        /// <param name="boatId">The ID of the boat</param>
        [Route("{boatId:int}")]
        public async Task<IHttpActionResult> DeleteBoat(int boatId)
        {
            var result = await Task<Boat>.Run(() => BoatData.DeleteBoatbyId(boatId));
            if (result ==true)
                return Ok();
            else
                return Content(HttpStatusCode.NotFound, $"No boat is available for the boatId provided");
        }

    }
}
