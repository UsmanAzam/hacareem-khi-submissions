namespace CareemDataFeeder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserHour")]
    public partial class UserHour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserHour()
        {
            DestinationsByHours = new HashSet<DestinationsByHour>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string userId { get; set; }

        public int hour { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DestinationsByHour> DestinationsByHours { get; set; }
    }
}
