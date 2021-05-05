using Dapper;
using Services.Models;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Services.Repository
{
    public interface ICustomerRepository
    {
        int Register(Customer obj);
        int Login(LoginViewModel obj);
        IEnumerable<CustomerViewModel> ListCustomerPaging(string fullname, string email,int page, int pageSize, ref int totalRow);
        int ChangeStatus(int Id);
        CustomerViewModel ViewDetail(int id);
        CustomerViewModel ViewDetail(string email);
        int Update(CustomerViewModel obj);

        bool ContactUs_Insert(ContactUs obj);
    }

    public class CustomerRepository : RepositoryBase, ICustomerRepository
    {
        public CustomerRepository(IDbTransaction transaction) : base(transaction) { }

        public IEnumerable<CustomerViewModel> ListCustomerPaging(string fullname, string email,int page,int pageSize,ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@FullName", fullname);
                p.Add("@Email", email);
                p.Add("@pageIndex", page);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction:ParameterDirection.Output);
                var res = DbConnect.Query<CustomerViewModel>("CustomerFullInfo_ListAll", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Register(Customer obj)
        {
            try
            {
                var p = param(obj);
                DbConnect.Execute("Customer_Create",p,commandType: CommandType.StoredProcedure,transaction: Transaction );
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Login(LoginViewModel obj)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Email", obj.Email);
                p.Add("@Password", obj.Password);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                DbConnect.Execute("Customer_Login", p, commandType: CommandType.StoredProcedure, transaction: Transaction);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ChangeStatus(int Id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", Id);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                DbConnect.Execute("Customer_ChangeStatus", p, commandType: CommandType.StoredProcedure,transaction: Transaction);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DynamicParameters param(Customer obj,string action = "Register")
        {
            var p = new DynamicParameters();
            if(action == "Register")
            {
                p.Add("@Output", dbType: DbType.Int32,direction: ParameterDirection.Output);
                p.Add("@GuidId", obj.GuidId);
                p.Add("@Status", obj.Status);
                p.Add("@CreatedDate", obj.CreatedDate);
            }
            p.Add("@Email", obj.Email);
            p.Add("@Password", obj.Password);

            return p;
        }

        public CustomerViewModel ViewDetail(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                var res = DbConnect.Query<CustomerViewModel>("CustomerFullInfo_ViewDetail", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(CustomerViewModel obj)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", obj.Id);
                p.Add("@Email", obj.Email);
                p.Add("@GuidId", obj.GuidId);
                p.Add("@PhoneNo", obj.PhoneNo);
                p.Add("@FullName", obj.FullName);
                p.Add("@Address", obj. Address);
                p.Add("@Status", obj. Status);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                DbConnect.Execute("CustomerFullInfo_Update", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public CustomerViewModel ViewDetail(string email)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Email", email);
                var res = DbConnect.Query<CustomerViewModel>("CustomerFullInfo_ViewDetail_ByEmail", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.FirstOrDefault(); ;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ContactUs_Insert(ContactUs obj)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Title", obj.Title);
                p.Add("@Content", obj.Content);
                p.Add("@FullName", obj.FullName);
                p.Add("@Email", obj.Email);
                p.Add("@CreatedDate", obj.CreatedDate);
                p.Add("@Status", obj.Status);
                var res = DbConnect.Execute("ContactUs_Insert", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res <= 0 ? false : true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
