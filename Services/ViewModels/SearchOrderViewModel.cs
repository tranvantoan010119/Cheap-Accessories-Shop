using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class SearchOrderViewModel
    {
        [Required]
        public int orderId { get; set; }
        [Required]
        [StringLength(255)]
        public string email { get; set; }
    }
}
