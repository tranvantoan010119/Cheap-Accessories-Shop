namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CartItem")]
    public partial class CartItem
    {
        public long Id { get; set; }

        public long? ProductId { get; set; }

        public int? CustomerId { get; set; }
    }
}
