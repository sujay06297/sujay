using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;

namespace WebClass.Service
{
    public class SmallBellService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        public void AddSmallBell(int id, string content)
        {
            var users = db.U_User.Select(x => x.UserID).ToList();
            foreach (var item in users)
            {
                U_SmallBell sb = new U_SmallBell();
                sb.ClassID = id;
                sb.NoticeContent = content;
                sb.CreatedDate = DateTime.Now;
                sb.StatusTypeID = 1;

                sb.UserID = item;
                db.U_SmallBell.Add(sb);
            }
            db.SaveChanges();
        }

        public void RecommendClass()
        {
            var users = db.U_User.Select(L => L.UserID).ToList();
            foreach (var item in users)
            {
                U_SmallBell sb = new U_SmallBell();

                var myLearnedClass = db.B_OrderDetail.Where(L => L.B_Order.UserID == item).GroupBy(L => L.C_Class.ClassTypeID).OrderByDescending(L => L.Count()).FirstOrDefault();
                myLearnedClass = myLearnedClass == null ? db.B_OrderDetail.Where(L => L.C_Class.ClassTypeID == 1).GroupBy(L => L.C_Class.ClassTypeID).FirstOrDefault() : myLearnedClass;
                int classID = myLearnedClass.Key;
                List<C_Class> classlist = myLearnedClass.Select(L => L.C_Class).ToList();
                List<C_Class> allClass = db.C_Class.Where(L => L.ClassTypeID == classID).ToList();

                foreach (var x in classlist)
                {
                    allClass.Remove(x);

                }
                int y = allClass.OrderBy(L => Guid.NewGuid()).FirstOrDefault().ClassID;

                sb.ClassID = y;
                sb.NoticeContent = "趕快來看看推薦課程吧!";
                sb.CreatedDate = DateTime.Now;
                sb.StatusTypeID = 1;

                sb.UserID = item;
                db.U_SmallBell.Add(sb);
            }
            db.SaveChanges();

        }

        public void AddClassNotice(int classid, string content)
        {
            U_SmallBell sb = new U_SmallBell();
            sb.ClassID = classid;
            sb.UserID = db.C_Class.Where(c => c.ClassID == classid).Select(u => u.U_User.UserID).FirstOrDefault();
            sb.NoticeContent = content;
            sb.CreatedDate = DateTime.Now;
            sb.StatusTypeID = 2;

            db.U_SmallBell.Add(sb);
            db.SaveChanges();
        }

        public void AddMessageNotice(int classid, string content)
        {
            U_SmallBell sb = new U_SmallBell();
            sb.ClassID = classid;
            sb.UserID = db.C_Class.Where(c => c.ClassID == classid).Select(u => u.U_User.UserID).FirstOrDefault();
            sb.NoticeContent = content;
            sb.CreatedDate = DateTime.Now;
            sb.StatusTypeID = 4;

            db.U_SmallBell.Add(sb);
            db.SaveChanges();
        }

        public void SmallBellRead(int Userid)
        {
            var items = db.U_SmallBell.Where(s => s.UserID == Userid);
            foreach (var item in items)
            {
                item.ReadYN = true;
            }
            db.SaveChanges();
        }
    }
}