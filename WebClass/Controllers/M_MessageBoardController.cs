using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;

namespace WebClass.Controllers
{
    public class M_MessageBoardController : Controller
    {
        private AOnlineClassEntities db = new AOnlineClassEntities();

        // GET: M_MessageBoard
        public ActionResult _Index()
        {
            //var m_MessageBoard = db.M_MessageBoard.Include(m => m.C_Class).Include(m => m.C_Lesson).Include(m => m.M_PostType).Include(m => m.U_User);
            //return PartialView(m_MessageBoard.ToList());

            var m_MessageBoard = db.M_MessageBoard.Include(m => m.C_Class).Include(m => m.C_Lesson).Include(m => m.M_PostType).Include(m => m.U_User);
            //int id = Convert.ToInt32(Request.Cookies["UserID"].Value);
            string classname = HttpUtility.UrlDecode(Request.Cookies["classname"].Value, System.Text.Encoding.GetEncoding("UTF-8"));
            var q = from userid in m_MessageBoard
                    where userid.C_Class.ClassName == classname
                    select userid;
            return PartialView(q.ToList());
        }

        public ActionResult Re_Index(int id, int reboardid)
        {
            var re_Messageboard = db.M_ReMessageBoard.Include(r => r.M_MessageBoard);
            int messageboardid = Convert.ToInt32(reboardid);
            var q = from boardid in re_Messageboard
                    where boardid.M_MessageBoard.MessageBoardID == messageboardid
                    select boardid;

            return PartialView(q.ToList());

            //M_ReMessageBoard m_ReMessageBoard = db.M_ReMessageBoard.Find(id);
            //return PartialView(m_ReMessageBoard);
        }
        

