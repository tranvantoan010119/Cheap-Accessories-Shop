namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        public int Id { get; set; }

        public Guid GuidId { get; set; }

        [StringLength(350)]
        public string Email { get; set; }

        [StringLength(350)]
        public string Password { get; set; }

        public bool? Status { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
