using Common;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Customers.Ultilities
{
    public static class Constant
    {
        public static readonly string AdminAddress = ConfigurationManager.AppSettings["AdminAddress"];
        public static readonly string Cart_Session = "Cart_Session";
        public static readonly string AddressDelivery_Session = "AddressDelivery_Session";
    }
}