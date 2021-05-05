using Common;
using Services.Models;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheBodyShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                ViewBag.HotProducts = uow.ProductRepository.HotProducts(6);
                ViewBag.BestDeal = uow.ProductRepository.ListBestDeal(8);
                ViewBag.BestSeller = uow.ProductRepository.ListBestSeller(8);
                ViewBag.BestView = uow.ProductRepository.ListAllProduct(8);

                return View();
            }
        }

        [ChildActionOnly]
        public PartialViewResult _Menu()
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.CategoryRepository.ListAll(1);
                return PartialView("/Views/Home/_Menu.cshtml",res);
            }
            
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUs obj)
        {
            if (!ModelState.IsValid)
                return View(obj);
            using (var uow = new UnitOfWork(Shared.connString))
            {
                obj.CreatedDate = DateTime.Now;
                obj.Status = 1;
                var res = uow.CustomerRepository.ContactUs_Insert(obj);
                if (!res)
                    return Redirect("/not-found");
                uow.Commit();
                ViewBag.msg = "Xin cảm ơn, vấn đề của bạn đã được tiếp nhận và chờ xử lý.";
                return View();
            }
        }

        public PartialViewResult HotBlogs()
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.NewsRepository.ListAll(3);
                return PartialView("/Views/Home/_Blogs.cshtml", res);
            }
        }
    }
}