using Admin.Helper;
using Common;
using Services.Repository;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class CustomerController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get_CustomerListAll(int pageIndex,int pageSize, string fullname = "", string email = "")
        {
            using (var uow =new UnitOfWork(Shared.connString))
            {
                int totalRow = 0;
                var res = uow.CustomerRepository.ListCustomerPaging(fullname, email, pageIndex, pageSize,ref totalRow);
                var html = RenderHelper.PartialView(this,"_customerList",res);
                return Json(new {
                    status = true,
                    totalRow = totalRow,
                    data = html
                },JsonRequestBehavior.AllowGet);   
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(int Id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.CustomerRepository.ChangeStatus(Id);
                uow.Commit();
                if (res <= 0)
                    return Json(new
                    {
                        status = false
                    },JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    status = true
                });
            }
        }

        public JsonResult ViewDetail(int id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.CustomerRepository.ViewDetail(id);
                return Json(new
                {
                    data = res
                },JsonRequestBehavior.AllowGet);
            }
        }

        public ViewResult Edit(int id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.CustomerRepository.ViewDetail(id);
                return View(res);
            }
        }

        
        [HttpPost]
        public ActionResult Edit(CustomerViewModel obj)
        {
            ModelState.Remove("Password");
            if(!ModelState.IsValid)
            {
                ViewBag.Result = "Có lỗi xảy ra, vui lòng thử lại";
                return View(obj);
            }

            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.CustomerRepository.Update(obj);
                if (res < 0)
                {
                    ViewBag.Result = "Có lỗi xảy ra, vui lòng thử lại";
                    return View(obj);
                }
                    
                uow.Commit();
                TempData["Result"] = "Cập nhật thành công.";
                return RedirectToAction("Index", "Category");
               
            }
        }
    }
}