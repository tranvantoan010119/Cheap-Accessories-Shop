using Dapper;
using Services.Models;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public interface IEmployeeRepository
    {
        Employee Login(LoginViewModel obj);
    }

    public class EmployeeRepository : RepositoryBase, IEmployeeRepository
    {
        public EmployeeRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public Employee Login(LoginViewModel obj)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Email", obj.Email);
                p.Add("@Password", obj.Password);
                var res = DbConnect.Query<Employee>("Employee_Login", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
