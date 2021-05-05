using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class CartItemViewModel
    {
        public long productId { get; set; }
        public string productCode { get; set; }
        public short quantity { get; set; }
        public string productName { get; set; }
        public string avatar { get; set; }
        public decimal Amount { get; set; }
        public decimal LastAmount { get; set; }
    }
}
