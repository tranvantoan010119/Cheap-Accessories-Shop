using Admin.Helper;
using Common;
using Services.Models;
using Services.Repository;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class ProductController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get_ProductListAll(int pageIndex, int pageSize, string keyword = "", string code = "")
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                int totalRow = 0;
                var res = uow.ProductRepository.ListProductPaging(keyword, code, pageIndex, pageSize, ref totalRow);
                var html = RenderHelper.PartialView(this, "_productList", res);
                return Json(new
                {
                    status = true,
                    totalRow = totalRow,
                    data = html
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(ProductViewModel obj)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    status = false,
                    msg = ModelState.Values.SelectMany(v => v.Errors)
                });
            using (var uow = new UnitOfWork(Shared.connString))
            {
                obj.View = 0;
                if (obj.SaleOff == null)
                    obj.SaleOff = 0;
                var res = uow.ProductRepository.Create(obj);
                // product create failed -> return false and not commit
                if (res <= 0)
                    return Json(new
                    {
                        status = false
                    }, JsonRequestBehavior.AllowGet);
                // product create success 
                if (!String.IsNullOrEmpty(obj.Categories))   // Categories has value
                {
                    var cateList = obj.Categories.Substring(0, obj.Categories.LastIndexOf(";")).Split(';');
                    foreach (var item in cateList)
                    {
                        var productCate = new ProductCate();
                        productCate.ProductId = res;
                        productCate.CateId = Int32.Parse(item);
                        var subres = uow.ProductRepository.ProductCate_Create(productCate);
                        if (subres <= 0)  // productCate create failed -> return false and not commit
                            return Json(new
                            {
                                status = false
                            }, JsonRequestBehavior.AllowGet);
                    }
                }

                // all success -> commit and return
                uow.Commit();
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ViewDetail(int id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.ProductRepository.ViewDetail(id);
                return Json(new
                {
                    data = res
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult Edit(ProductViewModel obj)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    status = false,
                    msg = ModelState.Values.SelectMany(v => v.Errors)
                });
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var oldObj = uow.ProductRepository.ViewDetail(obj.Id);
                var res = uow.ProductRepository.Update(obj);
                // product update failed -> return false and not commit
                if (res <= 0)
                    return Json(new
                    {
                        status = false
                    }, JsonRequestBehavior.AllowGet);
                // product update success
                var objCategories = obj.Categories != null ? obj.Categories.Substring(0, obj.Categories.LastIndexOf(";")) : null; // remove ; at last
                if (objCategories != oldObj.Categories) //  if Categories is changed
                {
                    if (!String.IsNullOrEmpty(oldObj.Categories)) // if old categories has value
                    {
                        uow.ProductRepository.ProductCate_DeleteByProductId(obj.Id);
                    }

                    if (!String.IsNullOrEmpty(obj.Categories))   // Categories has value
                    {
                        var cateList = obj.Categories.Substring(0, obj.Categories.LastIndexOf(";")).Split(';');
                        foreach (var item in cateList)
                        {
                            var productCate = new ProductCate();
                            productCate.ProductId = obj.Id;
                            productCate.CateId = Int32.Parse(item);
                            var subres = uow.ProductRepository.ProductCate_Create(productCate);
                            if (subres <= 0)  // productCate create failed -> return false and not commit
                                return Json(new
                                {
                                    status = false
                                }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                // all success -> commit and return
                uow.Commit();
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.ProductRepository.ChangeStatus(id);
                if (res)
                    uow.Commit();
                return Json(new { status = res });
            }
        }

        public ActionResult Statistic()
        {
            return View();
        }

        public JsonResult Get_Statistic_Product(int pageIndex, int pageSize, string type)
        {
            if (String.IsNullOrEmpty(type))
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);

            using (var uow = new UnitOfWork(Shared.connString))
            {
                int totalRow = 0;
                var res = uow.ProductRepository.Statistic_Product(type, pageIndex, pageSize, ref totalRow);
                var html = "";
                if (type == "inventory")
                    html = RenderHelper.PartialView(this, "_inventoryStatistic", res);
                else
                    html = RenderHelper.PartialView(this, "_bestSellerStatistic", res);
                return Json(new
                {
                    status = true,
                    totalRow = totalRow,
                    data = html
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChangeIsHot(int id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.ProductRepository.ChangeIsHot(id);
                if (res)
                    uow.Commit();
                return Json(new { status = res });
            }
        }
    }
}