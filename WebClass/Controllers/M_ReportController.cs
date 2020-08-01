using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
using WebClass.Service;

namespace WebClass.Controllers
{
    public class M_ReportController : Controller
    {
        private AOnlineClassEntities db = new AOnlineClassEntities();
        private Repository<ReportList> Rreport = new Repository<ReportList>();

        // GET: M_Report
        //public ActionResult Index()
        //{
        //    var m_Report = db.M_Report.Include(m => m.M_MessageBoard).Include(m => m.M_ReMessageBoard).Include(m => m.M_SolutionType);
        //    return View(m_Report.ToList());
        //}
        public ActionResult Index(int? page = 1)
        {
            var m_Report = db.M_Report.Include(m => m.M_MessageBoard).Include(m => m.M_ReMessageBoard).Include(m => m.M_SolutionType);
            return View(m_Report.ToList().ToPagedList(page ?? 1, 6));
        }


        // GET: M_Report/Create
        public ActionResult Create(int id)
        {
            ViewBag.MessageBoard = id;
            return PartialView();
        }

        // POST: M_Report/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportID,ReportContent,ReportDate,SolDate,SolutionTypeID,MessageBoardID,ReMessageBoardID,ReportUserID")] M_Report m_Report,int id)
        {
            if (ModelState.IsValid)
            {
                db.M_Report.Add(m_Report);
                db.Entry(m_Report).Entity.ReportDate = DateTime.Now.Date;
                db.Entry(m_Report).Entity.SolutionTypeID = 1;
                db.Entry(m_Report).Entity.ReportUserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
                db.Entry(m_Report).Entity.MessageBoardID = Convert.ToInt32(id);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Detial", "Class", new { id = Convert.ToInt32(Request.Cookies["classid"].Value) });
            }


            return View(m_Report);
        }

        // GET: M_Report/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Report m_Report = db.M_Report.Find(id);
            if (m_Report == null)
            {
                return HttpNotFound();
            }
            Response.Cookies["reportdate"].Value = db.Entry(m_Report).Entity.ReportDate.ToString() ;
            ViewBag.MessageBoardID = new SelectList(db.M_MessageBoard, "MessageBoardID", "MessageBoardContent", m_Report.MessageBoardID);
            ViewBag.ReMessageBoardID = new SelectList(db.M_ReMessageBoard, "ReMessageBoardID", "ReMessageBoardContent", m_Report.ReMessageBoardID);
            ViewBag.SolutionTypeID = new SelectList(db.M_SolutionType, "SolutionTypeID", "SolutionTypeName", m_Report.SolutionTypeID);
            ViewBag.ReportUserID = new SelectList(db.U_User, "UserID", "UserName", m_Report.ReportUserID);

            return View(m_Report);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReportID,ReportContent,ReportDate,SolDate,SolutionTypeID,MessageBoardID,ReMessageBoardID,ReportUserID")] M_Report m_Report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(m_Report).State = EntityState.Modified;
                db.Entry(m_Report).Entity.SolDate = DateTime.Now.Date;
                db.Entry(m_Report).Entity.ReportDate =Convert.ToDateTime(Request.Cookies["reportdate"].Value).Date;
                //db.Entry(m_Report).Entity.SolutionTypeID = 2;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MessageBoardID = new SelectList(db.M_MessageBoard, "MessageBoardID", "MessageBoardContent", m_Report.MessageBoardID);
            ViewBag.ReMessageBoardID = new SelectList(db.M_ReMessageBoard, "ReMessageBoardID", "ReMessageBoardContent", m_Report.ReMessageBoardID);
            ViewBag.SolutionTypeID = new SelectList(db.M_SolutionType, "SolutionTypeID", "SolutionTypeName", m_Report.SolutionTypeID);
            ViewBag.ReportUserID = new SelectList(db.U_User, "UserID", "UserName", m_Report.ReportUserID);
            return View(m_Report);
        }


        [HttpPost]
        public ActionResult Solved([Bind(Include = "ReportID,ReportContent,ReportDate,SolDate,SolutionTypeID,MessageBoardID,ReMessageBoardID,ReportUserID")] M_Report m_Report, string reportList)
        {
            var list = JsonConvert.DeserializeObject<List<ReportList>>(reportList);
            foreach (var item in list)
            {
                m_Report = db.M_Report.Find(item.ReportID);
                db.Entry(m_Report).State = EntityState.Modified;
                db.Entry(m_Report).Entity.SolDate = DateTime.Now.Date;
                db.Entry(m_Report).Entity.ReportDate = m_Report.ReportDate;
                db.Entry(m_Report).Entity.SolutionTypeID = item.SolutionTypeID;
                db.SaveChanges();
            }
            
            return Content("Index");

        }
        

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Deleted([Bind(Include = "ReportID,ReportContent,ReportDate,SolDate,SolutionTypeID,MessageBoardID,ReMessageBoardID,ReportUserID")] M_Report m_Report, string reportList)
        {
            var list = JsonConvert.DeserializeObject<List<ReportList>>(reportList);
            foreach (var item in list)
            {
                m_Report = db.M_Report.Find(item.ReportID);
                db.Entry(m_Report).State = EntityState.Modified;
                db.Entry(m_Report).Entity.SolDate = DateTime.Now.Date;
                db.Entry(m_Report).Entity.ReportDate = m_Report.ReportDate;
                db.Entry(m_Report).Entity.SolutionTypeID = item.SolutionTypeID;
                db.SaveChanges();
            }
            return Content("Index");

        }

        // GET: M_Report/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Report m_Report = db.M_Report.Find(id);
            if (m_Report == null)
            {
                return HttpNotFound();
            }
            return View(m_Report);
        }

        // POST: M_Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            M_Report m_Report = db.M_Report.Find(id);
            db.M_Report.Remove(m_Report);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
