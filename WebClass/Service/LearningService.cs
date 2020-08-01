using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;
using WebClass.ViewModel;

namespace WebClass.Service
{
    public class LearningService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        public List<LearningVM> ClassList(int id)
        {
            List<LearningVM> classitems = new List<LearningVM>();
            foreach (var item in db.B_OrderDetail)
            {
                LearningVM classitem = new LearningVM();

                classitem.ClassID = item.ClassID;
                classitem.ClassName = item.C_Class.ClassName;
                classitem.Price = item.Price;
                classitem.RegisterTime = item.C_Class.RegisterTime;

                classitem.ClassType = item.C_Class.C_ClassType.ClassTypeName;
                classitem.CreateType = item.C_Class.C_CreateType.CreateTypeName;
                classitem.CreateUser = item.C_Class.U_User.UserName;
                classitem.BuyingTime = item.B_Order.BuyTime;
                classitem.Buyer = item.B_Order.UserID;
                classitems.Add(classitem);
            }
            return classitems.Where(o => o.Buyer == id).ToList();
        }



    }
}