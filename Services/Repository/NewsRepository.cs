using Dapper;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public interface INewsRepository
    {
        bool Create(News obj);
        bool Edit(News obj);
        bool ChangePublished(int Id);
        IEnumerable<News> ListAll(int top = 0);
        IEnumerable<News> News_ListAllPaging(int page, int pageSize, ref int totalRow, string keyword = "", bool exclude = false);
        News ViewDetail(int id);
    }

    public class NewsRepository : RepositoryBase, INewsRepository
    {
        public NewsRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public bool ChangePublished(int Id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", Id);
                var res = DbConnect.Execute("News_ChangePublished", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Create(News obj)
        {
            try
            {
                var p = param(obj);
                var res = DbConnect.Execute("News_Create", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Edit(News obj)
        {
            try
            {
                var p = param(obj,action: "edit");
                var res = DbConnect.Execute("News_Edit", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<News> ListAll(int top)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Top", top);
                var res = DbConnect.Query<News>("News_ListAll",p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<News> News_ListAllPaging(int page, int pageSize, ref int totalRow, string keyword, bool exclude)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Exclude", exclude);
                p.Add("@Keyword", keyword);
                p.Add("@pageIndex", page);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var res = DbConnect.Query<News>("News_ListAll_Paging", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public News ViewDetail(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                var res = DbConnect.Query<News>("News_ViewDetail", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DynamicParameters param(News obj, string action = "add")
        {
            var p = new DynamicParameters();
            if (action != "add")
            {
                p.Add("@Id", obj.Id);
            }
            else
            {
                p.Add("@CreatedDate", obj.CreatedDate);
                p.Add("@Type", obj.Type);
            }
            p.Add("@Title", obj.Title);

            p.Add("@Detail", obj.Detail);
            p.Add("@Published", obj.Published);
            p.Add("@Avatar", obj.Avatar);
            p.Add("@Description", obj.Description);
            return p;
        }
    }
}
