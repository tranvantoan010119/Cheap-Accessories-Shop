namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerInfor")]
    public partial class CustomerInfor
    {
        [Key]
        public Guid GuidId { get; set; }

        [StringLength(20)]
        public string PhoneNo { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string FullName { get; set; }
    }
}
