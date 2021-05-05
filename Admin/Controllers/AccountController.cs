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
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel obj)
        {
            if (!ModelState.IsValid)
                return View(obj);
            using (var uow = new UnitOfWork(Shared.connString))
            {
                obj.Password = MD5HASH.Encryptor.MD5ENCRYPTOR(obj.Password + Shared.MD5_KEY);
                var employee = uow.EmployeeRepository.Login(obj);
                if(employee != null)
                {
                    Session.Add(Shared.Session_Admin, employee);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Remove(Shared.Session_Admin);
            return RedirectToAction("Login", "Account");
        }
    }
}