using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;
using WebClass.ViewModel;

namespace WebClass.Service
{

    public class TeachingService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        public List<TeachingVM> ClassList(int id)
        {
            List<TeachingVM> classitems = new List<TeachingVM>();
           var x =db.C_Class.ToList();
            foreach (var item in x)
            {
                TeachingVM classitem = new TeachingVM();

                classitem.ClassID = item.ClassID;
                classitem.ClassName = item.ClassName;
                classitem.Price = item.Price;
                classitem.RegisterTime = item.RegisterTime;
                classitem.CreateType = item.C_CreateType.CreateTypeName;
                classitem.CreateTypeID = item.C_CreateType.CreateTypeID;
                classitem.ClassType = item.C_ClassType.ClassTypeName;


                var order = db.B_OrderDetail.Where(o => o.ClassID == item.ClassID).Select(o => o.B_Order.UserID).Count();

                classitem.CreateUser = item.CreateUserID;

                classitem.ClassStudentNumber = order;
                classitems.Add(classitem);
            }
            return classitems.Where(c => c.CreateUser == id).ToList();
        }
    }
}