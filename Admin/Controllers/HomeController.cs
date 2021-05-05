using Common;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                ViewBag.CountCustomer = uow.UtilityRepository.CountCustomer();
                ViewBag.CountProduct = uow.UtilityRepository.CountProduct();
                ViewBag.CountOrder = uow.UtilityRepository.CountOrder();
                ViewBag.CountRevenue = uow.UtilityRepository.CountRevenue();

                return View();

            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}