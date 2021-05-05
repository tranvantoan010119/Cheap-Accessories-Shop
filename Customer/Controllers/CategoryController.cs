using Common;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customers.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(int id, int page = 1)
        {
            int pageSize = 12;
            int totalRow = 0;
            using (var uow = new UnitOfWork(Shared.connString))
            {
                ViewBag.currentLink = "/Category/Index?id="+ id +"";
                ViewBag.currentName = uow.CategoryRepository.ViewDetail(id).Name;
                var res = uow.CategoryRepository.ListProduct(id,page,pageSize,ref totalRow);
                ViewBag.totalRow = totalRow;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.totalPage = Math.Ceiling((decimal)totalRow / pageSize);
                ViewBag.url = "/Category/Index?id=" + id + "&page=";
                return View(res);
            }
        }

        public ActionResult ListByCate()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult LeftSide()
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.CategoryRepository.ListAllWithCount(1);
                return PartialView("/Views/Category/_LeftSide.cshtml",res);
            }
        }
    }
}