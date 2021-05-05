using Common;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customers.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult ListByCate()
        {
            return View();
        }

        public ActionResult Detail(long id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.ProductRepository.ViewDetail(id);
                return View(res);
            }
        }

        public ActionResult Search(decimal minPrice = 0, decimal maxPrice = 0 , string keyword = "",  int page = 1)
        {
            int pageSize = 12;
            int totalRow = 0;
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.ProductRepository.ListProductPaging(keyword,"",page, pageSize,ref totalRow, minPrice,maxPrice);
                ViewBag.totalRow = totalRow;
                ViewBag.keyword = keyword;
                ViewBag.minPrice = minPrice;
                ViewBag.maxPrice = maxPrice;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.totalPage = Math.Ceiling((decimal)totalRow/pageSize);
                return View(res);
            }

        }

        [ChildActionOnly]
        public PartialViewResult BestDeals()
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.ProductRepository.BestDeals(4);
                return PartialView("/Views/Product/_BestDeals.cshtml", res);
            }
        }

        [ChildActionOnly]
        public PartialViewResult RelatedProducts(int cateId)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.ProductRepository.RelatedProducts(top: 10,cateId: cateId);
                return PartialView("/Views/Product/_RelatedProducts.cshtml", res);
            }
        }

        public ActionResult ListProduct_BestDeal(int page = 1)
        {
            int pageSize = 12;
            int totalRow = 0;
            using (var uow = new UnitOfWork(Shared.connString))
            {
                ViewBag.currentLink = "/Product/ListProduct_BestDeal";
                ViewBag.currentName = "Giảm giá sốc nhất";
                var res = uow.ProductRepository.ListBestDeal_Paging(page,pageSize,ref totalRow);
                ViewBag.totalRow = totalRow;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.totalPage = Math.Ceiling((decimal)totalRow / pageSize);
                ViewBag.url = "/Product/ListProduct_BestDeal?page=";
                return View("/Views/Category/Index.cshtml", res);
            }
        }

        public ActionResult ListProduct_BestSeller(int page = 1)
        {
            int pageSize = 12;
            int totalRow = 0;
            using (var uow = new UnitOfWork(Shared.connString))
            {
                ViewBag.currentLink = "/Product/ListProduct_BestDeal";
                ViewBag.currentName = "Bán chạy nhất";
                var res = uow.ProductRepository.ListBestSeller_Paging(page, pageSize, ref totalRow);
                ViewBag.totalRow = totalRow;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.totalPage = Math.Ceiling((decimal)totalRow / pageSize);
                ViewBag.url = "/Product/ListProduct_BestSeller?page=";
                return View("/Views/Category/Index.cshtml", res);
            }
        }

        public ActionResult ListProduct_BestViewer(int page = 1)
        {

            int pageSize = 12;
            int totalRow = 0;
            using (var uow = new UnitOfWork(Shared.connString))
            {
                ViewBag.currentLink = "/Product/ListProduct_BestDeal";
                ViewBag.currentName = "Xem nhiều nhất";
                var res = uow.ProductRepository.ListProductPaging("","",page, pageSize, ref totalRow);
                ViewBag.totalRow = totalRow;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.totalPage = Math.Ceiling((decimal)totalRow / pageSize);
                ViewBag.url = "/Product/ListProduct_BestViewer?page=";
                return View("/Views/Category/Index.cshtml", res);
            }
        }

        public JsonResult Get_Price_Range()
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                decimal minValue = 0;
                decimal maxValue = 0;
                var res = uow.ProductRepository.Get_Price_Range(ref minValue, ref maxValue);
                return Json(new
                {
                    min = minValue,
                    max = maxValue
                }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}