namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductCate")]
    public partial class ProductCate
    {
        public int Id { get; set; }

        public long? ProductId { get; set; }

        public int? CateId { get; set; }
    }
}
