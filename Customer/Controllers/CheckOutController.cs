using Common;
using Customers.Ultilities;
using Services.Models;
using Services.Repository;
using Services.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customers.Controllers
{
    public class CheckOutController : Controller
    {
        // GET: CheckOut
        public ActionResult Step1()
        {
            if (Session[Constant.Cart_Session] == null)
                return RedirectToAction("Index", "Cart");
            if (Session[Shared.Session_Customer] != null)
                return RedirectToAction("Step2");
            else
                return View();
        }

        public ActionResult Step2()
        {
            if (Session[Constant.Cart_Session] == null)
                return RedirectToAction("Index", "Cart");
            if(Session[Shared.Session_Customer] != null)
            {
                ViewBag.ExistsSession = true;
                var customer = (CustomerViewModel)Session[Shared.Session_Customer];
                using (var uow = new UnitOfWork(Shared.connString))
                {
                    var customerFullInfo = uow.CustomerRepository.ViewDetail(customer.Email);
                    if (customerFullInfo == null || String.IsNullOrEmpty(customerFullInfo.Email) || String.IsNullOrEmpty(customerFullInfo.PhoneNo) || String.IsNullOrEmpty(customerFullInfo.Address) || String.IsNullOrEmpty(customerFullInfo.FullName))
                        ViewBag.InfoInvalid = "invalid";
                    else
                        ViewBag.Info = customerFullInfo;
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Step2(AddressDelivery obj,string type = "",string note = "")
        {
            if(type == "existing")
            {
                if (Session[Shared.Session_Customer] == null)
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);

                var customer = (CustomerViewModel)Session[Shared.Session_Customer];
                using (var uow = new UnitOfWork(Shared.connString))
                {
                    var customerFullInfo = uow.CustomerRepository.ViewDetail(customer.Email);
                    if(customerFullInfo == null || String.IsNullOrEmpty(customerFullInfo.Email) || String.IsNullOrEmpty(customerFullInfo.PhoneNo) || String.IsNullOrEmpty(customerFullInfo.Address) || String.IsNullOrEmpty(customerFullInfo.FullName))
                        return Json(new { status = false, invalid = true }, JsonRequestBehavior.AllowGet);
                    var address_Delivery = new AddressDelivery();
                    address_Delivery.Email = customerFullInfo.Email;
                    address_Delivery.FullName = customerFullInfo.FullName;
                    address_Delivery.PhoneNo = customerFullInfo.PhoneNo;
                    address_Delivery.Address = customerFullInfo.Address;
                    address_Delivery.Note = note;
                    Session[Constant.AddressDelivery_Session] = address_Delivery;
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    if (Session[Constant.Cart_Session] == null)
                        return RedirectToAction("Index", "Cart");
                    if (Session[Shared.Session_Customer] != null)
                        ViewBag.ExistsSession = true;

                    var msg = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                    return Json(new
                    {
                        status = false,
                        data = msg
                    }, JsonRequestBehavior.AllowGet);
                }

                Session[Constant.AddressDelivery_Session] = obj;
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
                       
        }

        public ActionResult Step3()
        {
            if (Session[Constant.Cart_Session] == null)
                return RedirectToAction("Index", "Cart");
            List<CartItemViewModel> ssCart = (List<CartItemViewModel>)Session[Constant.Cart_Session];
            List<CartItemViewModel> ssCartVerify = new List<CartItemViewModel>();
            using (var uow = new UnitOfWork(Shared.connString))
            {
                foreach (var item in ssCart)
                {
                    var res = uow.ProductRepository.ViewDetail(item.productId);
                    var cartItem = new CartItemViewModel();
                    cartItem.productId = item.productId;
                    cartItem.quantity = item.quantity;
                    cartItem.productCode = res.Code;
                    cartItem.productName = res.Name;
                    cartItem.Amount = res.Price;
                    cartItem.LastAmount = res.SellPrice;
                    cartItem.avatar = res.Avatar;

                    ssCartVerify.Add(cartItem);
                }
                Session[Constant.Cart_Session] = ssCartVerify;
                TempData["Step3"] = "complete";
            }
            return View(ssCartVerify);
        }

        [HttpPost]
        public JsonResult CheckOut()
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                if (TempData["Step3"] != null && (string)TempData["Step3"] == "complete")
                {
                                         
                    if (Session[Constant.Cart_Session] == null && Session[Constant.AddressDelivery_Session] == null)
                        return Json(new { status = false });
                    List<CartItemViewModel> ssCart = (List<CartItemViewModel>)Session[Constant.Cart_Session];
                    AddressDelivery ssAddress = (AddressDelivery)Session[Constant.AddressDelivery_Session];

                    #region insert Order
                    var order = new Order();

                    if (Session[Shared.Session_Customer] != null)
                        order.CustomerId = ((CustomerViewModel)Session[Shared.Session_Customer]).Id;
                    else
                        order.CustomerId = 0;
                    order.Email = ssAddress.Email;
                    order.Date = DateTime.Now;
                    order.Status = 1;
                    decimal totalAmount = 0;
                    foreach (var item in ssCart)
                    {
                        totalAmount += (item.LastAmount * item.quantity);
                    }
                    order.TotalAmount = totalAmount;

                    var resOrder = uow.OrderRepository.Insert(order);
                    if (resOrder <= 0)
                        return Json(new { status = false });
                    #endregion

                    #region insert Address Delivery
                    ssAddress.OrderId = resOrder;
                    var resAddressDelivery = uow.OrderRepository.InsertAddressDelivery(ssAddress);
                    if (resAddressDelivery <= 0)
                        return Json(new { status = false });
                    #endregion

                    #region insert Order Item
                    foreach (var item in ssCart)
                    {
                        var orderItem = new OrderItem();
                        orderItem.OrderId = resOrder;
                        orderItem.ProductId = item.productId;
                        orderItem.Price = item.Amount;
                        orderItem.Quantity = item.quantity;
                        orderItem.ProductName = item.productName;
                        orderItem.LastPrice = item.LastAmount;

                        var resItem = uow.OrderRepository.InsertItem(orderItem);
                        if (resItem <= 0)
                            return Json(new { status = false });
                        // subtract Product Quantity
                        uow.ProductRepository.UpdateQuantity(orderItem.ProductId, -orderItem.Quantity);
                    }
                    #endregion

                    uow.Commit();
                    Session[Constant.AddressDelivery_Session] = null;
                    Session[Constant.Cart_Session] = null;
                    return Json(new { status = true, orderId = resOrder, email = ssAddress.Email, ssAddress = ssAddress });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
        }

        public ActionResult SearchOrder()
        {
            return View();
        }

        public ActionResult ViewOrder(SearchOrderViewModel obj)
        {
            if(!ModelState.IsValid)
            {
                return View("/Views/CheckOut/SearchOrder.cshtml");
            }

            using (var uow = new UnitOfWork(Shared.connString))
            {
                var res = uow.OrderRepository.CheckOrder(obj.orderId, obj.email);
                if(res <= 0)
                {
                    ViewBag.orderId = obj.orderId;
                    ViewBag.email = obj.email;
                    ViewBag.notfound = true;
                    return View("/Views/CheckOut/SearchOrder.cshtml");
                }

                var model = new OrderViewModel();
                model.addressDelivery = uow.OrderRepository.ViewAddressDelivery(obj.orderId);
                model.order = uow.OrderRepository.ViewOrder(obj.orderId);
                model.listOrderItem = uow.OrderRepository.ViewListOrderItem(obj.orderId).ToList();

                return View(model);
            }
        }

        public ActionResult PaymentSuccess(string transaction_info, decimal price, int payment_id, int payment_type, string error_text, string secure_code, string order_code)
        {
            var orderId = int.Parse(order_code);
            using (var uow = new UnitOfWork(Shared.connString))
            {
                uow.OrderRepository.ChangePayment(orderId, 1);
                uow.Commit();
            }

            return RedirectToAction("ViewOrder", new SearchOrderViewModel { orderId = orderId, email = transaction_info });
        }

        public ActionResult PaymentFailed(string id)
        {
            return View();
        }
    }
}