using Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class CustomerViewModel
    {
 
        public int Id { get; set; }

        public Guid GuidId { get; set; }

        [StringLength(350)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        public string Email { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [StringLength(20)]
        public string PhoneNo { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ & Tên")]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [StringLength(350)]
        public string Password { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
