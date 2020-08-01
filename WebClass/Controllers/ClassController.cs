using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
using WebClass.Service;

namespace WebClass.Controllers
{
    public class ClassController : Controller
    {
        private IRepository<C_Class> repository = new Repository<C_Class>();
        private IRepository<UC_Score> repositoryscore = new Repository<UC_Score>();
        AOnlineClassEntities db = new AOnlineClassEntities();
        private TeachingService Tservice = new TeachingService();
        private LearningService Lservice = new LearningService();
        private ClassDetialService CDservice = new ClassDetialService();

        //轉到Teaching頁面
        public ActionResult Teaching(int id = 0)
        {
            return View(Tservice.ClassList(Convert.ToInt32(Request.Cookies["UserID"].Value)));
        }

        public ActionResult Learning(int id = 0)
        {
            return View(Lservice.ClassList(Convert.ToInt32(Request.Cookies["UserID"].Value)));
        }

        public ActionResult ClassDemo(int id)
        {
            C_Class c_class = repository.GetById(id);
            return Content(c_class.ClassContent);
        }

        //新增
        [HttpGet]
        public ActionResult Insert()
        {
            ViewBag.ClassType = db.C_ClassType;
            return View();
        }
        [HttpPost]
        //[ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Insert([Bind(Exclude = "MainPicture")]C_Class class_, HttpPostedFileBase PostPicture)
        {
            if(PostPicture != null)
            {
                int imgSize = PostPicture.ContentLength;
                byte[] imgByte = new byte[imgSize];
                PostPicture.InputStream.Read(imgByte, 0, imgSize);
                class_.MainPicture = imgByte;
            }
            else
            {
                string presetfilename = Request.PhysicalApplicationPath + "/images/nopic.jpg";
                FileStream pfs = new FileStream(presetfilename, FileMode.Open, FileAccess.Read);
                int plength = (int)pfs.Length;
                byte[] presetimage = new byte[plength];
                pfs.Read(presetimage, 0, plength);
                class_.MainPicture = presetimage;
            }

            if (ModelState.IsValid)
            {
                class_.CreateUserID = Convert.ToInt32(Request.Cookies["UserID"].Value);
                class_.PreRegisterTime = class_.RegisterTime.AddDays(-7);
                class_.UpLoadTime = DateTime.Now;
                class_.CreateTypeID = 4;

                repository.Create(class_);
                return RedirectToAction("Teaching", "Class");
            }
            ViewBag.ClassType = db.C_ClassType;
            class_.ClassTypeID = 1;
            return View(class_);
        }

        //修改
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.ClassType = db.C_ClassType;

            ViewBag.MainPicture = repository.GetById(id).MainPicture;

            C_Class class_ = repository.GetById(id);
            return View(class_);
        }
        [HttpPost]
        //[ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "MainPicture")]C_Class class_, HttpPostedFileBase PostPicture)
        {
            if (PostPicture == null)
            {
                class_.MainPicture = db.C_Class.Where(c => c.ClassID == class_.ClassID).Select(c => c.MainPicture).FirstOrDefault();
            }
            else
            {
                int imgSize = PostPicture.ContentLength;
                byte[] imgByte = new byte[imgSize];
                PostPicture.InputStream.Read(imgByte, 0, imgSize);

                class_.MainPicture = imgByte;
            }


            if (ModelState.IsValid)
            {
                
  
                class_.CreateTypeID = 4;
                class_.CreateUserID = db.C_Class.Where(c => c.ClassID == class_.ClassID).Select(c => c.CreateUserID).FirstOrDefault();
                class_.UpLoadTime = db.C_Class.Where(c => c.ClassID == class_.ClassID).Select(c => c.UpLoadTime).FirstOrDefault();
                class_.PreRegisterTime = db.C_Class.Where(c => c.ClassID == class_.ClassID).Select(c => c.RegisterTime).FirstOrDefault().AddDays(-7);

                repository.Update(class_);
                return RedirectToAction("Teaching", "Class");
            }
            ViewBag.ClassType = db.C_ClassType;
            class_.ClassTypeID = 1;
            return View(class_);

        }

        //刪除
        //public ActionResult Delete(int id = 0)
        //{
        //    repository.Delete(repository.GetById(id));
        //    return RedirectToAction("Index");
        //}

        //詳細
        public ActionResult Detial(int id)
        {
            int uid;

            Response.Cookies["classid"].Value = db.C_Class.Find(id).ClassID.ToString();
            Response.Cookies["classname"].Value = HttpUtility.UrlEncode(repository.GetById(id).ClassName);
            Response.Cookies["createuser"].Value = repository.GetById(id).CreateUserID.ToString();
            if (Request.Cookies["UserID"] != null)
            {

                uid = Convert.ToInt32(Request.Cookies["UserID"].Value);
            }
            else
            {
                uid = 0;
            }

            return View(CDservice.CDList(id, uid));
        }

        //拿圖片
        public ActionResult GetImage(int id)
        {
            var X = db.C_Class.Find(id);
            byte[] img = X.MainPicture;
            return File(img, "image/jpeg");
        }

        //開星星
        [HttpGet]
        public ActionResult SendStar(int id)
        {
            ViewBag.ClassID = id;
            if (Request.Cookies["account"].Value != HttpUtility.UrlEncode("訪客") && Request.Cookies["UserID"] != null)
            {
                ViewBag.UID = Convert.ToInt32(Request.Cookies["UserID"].Value);
            }
            
            return PartialView();
        }
        [HttpPost]
        public ActionResult SendStar(/*UC_Score score*/string score,int id=0)
        {
            string str = score.Replace("\uFEFF", "");

            var aaa = JsonConvert.DeserializeObject<UC_Score>(str);

            repositoryscore.Create(aaa);
            return RedirectToAction("Detial", "Class",new { id= id });
        }

        //確認星星
        [HttpGet]
        public ActionResult CheckStar(int id)
        {
            ViewBag.ClassID = id;
            var q = db.UC_Score.Where(c => c.ClassID == id).ToList();

            return PartialView(q);
        }


        public ActionResult GetStudent(int id)
        {
            StudentService service = new StudentService();
            return View(service.GetStudent(id));
        }
    }
}