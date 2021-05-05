namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wishlist")]
    public partial class Wishlist
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public long? ProductId { get; set; }

        public int? Type { get; set; }
    }
}
