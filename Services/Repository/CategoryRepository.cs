using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.Entity;
using Services.ViewModels;

namespace Services.Repository
{
    public interface ICategoryRepository
    {
        int Create(Category obj);
        IEnumerable<Category> ListAll(int exclude = 0);
        IEnumerable<CategoryViewModel> ListAllWithCount(int exclude = 0);
        IEnumerable<Product> ListProduct(int id, int pageIndex, int pageSize, ref int totalRow);
        IEnumerable<CategoryViewModel> ListAllPaging(int pageIndex,int pageSize, ref int totalRow);
        Category ViewDetail(int id);
        int Update(Category obj);
        int Delete(int id);
        bool ChangeStatus(int id);
    }

    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public CategoryRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                var res = DbConnect.Execute("Category_ChangeStatus", p, commandType: CommandType.StoredProcedure, transaction: Transaction);
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Create(Category obj)
        {
            try
            {
                var p = param(obj);
                DbConnect.Execute("Category_Create", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                var res = p.Get<int>("@Id");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Delete(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                return DbConnect.Execute("Category_Delete", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Category> ListAll(int exclude = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@exclude", exclude);
                var res = DbConnect.Query<Category>("Category_ListAll",p,transaction: Transaction,commandType: CommandType.StoredProcedure);
                return res.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CategoryViewModel> ListAllPaging(int pageIndex, int pageSize, ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@pageIndex", pageIndex);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var res = DbConnect.Query<CategoryViewModel>("Category_ListAllPaging", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CategoryViewModel> ListAllWithCount(int exclude = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@exclude", exclude);
                var res = DbConnect.Query<CategoryViewModel>("Category_ListAllWithCountProduct", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> ListProduct(int id, int pageIndex, int pageSize, ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                p.Add("@pageIndex", pageIndex);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var res = DbConnect.Query<Product>("Product_ListByCate", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Category obj)
        {
            try
            {
                var p = param(obj, "edit");
                DbConnect.Execute("Category_Update", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@Id");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Category ViewDetail(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                var res = DbConnect.Query<Category>("Category_ViewDetail",p,transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DynamicParameters param(Category obj, string action = "create")
        {
            var p = new DynamicParameters();
            if(action == "edit")
            {
                p.Add("@Id", obj.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            }
            else
            {
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@CreatedDate", obj.CreatedDate);
            }

            p.Add("@Name", obj.Name);
            p.Add("@ParentId", obj.ParentId);
            p.Add("@Sort", obj.Sort);
            p.Add("@Published", obj.Published);

            return p;
        }
    }
}
