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
    public interface IProductRepository
    {
        IEnumerable<Product> ListAllProduct(int top = 0);
        IEnumerable<Product> ListProductPaging(string keyword,string code,int page, int pageSize, ref int totalRow, decimal MinPrice = 0, decimal MaxPrice = 0);
        IEnumerable<Product> HotProducts(int top = 0);
        IEnumerable<Product> ListBestDeal(int top = 0);
        IEnumerable<Product> ListBestSeller(int top = 0);
        IEnumerable<ProductViewModel> Statistic_Product(string type, int page, int pageSize, ref int totalRow);

        IEnumerable<Product> ListBestDeal_Paging(int page, int pageSize, ref int totalRow);
        IEnumerable<Product> ListBestSeller_Paging(int page, int pageSize, ref int totalRow);
        
        long Create(ProductViewModel obj);
        int ProductCate_Create(ProductCate obj);
        ProductViewModel ViewDetail(long Id);
        int CountByCate(int id);
        long Update(ProductViewModel obj);
        int ProductCate_DeleteByProductId(long Id);
        bool ChangeStatus(int id);
        bool ChangeIsHot(int id);
        IEnumerable<Product> BestDeals(int top = 0);
        IEnumerable<Product> RelatedProducts(int cateId,int top = 0);

        bool Get_Price_Range(ref decimal MinValue, ref decimal MaxValue);
        bool UpdateQuantity(long id, int Quantity);
    }

    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public ProductRepository(IDbTransaction transaction) : base(transaction) { }

        public IEnumerable<Product> ListProductPaging(string keyword,string code,int page, int pageSize, ref int totalRow, decimal MinPrice = 0, decimal MaxPrice = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@MinPrice", MinPrice);
                p.Add("@MaxPrice", MaxPrice);
                p.Add("@code", code);
                p.Add("@keyword", keyword);
                p.Add("@pageIndex", page);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var res = DbConnect.Query<Product>("Product_ListAllPaging", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long Create(ProductViewModel obj)
        {
            try
            {
                var p = param(obj);
                DbConnect.Execute("Product_Create", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                var res = p.Get<long>("@Id");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ProductCate_Create(ProductCate obj)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@ProductId", obj.ProductId);
                p.Add("@CateId", obj.CateId);
                p.Add("@Id", dbType: DbType.Int32,direction: ParameterDirection.Output);
                DbConnect.Execute("ProductCate_Create", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@Id");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductViewModel ViewDetail(long Id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", Id);
                var res = DbConnect.Query<ProductViewModel>("Product_ViewDetal", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DynamicParameters param(ProductViewModel obj, string action = "create")
        {
            var p = new DynamicParameters();
            if(action == "edit")
            {
                p.Add("@Id", obj.Id);
                p.Add("@Output", dbType: DbType.Int64, direction: ParameterDirection.Output);
            }
            else
            {
                p.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                p.Add("@View", obj.View);
            }
            
            p.Add("@Name", obj.Name);
            p.Add("@Code", obj.Code);
            p.Add("@Detail", obj.Detail);
            p.Add("@Description", obj.Description);
            p.Add("@Avatar", obj.Avatar);
            p.Add("@Images", obj.Images);
            p.Add("@Price", obj.Price);
            p.Add("@UnitPrice", obj.UnitPrice);
            p.Add("@SaleOff", obj.SaleOff);
            p.Add("@StartDate", obj.StartDate);
            p.Add("@EndDate", obj.EndDate);
            p.Add("@Published", obj.Published);
            p.Add("@IsHot", obj.IsHot);
            p.Add("@Quantity", obj.Quantity);

            return p;
        }

        public int CountByCate(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                DbConnect.Execute("Product_CountByCate", p,transaction: Transaction, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@Output");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long Update(ProductViewModel obj)
        {
            try
            {
                var p = param(obj, "edit");
                DbConnect.Execute("Product_Update", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                var res = p.Get<long>("@Output");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ProductCate_DeleteByProductId(long Id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@ProductId", Id);
                var res = DbConnect.Execute("ProductCate_DeleteByProductId", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> ListAllProduct(int top = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@top", top);
                var res = DbConnect.Query<Product>("Product_ListAll", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> BestDeals(int top = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@top", top);
                var res = DbConnect.Query<Product>("Product_BestDeals", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> RelatedProducts(int cateId,int top = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@top", top);
                p.Add("@CateId", cateId);
                var res = DbConnect.Query<Product>("Product_RelatedList", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> HotProducts(int top = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@top", top);
                var res = DbConnect.Query<Product>("Product_ListHot", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> ListBestDeal(int top = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@top", top);
                var res = DbConnect.Query<Product>("Product_ListDeal", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> ListBestSeller(int top = 0)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@top", top);
                var res = DbConnect.Query<Product>("Product_ListBestSeller", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> ListBestDeal_Paging(int page, int pageSize, ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@pageIndex", page);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var res = DbConnect.Query<Product>("Product_ListDeal_Paging", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> ListBestSeller_Paging(int page, int pageSize, ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@pageIndex", page);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var res = DbConnect.Query<Product>("Product_ListBestSeller_Paging", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                var res = DbConnect.Execute("Product_ChangeStatus", p, commandType: CommandType.StoredProcedure, transaction: Transaction);
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ProductViewModel> Statistic_Product(string type, int page, int pageSize, ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@type", type);
                p.Add("@pageIndex", page);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var res = DbConnect.Query<ProductViewModel>("Product_Statistic_Paging", p, transaction: Transaction, commandType: CommandType.StoredProcedure);
                totalRow = p.Get<int>("@totalRow");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangeIsHot(int id)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);
                var res = DbConnect.Execute("Product_ChangeIsHot", p, commandType: CommandType.StoredProcedure, transaction: Transaction);
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Get_Price_Range(ref decimal MinValue, ref decimal MaxValue)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@MinValue", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                p.Add("@MaxValue", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                var res = DbConnect.Execute("Price_Range", p, commandType: CommandType.StoredProcedure, transaction: Transaction);
                MinValue = p.Get<decimal>("@MinValue");
                MaxValue = p.Get<decimal>("@MaxValue");
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateQuantity(long id, int Quantity)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Id",id);
                p.Add("@Quantity", Quantity);
                var res = DbConnect.Execute("Product_UpdateQuantity", p, commandType: CommandType.StoredProcedure, transaction: Transaction);
                return res > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
