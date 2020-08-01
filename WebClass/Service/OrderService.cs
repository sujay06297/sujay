using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;
using WebClass.ViewModel;

namespace WebClass.Service
{
    public class OrderService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        //加入訂單
        public void AddOrder(int userID)
        {
            //加入訂單資料
            B_Order o = new B_Order();
            o.UserID = userID;
            o.BuyTime = DateTime.Now;
            db.B_Order.Add(o);
            db.SaveChanges();

            //加入訂單明細資料
            var items = db.B_ShoppingCar.Where(s => s.UserID == userID);
            List<B_OrderDetail> OD = new List<B_OrderDetail>();
            foreach (var item in items)
            {
                B_OrderDetail od = new B_OrderDetail();

                od.OrderID = o.OrderID;

                od.ClassID = item.ClassID;
                od.Price = item.C_Class.Price;

                OD.Add(od);
                db.B_OrderDetail.AddRange(OD);
            }
            db.SaveChanges();
        }

        public void DeleteShoppingCar(int userID)
        {
            B_ShoppingCar sc = new B_ShoppingCar();
            var q = from n in db.B_ShoppingCar
                    where n.UserID == userID
                    select n;
            db.B_ShoppingCar.RemoveRange(q);
            db.SaveChanges();
        }
    }
}