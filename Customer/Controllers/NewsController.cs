using Common;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customers.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index(string keyword = "",int page = 1)
        {
            ViewBag.keyword = keyword;
            int pageSize = 12;
            int totalRow = 0;
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.NewsRepository.News_ListAllPaging(page, pageSize, ref totalRow, keyword, exclude: true);

                ViewBag.totalRow = totalRow;
                ViewBag.page = page;
                ViewBag.pageSize = pageSize;
                ViewBag.totalPage = Math.Ceiling((decimal)totalRow / pageSize);
                return View(res);
            }
                
        }

        public ActionResult Detail(int id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.NewsRepository.ViewDetail(id);
                return View(res);
            }
                
        }
    }
}