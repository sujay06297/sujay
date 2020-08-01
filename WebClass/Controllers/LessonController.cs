using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
using WebClass.Service;

namespace WebClass.Controllers
{
    public class LessonController : Controller
    {
        private IRepository<C_Lesson> repository = new Repository<C_Lesson>();
        AOnlineClassEntities db = new AOnlineClassEntities();
        private LessonService Lessonservice = new LessonService();
        private IRepository<C_LessonCheckTime> reLCT = new Repository<C_LessonCheckTime>();


        //一覽
        public ActionResult Index(int id,int lid=0)
        {
            ViewBag.ClassID = id;
            ViewBag.ClassName = db.C_Class.Where(c => c.ClassID == id).Select(c => c.ClassName).FirstOrDefault();
            return View(Lessonservice.LessonList(id));
        }

        //詳細
        public ActionResult Details(int id, int lid)
        {
            ViewBag.ClassID = id;
            return PartialView(repository.GetById(lid));
        }

        //新增
        public ActionResult Create(int id)
        {
            ViewBag.ClassID = id;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(C_Lesson lesson, int id=0)
        {
            if (ModelState.IsValid)
            {
                repository.Create(lesson);

                return RedirectToAction("Index", "Lesson", new { id = lesson.ClassID });
            }

            return View(lesson);
        }

        //修改
        [HttpGet]
        public ActionResult Edit(int id, int lid)
        {
            ViewBag.ClassID = id;

            return PartialView(repository.GetById(lid));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(C_Lesson lesson)
        {
            repository.Update(lesson);
            
            return RedirectToAction("Index", "Lesson", new { id = lesson.ClassID });
        }

        public ActionResult InClass(string lessonid, string userid, string timenow, string timeall, int id, int lid = 0)
        {

            ViewBag.UID = -1;
            if (Request.Cookies["account"].Value != HttpUtility.UrlEncode("訪客") && Request.Cookies["UserID"] != null)
            {
                ViewBag.UID = Convert.ToInt32(Request.Cookies["UserID"].Value);

                if (lessonid != null)
                {
                    C_LessonCheckTime LCT = new C_LessonCheckTime();

                    LCT.LessonID = Convert.ToInt32(lessonid);
                    LCT.UserID = Convert.ToInt32(userid);
                    LCT.LessonTimeCheck = Convert.ToInt32(timenow);
                    LCT.LessonTime = Convert.ToInt32(timeall);

                    reLCT.Create(LCT);

                }
            }

            ViewBag.ClassID = id;
            return PartialView(Lessonservice.LessonList(id));
        }

        [HttpGet]
        public ActionResult RememberTime()
        {



            return Content("");
        }
        [HttpPost]
        public ActionResult RememberTime(C_LessonCheckTime LCT ,string lessonid, string userid, string timenow, string timeall)
        {
            LCT.LessonID = Convert.ToInt32(lessonid);
            LCT.UserID = Convert.ToInt32(userid);
            LCT.LessonTimeCheck = Convert.ToInt32(timenow);
            LCT.LessonTime = Convert.ToInt32(timeall);

            reLCT.Create(LCT);

            return Content("");
        }
    }
}
