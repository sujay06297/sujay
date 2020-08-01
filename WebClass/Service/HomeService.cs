using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;
using WebClass.ViewModel;

namespace WebClass.Service
{
    public class HomeService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        public List<HomeVM> GetTop()
        {

            List<HomeVM> ListVM = new List<HomeVM>();

            List<HomeVM> v = (from f in db.B_OrderDetail
                              join ff in db.C_Class
                              on f.ClassID equals ff.ClassID
                              group f by f.ClassID into g
                              orderby g.Count() descending
                              select new HomeVM
                              {
                                  ClassName = g.FirstOrDefault().C_Class.ClassName.ToString(),
                                  Description = g.FirstOrDefault().C_Class.ClassContent.ToString(),
                                  ID = g.FirstOrDefault().C_Class.ClassID,
                                  Price = g.FirstOrDefault().C_Class.Price,
                                  Status = g.FirstOrDefault().C_Class.CreateTypeID
                              }
                    ).Take(3).ToList();

            return v;
        }

        public List<HomeVM> GetNew()
        {

            List<HomeVM> ListVM = new List<HomeVM>();

            List<HomeVM> v = (from f in db.C_Class
                              orderby f.UpLoadTime descending
                              where f.CreateTypeID < 3
                              select new HomeVM
                              {
                                  ClassName = f.ClassName.ToString(),
                                  Description = f.ClassContent.ToString(),
                                  ID = f.ClassID,
                                  Price = f.Price,
                                  Status = f.CreateTypeID
                              }
                    ).Take(3).ToList();

            return v;
        }

        public List<HomeVM> Recommend()
        {
            int UserID = Convert.ToInt32(HttpContext.Current.Request.Cookies["UserID"].Value);

            if (HttpContext.Current.Request.Cookies["UserID"].Value == null)
            {
                UserID = 0;
            }

            var vv = (from f in db.B_Order
                      join ff in db.B_OrderDetail
                      on f.OrderID equals ff.OrderID
                      join fff in db.U_User
                      on f.UserID equals fff.UserID
                      join ffff in db.C_Class
                      on ff.ClassID equals ffff.ClassID
                      where f.UserID == UserID && ff.ClassID == ffff.ClassID
                      select new
                      {
                          ClassID = ffff.ClassID,
                          ClassTypeID = ffff.ClassTypeID
                      }).ToList().FirstOrDefault();
            Random r = new Random();
            int n = r.Next(1, 15);
            if (vv == null)
            {
                vv = (from f in db.C_Class
                      join ff in db.C_ClassType
                      on f.ClassTypeID equals ff.ClassTypeID
                      where f.ClassTypeID == n
                      select new
                      {
                          ClassID = f.ClassID,
                          ClassTypeID = f.ClassTypeID
                      }).ToList().FirstOrDefault();
            }

            List<HomeVM> v = (from f in db.C_Class
                              join ff in db.C_ClassType
                              on f.ClassTypeID equals ff.ClassTypeID
                              where ff.ClassTypeID == vv.ClassTypeID && f.CreateTypeID < 3
                              select new HomeVM
                              {

                                  ClassName = f.ClassName,
                                  Description = f.ClassContent,
                                  ID = f.ClassID,
                                  Price = f.Price,
                                  Status = f.CreateTypeID

                              }).ToList();

            List<HomeVM> vvv = (from f in db.B_Order
                                join ff in db.B_OrderDetail
                                on f.OrderID equals ff.OrderID
                                join fff in db.U_User
                                on f.UserID equals fff.UserID
                                join ffff in db.C_Class
                                on ff.ClassID equals ffff.ClassID
                                where f.UserID == UserID && ff.ClassID == ffff.ClassID
                                select new HomeVM
                                {
                                    ClassName = ffff.ClassName,
                                    Description = ffff.ClassContent,
                                    ID = ffff.ClassID,
                                    Price = ffff.Price,
                                    Status = ffff.CreateTypeID
                                }).ToList();

            foreach (HomeVM x in vvv)
            {
                for (int i = 0; i <= v.Count() - 1;)
                {
                    if (v[i].ID == x.ID)
                    {
                        v.RemoveAt(i);
                        continue;
                    }
                    i++;
                }
            }


            return v.Take(3).ToList();

        }

        public IEnumerable<TopTeacherVM> GetTopTeacher()
        {

            var x = db.U_User.Where(L => L.PermissionID != 1).Select(L => new { L.UserID, total = L.C_Class.Select(c => c.B_OrderDetail.Count).Sum() }).OrderByDescending(L => L.total);

            IEnumerable<TopTeacherVM> v = db.U_User.Where(L => L.PermissionID != 2).Select(L => new TopTeacherVM { TopTeacherName = L.UserName, TopTeacherID = L.UserID, TopStudentNumber = L.C_Class.Select(c => c.B_OrderDetail.Count).Sum() }).OrderByDescending(L => L.TopStudentNumber).Take(3);

            return v;
        }


    }
}