using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Vui lòng nhập Tên sản phẩm.")]
        public string Name { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Vui lòng nhập Mã sản phẩm.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mô tả ngắn.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Chi tiêt.")]
        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        public string Images { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Ảnh đại diện")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Số lượng")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Giá nhập")]
        public decimal? UnitPrice { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Giá niêm yết")]
        public decimal Price { get; set; }

        public decimal? SaleOff { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? Published { get; set; }

        public bool? IsHot { get; set; }

        public int? View { get; set; }

        public string Categories { get; set; }

        public string CateName { get; set; }

        public int CateId { get; set; }

        public decimal SellPrice
        {
            set { SellPrice = value; }
            get
            {
                if (SaleOff != null && SaleOff.Value > 0 && DateTime.Now > StartDate && DateTime.Now < EndDate)
                {
                    return SaleOff.Value;
                }
                else
                {
                    return Price;
                }
            }
        }

        public int QuantitySold { get; set; }
    }
}
