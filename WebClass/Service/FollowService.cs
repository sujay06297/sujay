using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;

namespace WebClass.Service
{
    public class FollowService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        //public void AddFollow(int userID, int classID)
        //{
        //    UC_Follow follow = Check(userID, classID);
        //    if (follow == null)
        //    {
        //        follow = new UC_Follow();
        //        follow.UserID = userID;
        //        follow.ClassID = classID;
        //        db.UC_Follow.Add(follow);
        //    }
        //    db.SaveChanges();
        //}

        public string AddFollow(int userID, int classID)
        {
            UC_Follow follow = Check(userID, classID);
            if (follow == null)
            {
                follow = new UC_Follow();
                follow.UserID = userID;
                follow.ClassID = classID;
                db.UC_Follow.Add(follow);
                db.SaveChanges();
                return "成功加入收藏";
            }
            return "已收藏此課程";
        }

        public UC_Follow Check(int userID, int classID)
        {
            var item = db.UC_Follow.Where(s => s.UserID == userID && s.ClassID == classID).FirstOrDefault();
            return item;
        }



    }
}