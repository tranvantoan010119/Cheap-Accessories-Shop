using Dapper;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public interface IOrderRepository {
        int Insert(Order obj);
        Order ViewOrder(int id);
        int CheckOrder(int orderId, string email);
        bool ChangeStatus(int orderid, int statusId);
        bool ChangePayment(int orderid, int payment);
        IEnumerable<ChartsDataViewModel> statisticsByDay(DateTime fromDate, DateTime toDate);

        IEnumerable<Order> ListByCustomer(int customerId);
        IEnumerable<Order> ListAll_Paging(int pageIndex, int pageSize, ref int totalRow,int orderId = 0, string email = "");

        IEnumerable<OrderItem> ViewListOrderItem(int orderId);

        int InsertItem(OrderItem obj);
        int InsertAddressDelivery(AddressDelivery obj);
        AddressDelivery ViewAddressDelivery(int orderId);
    }

    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(IDbTransaction transaction) : base(transaction)
        {

        }

        public bool ChangeStatus(int orderid, int statusId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@orderId", orderid);
                p.Add("@statusId", statusId);
                var res = DbConnect.Execute("Order_ChangeStatus", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangePayment(int orderid, int payment)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@orderId", orderid);
                p.Add("@payment", payment);
                var res = DbConnect.Execute("Order_ChangePayment", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int CheckOrder(int orderId, string email)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@OrderId",orderId);
                p.Add("@Email", email);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                DbConnect.Execute("Order_CheckOrder", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Insert(Order obj)
        {
            try
            {
                var p = param(obj);
                DbConnect.Execute("Order_Insert",p,transaction:Transaction,commandType:CommandType.StoredProcedure);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertAddressDelivery(AddressDelivery obj)
        {
            var p = new DynamicParameters();
            p.Add("@OrderId", obj.OrderId);
            p.Add("@FullName", obj.FullName);
            p.Add("@Email", obj.Email);
            p.Add("@PhoneNo", obj.PhoneNo);
            p.Add("@Address", obj.Address);
            p.Add("@Note", obj.Note);

            var res = DbConnect.Execute("AddressDelivery_Insert", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
            return res;
        }

        public int InsertItem(OrderItem obj)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@OrderId", obj.OrderId);
                p.Add("@ProductId", obj.ProductId);
                p.Add("@Price", obj.Price);
                p.Add("@Quantity", obj.Quantity);
                p.Add("@ProductName", obj.ProductName);
                p.Add("@LastPrice", obj.LastPrice);

                var res = DbConnect.Execute("OrderItem_Insert", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Order> ListAll_Paging(int pageIndex, int pageSize, ref int totalRow, int orderId = 0, string email = "")
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@pageIndex", pageIndex);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@orderId", orderId);
                p.Add("@email", email);
                var res = DbConnect.Query<Order>("Order_ListAllPaging", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Order> ListByCustomer(int customerId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@CustomerId", customerId);
                var res = DbConnect.Query<Order>("Order_ListByCustomer", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ChartsDataViewModel> statisticsByDay(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@fromDate", fromDate);
                p.Add("@toDate", toDate);
                var res = DbConnect.Query<ChartsDataViewModel>("Order_StatisticsByDay", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddressDelivery ViewAddressDelivery(int orderId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@OrderId", orderId);
                var res = DbConnect.Query<AddressDelivery>("AddressDelivery_ViewDetail", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<OrderItem> ViewListOrderItem(int orderId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@OrderId", orderId);
                var res = DbConnect.Query<OrderItem>("OrderItem_ListAll", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Order ViewOrder(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                var res = DbConnect.Query<Order>("Order_ViewDetail",p,transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DynamicParameters param(Order obj)
        {
            var p = new DynamicParameters();
            p.Add("@CustomerId", obj.CustomerId);
            p.Add("@Email", obj.Email);
            p.Add("@Date", obj.Date);
            p.Add("@TotalAmount", obj.TotalAmount);
            p.Add("@Status", obj.Status);
            p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);

            return p;
        }
    }
}
