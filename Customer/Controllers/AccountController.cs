using Common;
using Services.Models;
using Services.Repository;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customers.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string url = "")
        {
            ViewBag.url = url;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel obj,string url = "")
        {
            ViewBag.url = url;
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            using (var uow = new UnitOfWork(Shared.connString))
            {
                obj.Password = MD5HASH.Encryptor.MD5ENCRYPTOR(obj.Password + Shared.MD5_KEY);
                var res = uow.CustomerRepository.Login(obj);
                if (res == 1)
                {
                    var customer = uow.CustomerRepository.ViewDetail(obj.Email);
                    Session.Add(Shared.Session_Customer, customer);
                    if (!String.IsNullOrEmpty(url))
                        return Redirect(url);
                    return Redirect("/");
                }
                else if (res == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                    return View(obj);
                }
                else if (res == -2)
                {
                    ModelState.AddModelError("", "Tài khoản chưa được kích hoạt.Vui lòng liên hệ với ban quản trị.");
                    return View(obj);
                }
                else
                {
                    return Redirect("/not-found");
                }
                
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(CustomerViewModel obj, string ConfirmPassword)
        {
            if (!ModelState.IsValid)
                return View(obj);
            if(String.IsNullOrEmpty(ConfirmPassword))
            {
                ModelState.AddModelError("", "Vui lòng nhập Xác nhận mật khẩu.");
                return View(obj);
            }
            if(ConfirmPassword != obj.Password)
            {
                ModelState.AddModelError("", "Mật khẩu và Xác nhận mật khẩu không giống nhau.");
                return View(obj);
            }

            using (var uow = new UnitOfWork(Shared.connString))
            {
                obj.GuidId = Guid.NewGuid();

                // binding to customer
                Customer customer = new Customer();
                customer.GuidId = obj.GuidId;
                customer.Email = obj.Email;
                customer.Password = MD5HASH.Encryptor.MD5ENCRYPTOR(obj.Password + Shared.MD5_KEY);
                customer.Status = true;
                customer.CreatedDate = DateTime.Now;
                var res1 = uow.CustomerRepository.Register(customer);
                if (res1 <= 0)
                    return Redirect("/not-found");
                // binding to customerInfo
                if(!String.IsNullOrEmpty(obj.Address) && !String.IsNullOrEmpty(obj.Address) && !String.IsNullOrEmpty(obj.Address))
                {
                    CustomerInfor customerInfo = new CustomerInfor();
                    customerInfo.Address = obj.Address;
                    customerInfo.FullName = obj.FullName;
                    customerInfo.PhoneNo = obj.PhoneNo;
                    customerInfo.GuidId = obj.GuidId;
                    var res2 = uow.CustomerInfoRepository.Create(customerInfo);
                    if(res2 <= 0)
                        return Redirect("/not-found");
                }

                uow.Commit();
                return RedirectToAction("Login");
            }
        }

        public ActionResult UpdateInfo(string url = "")
        {
            if (Session[Shared.Session_Customer] == null)
                return Redirect("/not-found");
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var customer = (CustomerViewModel)Session[Shared.Session_Customer];
                var res = uow.CustomerRepository.ViewDetail(customer.Email);
                return View(res);
            }
        }

        [HttpPost]
        public ActionResult UpdateInfo(CustomerViewModel obj, string url = "")
        {
            if (Session[Shared.Session_Customer] == null)
                return Redirect("/not-found");
            if (!ModelState.IsValid)
                return View(obj);
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var customerInfo = new CustomerInfor();
                customerInfo.GuidId = obj.GuidId;
                customerInfo.FullName = obj.FullName;
                customerInfo.PhoneNo = obj.PhoneNo;
                customerInfo.Address = obj.Address;

                var res = uow.CustomerInfoRepository.Create(customerInfo);
                if (res <= 0)
                    return Redirect("not-found");
                uow.Commit();
                return String.IsNullOrEmpty(url) ? Redirect("/Account/UpdateInfo") : Redirect(url);
            }
        }

        public ActionResult Logout()
        {
            if(Session[Shared.Session_Customer] != null)
            {
                Session.Abandon();
            }
            return Redirect("/");
        }

        public ActionResult TransactionHistory()
        {
            if (Session[Shared.Session_Customer] == null)
                return Redirect("/");

            var customer = (CustomerViewModel)Session[Shared.Session_Customer];
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.OrderRepository.ListByCustomer(customer.Id);
                return View(res);
            }
        }
    }
}
