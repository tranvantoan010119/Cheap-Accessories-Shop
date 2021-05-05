using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Services.Models;
using Dapper;

namespace Services.Repository
{
    public interface ICustomerInfoRepository
    {
        int Create(CustomerInfor obj);
    }

    public class CustomerInfoRepository : RepositoryBase,ICustomerInfoRepository
    {
        public CustomerInfoRepository(IDbTransaction transaction) : base(transaction) { }

        public int Create(CustomerInfor obj)
        {
            try
            {
                var p = param(obj);
                var res = DbConnect.Execute("CustomerInfor_Create", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DynamicParameters param(CustomerInfor obj)
        {
            var p = new DynamicParameters();
            p.Add("@GuidId", obj.GuidId);
            p.Add("@PhoneNo", obj.PhoneNo);
            p.Add("@Address", obj.Address);
            p.Add("@FullName", obj.FullName);

            return p;
        }
    }
}
