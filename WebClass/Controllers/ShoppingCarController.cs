using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
using WebClass.Service;

namespace WebClass.Controllers
{
    public class ShoppingCarController : Controller

    {
        AOnlineClassEntities db = new AOnlineClassEntities();
        ShoppingCarService car = new ShoppingCarService();
        private Repository<U_User> repository = new Repository<U_User>();
        // GET: ShoppingCar
        //public ActionResult Index()
        //{
        //    return View();
        //}


        public ActionResult AddToCart(int id = 0)
        {
            string CheckClass = "";

            if (Request.Cookies["UserID"] == null)
            {
                return Content("尚未登入");
            }
            else
            {
                int UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
                CheckClass = car.AddCart(UserID, id);
                return Content(CheckClass);
            }
        }

        public ActionResult CartList()
        {
            int f = Convert.ToInt32(Request.Cookies["UserID"].Value);
            List<B_ShoppingCar> sc = db.B_ShoppingCar.Where(id => id.UserID == f).ToList();

            return View(sc);
        }

        public ActionResult DeleteToCar(int id = 0)
        {
            B_ShoppingCar b_ShoppingCar = db.B_ShoppingCar.Find(id);
            db.B_ShoppingCar.Remove(b_ShoppingCar);
            db.SaveChanges();
            return RedirectToAction("CartList");
        }

        public ActionResult AddToOrder()
        {
            return RedirectToAction("OrderCheck", "Order");
        }

        public ActionResult test()
        {
            var q= db.B_ShoppingCar.Distinct().ToList();
            var q1 = from x in db.B_ShoppingCar
                     select x.U_User.UserName.Distinct().ToList();
            return View(q1);
        }

    }
}