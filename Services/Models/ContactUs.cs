namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContactUs
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Vui lòng nhập Tiêu đề.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Nội dung.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ & Tên.")]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [StringLength(255)]
        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Status { get; set; }
    }
}
