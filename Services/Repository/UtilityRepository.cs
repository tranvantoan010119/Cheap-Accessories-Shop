using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public interface IUtilityRepository
    {
        int CountProduct();
        int CountCustomer();
        int CountOrder();
        decimal CountRevenue();
    }

    public class UtilityRepository : RepositoryBase, IUtilityRepository
    {
        public UtilityRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public int CountCustomer()
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                DbConnect.Execute("Utility_CountCustomer",p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CountOrder()
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                DbConnect.Execute("Utility_CountOrder", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CountProduct()
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                DbConnect.Execute("Utility_CountProduct", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal CountRevenue()
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Output", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                DbConnect.Execute("Utility_CountRevenue", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<decimal>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
