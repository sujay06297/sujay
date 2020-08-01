using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
//using WebClass.Servce;
using WebClass.Service;

namespace WebClass.Controllers
{
    public class OrderController : Controller
    {
        OrderService os = new OrderService();
        ShoppingCarService car = new ShoppingCarService();
        AOnlineClassEntities db = new AOnlineClassEntities();

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderCheck()
        {
            int f = Convert.ToInt32(Request.Cookies["UserID"].Value);
            List<B_ShoppingCar> sc = db.B_ShoppingCar.Where(id => id.UserID == f).ToList();

            return View(sc);
        }

        public ActionResult OrderCheckout(int id = 0)
        {
            os.AddOrder(Convert.ToInt32(Request.Cookies["UserID"].Value));
            os.DeleteShoppingCar(Convert.ToInt32(Request.Cookies["UserID"].Value));
            ViewBag.message = "付款成功";
            return RedirectToAction("OrderDetail");
        }

        public ActionResult OrderDetail()
        {
            int cookieID = Convert.ToInt32(Request.Cookies["UserID"].Value);

            List<B_Order> od = db.B_Order.Where(o => o.UserID == cookieID).OrderByDescending(t => t.BuyTime).ToList();

            return View(od);
        }
    }
}