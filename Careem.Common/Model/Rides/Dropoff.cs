using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Careem.Common.Models.Rides
{
    public class Dropoff
    {
        public string display { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string geohash { get; set; }
    }
}