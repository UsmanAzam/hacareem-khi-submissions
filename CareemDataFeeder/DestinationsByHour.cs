namespace CareemDataFeeder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DestinationsByHour")]
    public partial class DestinationsByHour
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string dropoffLocation { get; set; }

        public double dropoffLat { get; set; }

        public double? dropoffLong { get; set; }

        public int? count { get; set; }

        public int? userHourId { get; set; }

        public virtual UserHour UserHour { get; set; }
    }
}
