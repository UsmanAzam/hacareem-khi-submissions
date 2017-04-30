namespace CareemDataFeeder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ride
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string userId { get; set; }

        [Required]
        [StringLength(50)]
        public string rideId { get; set; }

        public long pickupTime { get; set; }

        [Required]
        [StringLength(250)]
        public string pickupDisplay { get; set; }

        public double pickupLat { get; set; }

        public double pickupLong { get; set; }

        [Required]
        [StringLength(50)]
        public string pickupGeohash { get; set; }

        [Required]
        [StringLength(250)]
        public string dropoffDisplay { get; set; }

        public double dropoffLat { get; set; }

        public double dropoffLong { get; set; }

        [StringLength(50)]
        public string dropoffGeohash { get; set; }
    }
}
