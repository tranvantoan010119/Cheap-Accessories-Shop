namespace Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Danh mục cha.")]
        public int? ParentId { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Vui lòng nhập Tên danh mục.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Thứ tự.")]
        public int? Sort { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Xuất bản?.")]
        public bool Published { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
