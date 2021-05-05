using Admin.Helper;
using Common;
using Services.Models;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult Get_NewsListAll(int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                bool exclude = false;
                int totalRow = 0;
                var res = uow.NewsRepository.News_ListAllPaging(pageIndex, pageSize, ref totalRow,exclude: exclude);
                var html = RenderHelper.PartialView(this, "_newsList", res);
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
        public ActionResult Create(News obj)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    status = false,
                    msg = ModelState.Values.SelectMany(v => v.Errors)
                });
            using (var uow = new UnitOfWork(Shared.connString))
            {
                obj.CreatedDate = DateTime.Now;
                obj.Type = 1;
                var res = uow.NewsRepository.Create(obj);
                if (!res)
                    return Json(new { status = false });
                uow.Commit();
                return Json(new { status = true });
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        public JsonResult ViewDetail(int id)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.NewsRepository.ViewDetail(id);
                return Json(new
                {
                    data = res
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Edit(News obj)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    status = false,
                    msg = ModelState.Values.SelectMany(v => v.Errors)
                });
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.NewsRepository.Edit(obj);
                if (!res)
                    if (!ModelState.IsValid)
                        return Json(new
                        {
                            status = false
                        });
                uow.Commit();
                return Json(new
                {
                    status = true
                });
            }
        }

        [HttpPost]
        public JsonResult ChangePublished(int Id)
        {
            if (Id <= 0)
                return Json(new { status = false });
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.NewsRepository.ChangePublished(Id);
                if (!res)
                    return Json(new { status = false });
                uow.Commit();
                return Json(new { status = true });
            }
        }
    }
}