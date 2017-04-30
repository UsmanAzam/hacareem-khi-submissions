namespace Careem.DataService
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RideModel : DbContext
    {
        public RideModel()
            : base("name=RideModel")
        {
        }

        public virtual DbSet<DestinationsByHour> DestinationsByHours { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
        public virtual DbSet<UserHour> UserHours { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
