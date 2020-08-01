using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;
using WebClass.ViewModel;

namespace WebClass.Service
{
    public class StudentService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        public List<StudentVM> GetStudent(int id)
        {
            List<StudentVM> list = (from f in db.C_Class
                                    join ff in db.B_OrderDetail
                                    on f.ClassID equals ff.ClassID
                                    join fff in db.B_Order
                                    on ff.OrderID equals fff.OrderID
                                    join ffff in db.U_User
                                    on fff.UserID equals ffff.UserID
                                    where f.ClassID == id
                                    select new StudentVM
                                    {
                                        Account = ffff.UserAccount,
                                        Name = ffff.UserName,
                                        BuyTime = fff.BuyTime,
                                        Gender = ffff.Gender,
                                        Email = ffff.Email
                                    }).ToList();

            return list;

        }
    }
}