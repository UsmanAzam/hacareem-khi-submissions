using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Careem.Common.Models.Rides
{
    public class Ride
    {
        public string userId { get; set; }
        public string rideId { get; set; }
        public int pickUpTime { get; set; }
        public Pickup pickup { get; set; }
        public Dropoff dropoff { get; set; }
    }
}