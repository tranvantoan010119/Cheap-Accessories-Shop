using Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ExtendMethod
    {
       
        public static string getOrderStatusName(this int num)
        {
            var list = get_OrderStatus();
            return list.Where(x => x.Id == num).SingleOrDefault().Name;
        }

        public static string getOrderPaymentName(this int num)
        {
            var list = get_OrderPayment();
            return list.Where(x => x.Id == num).SingleOrDefault().Name;
        }

        public static string convertDateToString(this DateTime date)
        {
            return date.Day + "/" + (date.Month + 1) + "/" + date.Year;
        }

        public static List<OrderStatus> get_OrderStatus()
        {
            List<OrderStatus> listStatus = new List<OrderStatus>();
            listStatus.Add(new OrderStatus { Id = 1, Name = "Chờ xử lý" });
            listStatus.Add(new OrderStatus { Id = 2, Name = "Đang xử lý" });
            listStatus.Add(new OrderStatus { Id = 3, Name = "Đang giao hàng" });
            listStatus.Add(new OrderStatus { Id = 4, Name = "Hoàn thành" });
            listStatus.Add(new OrderStatus { Id = 5, Name = "Hủy" });

            return listStatus;
        }

        public static List<OrderStatus> get_OrderPayment()
        {
            List<OrderStatus> listStatus = new List<OrderStatus>();
            listStatus.Add(new OrderStatus { Id = 0, Name = "Chưa thanh toán" });
            listStatus.Add(new OrderStatus { Id = 1, Name = "Đã thanh toán" });

            return listStatus;
        }

        public static DateTime convertStringtoDateTime(this string date)
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static string labelFormatCurrency(this decimal num)
        {
            return String.Format("{0:0,0} ₫", num);
        }
    }

    public enum OrderStatusEnum
    {
        Pending = 1,
        Process = 2,
        Delivery = 3,
        Done = 4,
        Cancel = 5
    }
}
