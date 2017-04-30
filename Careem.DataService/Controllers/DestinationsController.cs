using Careem.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;

namespace Careem.DataService.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/destinations")]
    public class DestinationsController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Get([FromBody]RideInfo ride)
        {
            RideModel model = new RideModel();

           var hours =  model.UserHours.Where(x => x.userId == ride.user_id && new int[] { ride.hour, ride.hour + 1, ride.hour - 1 }.Contains(x.hour));

           List<RideDest> dest = new List<RideDest>();

            foreach(var hour in hours)
            {
                dest.Add(new RideDest()
                {
                    dropOffLat = hour.DestinationsByHours.First().dropoffLat,
                    dropOffLong = hour.DestinationsByHours.First().dropoffLong.Value, 
                    dropOffLocation = hour.DestinationsByHours.First().dropoffLocation
                });
            }

            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
            //return response;

            return Request.CreateResponse(HttpStatusCode.OK, dest);
        }
    }
}