        [ChildActionOnly]
        public ActionResult _Create()
        {
            ViewBag.ClassID = new SelectList(db.C_Class, "ClassID", "ClassName");
            //ViewBag.LessonID = new SelectList(db.C_Lesson, "LessonID", "LessonName");
            ViewBag.PostTypeID = new SelectList(db.M_PostType, "PostTypeID", "PostTypeName");
            ViewBag.UserID = new SelectList(db.U_User, "UserID", "UserAccount");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "MessageBoardID,MessageBoardContent,PostTime,Status,UserID,ClassID,LessonID,PostTypeID")] M_MessageBoard m_MessageBoard)
        {
            if (ModelState.IsValid)
            {
                db.M_MessageBoard.Add(m_MessageBoard);
                db.Entry(m_MessageBoard).Entity.PostTime = DateTime.Now.Date;
                db.Entry(m_MessageBoard).Entity.Status = true;
                db.Entry(m_MessageBoard).Entity.PostTypeID = 5;
                db.Entry(m_MessageBoard).Entity.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
                db.Entry(m_MessageBoard).Entity.ClassID = Convert.ToInt32(Request.Cookies["classid"].Value);
                db.SaveChanges();
                return RedirectToAction("Detial", "Class", new { id = Convert.ToInt32(Request.Cookies["classid"].Value) });

            }

            ViewBag.ClassID = new SelectList(db.C_Class, "ClassID", "ClassName", m_MessageBoard.ClassID);
            //ViewBag.LessonID = new SelectList(db.C_Lesson, "LessonID", "LessonName", m_MessageBoard.LessonID);
            ViewBag.PostTypeID = new SelectList(db.M_PostType, "PostTypeID", "PostTypeName", m_MessageBoard.PostTypeID);
            ViewBag.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
            return PartialView(m_MessageBoard);
        }

        

        // GET: M_MessageBoard/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_MessageBoard m_MessageBoard = db.M_MessageBoard.Find(id);
            if (m_MessageBoard == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.C_Class, "ClassID", "ClassName", m_MessageBoard.ClassID);
            //ViewBag.LessonID = new SelectList(db.C_Lesson, "LessonID", "LessonName", m_MessageBoard.LessonID);
            ViewBag.PostTypeID = new SelectList(db.M_PostType, "PostTypeID", "PostTypeName", m_MessageBoard.PostTypeID);
            ViewBag.UserID = new SelectList(db.U_User, "UserID", "UserAccount", m_MessageBoard.UserID);
            return PartialView(m_MessageBoard);
        }

        // POST: M_MessageBoard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageBoardID,MessageBoardContent,PostTime,Status,UserID,ClassID,LessonID,PostTypeID")] M_MessageBoard m_MessageBoard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(m_MessageBoard).State = EntityState.Modified;
                db.Entry(m_MessageBoard).Entity.PostTime = DateTime.Now.Date;
                db.Entry(m_MessageBoard).Entity.Status = true;
                db.Entry(m_MessageBoard).Entity.PostTypeID = 5;

                db.Entry(m_MessageBoard).Entity.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
                db.Entry(m_MessageBoard).Entity.ClassID = Convert.ToInt32(Request.Cookies["classid"].Value);
                db.SaveChanges();
                //return RedirectToAction("Index", "Class");
                return RedirectToAction("Detial", "Class", new { id = Convert.ToInt32(Request.Cookies["classid"].Value) });
            }
            ViewBag.ClassID = new SelectList(db.C_Class, "ClassID", "ClassName", m_MessageBoard.ClassID);
            //ViewBag.LessonID = new SelectList(db.C_Lesson, "LessonID", "LessonName", m_MessageBoard.LessonID);
            ViewBag.PostTypeID = new SelectList(db.M_PostType, "PostTypeID", "PostTypeName", m_MessageBoard.PostTypeID);
            ViewBag.UserID = new SelectList(db.U_User, "UserID", "UserAccount", m_MessageBoard.UserID);
            return PartialView(m_MessageBoard);
        }

        // GET: M_MessageBoard/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_MessageBoard m_MessageBoard = db.M_MessageBoard.Find(id);
            if (m_MessageBoard == null)
            {
                return HttpNotFound();
            }
            return PartialView(m_MessageBoard);
        }

        // POST: M_MessageBoard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            M_MessageBoard m_MessageBoard = db.M_MessageBoard.Find(id);
            db.M_MessageBoard.Remove(m_MessageBoard);
            db.SaveChanges();
            return RedirectToAction("Detial", "Class", new { id = Convert.ToInt32(Request.Cookies["classid"].Value) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult List()
        {
            return PartialView("_Index", db.M_MessageBoard.ToList());
        }


        public ActionResult Reply(int id)
        {
            ViewBag.UserID = new SelectList(db.U_User, "UserID", "UserAccount");
            ViewBag.MessageBoard = id;
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply([Bind(Include = "ReMessageBoardID,ReMessageBoardContent,RePostTime,Status,UserID,MessageBoardID")] M_ReMessageBoard m_ReMessageBoard, int id)
        {
            if (ModelState.IsValid)
            {

                //M_MessageBoard m_MessageBoard = db.M_MessageBoard.Find(id);
                db.M_ReMessageBoard.Add(m_ReMessageBoard);
                db.Entry(m_ReMessageBoard).Entity.RePostTime = DateTime.Now.Date;
                db.Entry(m_ReMessageBoard).Entity.Status = true;
                db.Entry(m_ReMessageBoard).Entity.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
                db.Entry(m_ReMessageBoard).Entity.MessageBoardID = Convert.ToInt32(id);

                db.SaveChanges();
                return RedirectToAction("Detial", "Class", new { id = Convert.ToInt32(Request.Cookies["classid"].Value) });

            }

            //ViewBag.ClassID = new SelectList(db.M_MessageBoard, "ClassID", "ClassName", m_ReMessageBoard.ClassID);
            //ViewBag.LessonID = new SelectList(db.C_Lesson, "LessonID", "LessonName", m_ReMessageBoard.LessonID);
            ViewBag.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
            return PartialView(m_ReMessageBoard);
        }

        public ActionResult Re_Edit(int? reboardid, int? id)
        {
            if (reboardid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_ReMessageBoard m_ReMessageBoard = db.M_ReMessageBoard.Find(reboardid);
            if (m_ReMessageBoard == null)
            {
                return HttpNotFound();
            }
            Response.Cookies["boardid"].Value = m_ReMessageBoard.MessageBoardID.ToString();
            ViewBag.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
            return PartialView(m_ReMessageBoard);
        }

        // POST: M_MessageBoard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Re_Edit([Bind(Include = "ReMessageBoardID,ReMessageBoardContent,RePostTime,Status,UserID")] M_ReMessageBoard m_ReMessageBoard/*,int? id*/)
        {
            if (ModelState.IsValid)
            {
                //m_ReMessageBoard = db.M_ReMessageBoard.Find(id);

                db.Entry(m_ReMessageBoard).State = EntityState.Modified;
                db.Entry(m_ReMessageBoard).Entity.MessageBoardID = Convert.ToInt32(Request.Cookies["boardid"].Value);
                db.Entry(m_ReMessageBoard).Entity.RePostTime = DateTime.Now.Date;
                db.Entry(m_ReMessageBoard).Entity.Status = true;
                db.Entry(m_ReMessageBoard).Entity.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
                db.SaveChanges();
                return RedirectToAction("Detial", "Class", new { id = Convert.ToInt32(Request.Cookies["classid"].Value) });
            }
            ViewBag.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
            return PartialView(m_ReMessageBoard);
        }

        public ActionResult Re_Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_ReMessageBoard m_ReMessageBoard = db.M_ReMessageBoard.Find(id);
            if (m_ReMessageBoard == null)
            {
                return HttpNotFound();
            }
            return PartialView(m_ReMessageBoard);
        }

        // POST: M_MessageBoard/Delete/5
        [HttpPost, ActionName("Re_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Re_DeleteConfirmed(int id)
        {
            M_ReMessageBoard m_ReMessageBoard = db.M_ReMessageBoard.Find(id);
            db.M_ReMessageBoard.Remove(m_ReMessageBoard);
            db.SaveChanges();
            return RedirectToAction("Detial", "Class", new { id = Convert.ToInt32(Request.Cookies["classid"].Value) });
        }

    }
}
