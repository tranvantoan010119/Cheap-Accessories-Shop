namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AddressDelivery")]
    public partial class AddressDelivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ & Tên")]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [StringLength(350)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [StringLength(20)]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ")]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public virtual Order Order { get; set; }
    }
}
