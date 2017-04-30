using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Careem.Common.Models.Rides;
using Careem.DataService.RabbitMQ;
using System.Net;
using System.Diagnostics;

namespace Careem.DataService.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/rides")]
    
    public class RidesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromBody]Ride ride)
        {

                //Debug.WriteLine(await Request.Content.ReadAsStringAsync());
                RabbitMQClient2 client = new RabbitMQClient2();
                client.SaveRide(ride);
                client.Close();

                return Ok();
        }
    }
}
