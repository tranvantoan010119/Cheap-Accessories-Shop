using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class OrderViewModel
    {
        public AddressDelivery addressDelivery;
        public List<OrderItem> listOrderItem;
        public Order order;
    }
}
