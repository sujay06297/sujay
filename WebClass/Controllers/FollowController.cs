using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
using WebClass.Service;

namespace WebClass.Controllers
{
    public class FollowController : Controller
    {
        AOnlineClassEntities db = new AOnlineClassEntities();
        FollowService fs = new FollowService();

        // GET: Follow
        public ActionResult Index(int id = 0, int ii = 0)
        {
            int r = Convert.ToInt32(Request.Cookies["UserID"].Value);
            fs.AddFollow(r, id);
            return RedirectToAction("Browse", "Home", new { id = ii });
        }

        public ActionResult AddFollow(int id = 0)
        {

            if (Request.Cookies["UserID"] == null)
            {
                return Content("尚未登入");
            }
            int UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
            var text = fs.AddFollow(UserID, id);
            return Content(text);
        }

        public ActionResult FollowList()
        {
            int f = Convert.ToInt32(Request.Cookies["UserID"].Value);
            List<UC_Follow> od = db.UC_Follow.Where(o => o.UserID == f).ToList();
            return View(od);
        }

        public ActionResult DeleteToFollow(int id = 0)
        {
            UC_Follow uf = db.UC_Follow.Find(id);
            db.UC_Follow.Remove(uf);
            db.SaveChanges();
            return RedirectToAction("FollowList");
        }

    }
}