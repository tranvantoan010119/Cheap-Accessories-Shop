namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string FullName { get; set; }

        public short? RoleId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(255)]
        public string CreatedBy { get; set; }
    }
}
