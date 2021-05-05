namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Tiêu đề.")]
        [StringLength(250)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mô tả ngắn.")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Nội dung chi tiết.")]
        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Ảnh đại diện.")]
        [StringLength(500)]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Xuất bản?")]
        public bool? Published { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? Type { get; set; }
    }
}
