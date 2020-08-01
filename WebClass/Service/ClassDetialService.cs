using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;
using WebClass.ViewModel;

namespace WebClass.Service
{
    public class ClassDetialService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();
        
        public ClassDetailVM CDList(int id,int uid)
        {
            var q = db.C_Class.Where(c => c.ClassID == id).FirstOrDefault();


            ClassDetailVM CDitem = new ClassDetailVM();
            CDitem.ClassID = q.ClassID;
            CDitem.ClassName = q.ClassName;
            CDitem.ClassTypeName = q.C_ClassType.ClassTypeName;
            CDitem.CreateTypeID = q.CreateTypeID;
            CDitem.CreateUserID = q.CreateUserID;
            CDitem.MainPicture = q.MainPicture;
            CDitem.PreRegisterTime = q.PreRegisterTime;
            CDitem.Price = q.Price;
            CDitem.RegisterTime = q.RegisterTime;
            CDitem.NeedUser = q.NeedUser;
            CDitem.ClassContent = q.ClassContent;


            foreach (var item in q.B_OrderDetail)
            {
                    
                if (uid== item.B_Order.UserID)
                {
                    CDitem.Buyer = true;
                    break;
                }
                else
                {
                    CDitem.Buyer = false;
                }
            }

            if (q.UC_Score.Select(c => c.Score).Count()!=0)
            {
                CDitem.Score= Math.Round(q.UC_Score.Select(c => c.Score).Average(),1);
            }
            else
            {
                CDitem.Score = 0;
            }

            return CDitem;
        }

    }
}