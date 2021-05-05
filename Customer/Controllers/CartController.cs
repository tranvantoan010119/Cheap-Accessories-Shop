using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Customers.Ultilities;
using Services.Models;
using Services.ViewModels;
using Services.Repository;
using Common;
using System.Web.Script.Serialization;
using Customers.Helper;

namespace Customers.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            var model = (List<CartItemViewModel>)Session[Constant.Cart_Session];
            return View(model);
        }

        public ActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public JsonResult addToCart(long productId, Int16 quantity = 1)
        {
            using (var uow = new UnitOfWork(Shared.connString))
            {
                List<CartItemViewModel> ssCart = new List<CartItemViewModel>();
                var product = uow.ProductRepository.ViewDetail(productId);
                if (Session[Constant.Cart_Session] == null)
                {
                    var cartItem = new CartItemViewModel();
                    cartItem.productId = productId;
                    cartItem.quantity = quantity;
                    cartItem.productCode = product.Code;
                    cartItem.productName = product.Name;
                    cartItem.Amount = product.Price;
                    cartItem.LastAmount = product.SellPrice;
                    cartItem.avatar = product.Avatar;

                    ssCart.Add(cartItem);
                }
                else
                {
                    ssCart = (List<CartItemViewModel>)Session[Constant.Cart_Session];
                    if (ssCart.Exists(x => x.productId == productId))
                    {
                        foreach (var item in ssCart)
                        {
                            if (item.productId == productId)
                                item.quantity += quantity;
                        }
                    }
                    else
                    {
                        var cartItem = new CartItemViewModel();
                        cartItem.productId = productId;
                        cartItem.quantity = quantity;
                        cartItem.productCode = product.Code;
                        cartItem.productName = product.Name;
                        cartItem.Amount = product.Price;
                        cartItem.LastAmount = product.SellPrice;
                        cartItem.avatar = product.Avatar;

                        ssCart.Add(cartItem);
                    }
                }
                Session[Constant.Cart_Session] = ssCart;
                return Json(new
                {
                    status = true,
                    totalItem = ssCart.Count,
                    data = new JavaScriptSerializer().Serialize(ssCart)
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult getCartItemNo(string localCart)
        {
            if (Session[Constant.Cart_Session] != null)
            {
                var ssCart = (List<CartItemViewModel>)Session[Constant.Cart_Session];
                return Json(new
                {
                    status = true,
                    totalItem = ssCart.Count
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (String.IsNullOrEmpty(localCart) || localCart == "undefined")
                    return Json(new {
                        status = true,
                        totalItem = -1
                    });
                var ssCart = new JavaScriptSerializer().Deserialize<List<CartItemViewModel>>(localCart);
                Session[Constant.Cart_Session] = ssCart;
                return Json(new
                {
                    status = true,
                    totalItem = ssCart.Count
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult updateQuantity(long productId, Int16 quantity)
        {
            if (Session[Constant.Cart_Session] == null)
                return Json(new { status = 1 });
            if (quantity <= 0)
                return Json(new { status = 2 });
            using (var uow = new UnitOfWork(Shared.connString))
            {
                var product = uow.ProductRepository.ViewDetail(productId);
                if (quantity > product.Quantity)
                    return Json(new { status = 3 });
            }

            List<CartItemViewModel> ssCart = (List<CartItemViewModel>)Session[Constant.Cart_Session];
            var curProduct = ssCart.Where(x => x.productId == productId).SingleOrDefault();
            curProduct.quantity = quantity;
            Session[Constant.Cart_Session] = ssCart;
            var html = RenderHelper.PartialView(this, "/Views/Cart/_cartBox.cshtml", ssCart);
            return Json(new
            {
                status = 4,
                data = new JavaScriptSerializer().Serialize(ssCart),
                html = html
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteItem(long productId)
        {
            if (Session[Constant.Cart_Session] == null)
                return Json(new { status = false });

            List<CartItemViewModel> ssCart = (List<CartItemViewModel>)Session[Constant.Cart_Session];
            var item = ssCart.Where(x => x.productId == productId).SingleOrDefault();

            if (item == null)
                return Json(new { status = false });
            else
            {
                ssCart.Remove(item);
                Session[Constant.Cart_Session] = ssCart;
                return Json(new
                {
                    status = true,
                    data = new JavaScriptSerializer().Serialize(ssCart),
                    totalRow = ssCart.Count,
                    html = RenderHelper.PartialView(this, "/Views/Cart/_cartBox.cshtml", ssCart)
                },JsonRequestBehavior.AllowGet);
            }
        }
    }
}