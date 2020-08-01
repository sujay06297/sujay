using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
using WebClass.ViewModel;

namespace WebClass.Service
{

    public class ShoppingCarService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        public string AddCart(int userID, int classID)
        {

            B_ShoppingCar car = CheckCart(userID, classID);
            var Check_OrderDetail = db.B_OrderDetail.Where(o => o.B_Order.UserID == userID && o.ClassID == classID).FirstOrDefault();
            string s = string.Empty;
            if (Check_OrderDetail != null)
            {
                s = "已報名此課程";
                return s;
            }
            if (car == null)
            {
                car = new B_ShoppingCar();
                car.UserID = userID;
                car.ClassID = classID;
                db.B_ShoppingCar.Add(car);
                db.SaveChanges();
                s = "";
                return s;
            }
            s = "課程已在購物車";
            return s;
        }
        public B_ShoppingCar CheckCart(int userID, int classID)
        {
            var item = db.B_ShoppingCar.Where(s => s.UserID == userID && s.ClassID == classID).FirstOrDefault();

            return item;
        }
    }
}