using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
using WebClass.ViewModel;

namespace WebClass.Controllers
{
    public class UserController : Controller
    {
        private IRepository<U_User> repository = new Repository<U_User>();
        private IRepository<U_UserSkill> repositoryUS = new Repository<U_UserSkill>();
        private AOnlineClassEntities db = new AOnlineClassEntities();


        //登入，回到進入時的頁面

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM login)
        {
            var user = db.U_User.FirstOrDefault(us => us.UserAccount == login.account && us.UserPassword == login.password);
            if (user != null)
            {
                ViewBag.message = "登入成功";
                Response.Cookies["UserID"].Value = user.UserID.ToString();
                Response.Cookies["login"].Value = HttpUtility.UrlEncode(user.UserName);
                Response.Cookies["account"].Value = HttpUtility.UrlEncode(user.UserAccount);
                Response.Cookies["permission"].Value = user.PermissionID.ToString();

                if (login.remember == "yes")
                {
                    //產生cookies
                    Response.Cookies["login"].Expires = DateTime.Now.AddDays(7);
                    Response.Cookies["account"].Expires = DateTime.Now.AddDays(7);
                }

                if (Session["path"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(Session["path"].ToString());
                }
            }
            else
            {
                Response.Cookies["account"].Value = HttpUtility.UrlEncode("訪客");
                ViewBag.message = "登入失敗";
            }

            return View();
        }

        //登出
        public ActionResult Logout()
        {
            //刪除cookies中的資料
            Response.Cookies["login"].Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies["UserID"].Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies["account"].Value = HttpUtility.UrlEncode("訪客");
            Response.Cookies["permission"].Expires = DateTime.Now.AddSeconds(-1);
            //刪除session中的資料
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert([Bind(Exclude = "UserPhoto")]U_User user, HttpPostedFileBase UserPhoto, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {

                if (db.U_User.Where(o => o.UserAccount == user.UserAccount).FirstOrDefault() != null)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('使用者帳號已存在，請重新輸入!');history.go(-1);</script>");
                }

                else if (user.UserPassword != ConfirmPassword)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('確認密碼不正確，請重新輸入!');history.go(-1);</script>");
                }

                else
                {

                    user.PermissionID = Convert.ToInt32(db.U_Permission.Where(p => p.PermissionID == 2).Select(p => p.PermissionID).First());
                    user.CreateTime = DateTime.Now;
                    //repository.Create(user);

                    if (UserPhoto != null)
                    {
                        byte[] imageByte = null;
                        BinaryReader reader = new BinaryReader(UserPhoto.InputStream);
                        imageByte = reader.ReadBytes((int)UserPhoto.ContentLength);
                        user.UserPhoto = imageByte;
                        repository.Create(user);

                        Response.Cookies["UserID"].Value = user.UserID.ToString();
                        Response.Cookies["login"].Value = HttpUtility.UrlEncode(user.UserName);
                        Response.Cookies["account"].Value = HttpUtility.UrlEncode(user.UserAccount);
                        Response.Cookies["permission"].Value = user.PermissionID.ToString();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {

                        string presetfilename = Request.PhysicalApplicationPath + "images/nouser.png";
                        FileStream pfs = new FileStream(presetfilename, FileMode.Open, FileAccess.Read);
                        int plength = (int)pfs.Length;
                        byte[] presetimage = new byte[plength];
                        pfs.Read(presetimage, 0, plength);

                        user.UserPhoto = presetimage;
                        repository.Create(user);

                        Response.Cookies["UserID"].Value = user.UserID.ToString();
                        Response.Cookies["login"].Value = HttpUtility.UrlEncode(user.UserName);
                        Response.Cookies["account"].Value = HttpUtility.UrlEncode(user.UserAccount);
                        Response.Cookies["permission"].Value = user.PermissionID.ToString();

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (Request.Cookies["UserID"].Value != id.ToString())
            {
                return Content("<script language='javascript' type='text/javascript'>alert('帳戶錯誤，請勿更改網址!');history.go(-1);</script>");
            }

            else
            {
                return View(repository.GetById(id));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "UserPhoto")]U_User user, HttpPostedFileBase UserPhoto, string ConfirmPassword)
        {
            string s = HttpUtility.HtmlDecode(Request.Cookies["account"].Value);
            var v = db.U_User.Where(x => x.UserAccount == s).FirstOrDefault();
            if (user.UserAccount != v.UserAccount)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('帳戶錯誤，請勿更改網址!');history.go(-1);</script>");
            }
            else if (ModelState.IsValid)
            {

                if (user.UserPassword != ConfirmPassword)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('確認密碼不正確，請重新輸入!');history.go(-1);</script>");
                }

                else
                {
                    U_User u = repository.GetById(v.UserID);
                    u.UserName = user.UserName;
                    u.UserPassword = user.UserPassword;
                    u.Gender = user.Gender;
                    u.Email = user.Email;
                    u.Phone = user.Phone;
                    u.Address = user.Address;
                    u.Birth = user.Birth;

                    if (UserPhoto != null)
                    {
                        byte[] imageByte = null;
                        BinaryReader reader = new BinaryReader(UserPhoto.InputStream);
                        imageByte = reader.ReadBytes((int)UserPhoto.ContentLength);
                        u.UserPhoto = imageByte;
                        repository.Update(u);

                        Response.Cookies["login"].Value = HttpUtility.UrlEncode(u.UserName);

                        return RedirectToAction("Account");
                    }
                    else
                    {
                        u.UserPhoto = v.UserPhoto;
                        repository.Update(u);

                        Response.Cookies["login"].Value = HttpUtility.UrlEncode(u.UserName);

                        return RedirectToAction("Account");
                    }
                }
            }
            return View(user);
        }



        public ActionResult GetPicture(int id = 0)
        {
            U_User user = db.U_User.Find(id);

            byte[] img = user.UserPhoto;
            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            else
            {
                return ViewBag.nothing;
            }
        }

        public ActionResult Account()
        {
            if (Request.Cookies["account"].Value == null)
            {
                Session["path"] = Request.Url.AbsolutePath;
                return RedirectToAction("Login", "User");
            }
            else
            {
                string s = Request.Cookies["account"].Value;
                ViewBag.UserAccount = s;

                var v = db.U_User.Where(u => u.UserAccount == s).FirstOrDefault();

                ViewBag.UserID = v.UserID;

                ViewBag.UserName = v.UserName;
                ViewBag.Email = v.Email;
                ViewBag.Gender = v.Gender;

                string year = Convert.ToDateTime(v.Birth).Year.ToString();
                string month = Convert.ToDateTime(v.Birth).Month.ToString();
                string day = Convert.ToDateTime(v.Birth).Day.ToString();

                ViewBag.Birth = $"{year}/{month}/{day}";

                ViewBag.Phone = v.Phone;
                ViewBag.Address = v.Address;
                ViewBag.CreateTime = v.CreateTime;

                if (v.PermissionID == 1)
                {
                    ViewBag.Rank = "管理員";
                }
                else if (v.PermissionID == 2)
                {
                    ViewBag.Rank = "一般使用者";
                }
                else if (v.PermissionID == 3)
                {
                    ViewBag.Rank = "教師使用者";
                }

                return View();
            }
        }

        //審查課程

        //讀取所有課程CreateTypeID == 4(送審中)的資料+分頁
        public ActionResult ClassCheck()
        {
            List<C_Class> od = db.C_Class.Where(o => o.CreateTypeID == 4).ToList();

            return View(od.ToList());
        }

        //讀取所有課程CreateTypeID == 2(開課中)的資料+分頁
        public ActionResult AllPassedClass()
        {
            //找審查人資料
            int userid = Convert.ToInt32(Request.Cookies["UserID"].Value);
            var q1 = db.U_User.Where(o => o.UserID == userid).Select(p => p.UserName).FirstOrDefault();
            ViewBag.Editor = q1;

            List<C_Class> APC = db.C_Class.Where(o => o.CreateTypeID == 2).ToList();
            return View(APC.ToList());
        }

        //讀取所有課程CreateTypeID == 5(送審失敗)的資料+分頁
        public ActionResult AllFailedClass()
        {
            //找審查人資料
            int userid = Convert.ToInt32(Request.Cookies["UserID"].Value);
            var q2 = db.U_User.Where(o => o.UserID == userid).Select(p => p.UserName).FirstOrDefault();
            ViewBag.Editor = q2;

            List<C_Class> AFC = db.C_Class.Where(o => o.CreateTypeID == 5).ToList();
            return View(AFC.ToList());
        }

        //讀取所有課程CreateTypeID == 1(已開課)的資料+分頁
        public ActionResult AllHavingClass()
        {
            //找審查人資料
            int userid = Convert.ToInt32(Request.Cookies["UserID"].Value);
            var q3 = db.U_User.Where(o => o.UserID == userid).Select(p => p.UserName).FirstOrDefault();
            ViewBag.Editor = q3;

            List<C_Class> AHC = db.C_Class.Where(o => o.CreateTypeID == 1).ToList();
            return View(AHC.ToList());
        }

        //抓資料(純粹看課程介紹)
        public ActionResult ClassInfoOnly(int id = 0)
        {
            return View(db.C_Class.Find(id));
        }

        //抓資料(包含審核功能)
        [HttpGet]
        public ActionResult EditClassCheck(int id = 0)
        {
            return View(db.C_Class.Find(id));
        }

        //修改-審核通過
        [HttpGet]
        public ActionResult EditClassCheckPassed(int id = 0)
        {
            C_Class _Class = db.C_Class.Where(L => L.ClassID == id).First();
            _Class.CreateTypeID = Convert.ToInt32(Request.QueryString["CreateTypeID"]);
            _Class.U_User.PermissionID = Convert.ToInt32(Request.QueryString["PermissionID"]);

            db.SaveChanges();
            return RedirectToAction("ClassCheck");
        }

        //修改-審核沒過
        [HttpGet]
        public ActionResult EditClassCheckFailed(int id = 0)
        {
            C_Class _Class = db.C_Class.Where(F => F.ClassID == id).First();
            _Class.CreateTypeID = Convert.ToInt32(Request.QueryString["CreateTypeID"]);

            db.SaveChanges();
            return RedirectToAction("ClassCheck");
        }

        //管理員頁面
        public ActionResult Administrator()
        {
            List<C_Class> B = db.C_Class.ToList();

            return View(B);
        }

        //所有會員資料+分頁
        public ActionResult AlluserInfo()
        {
            List<U_User> op = db.U_User.ToList();
            return View(op.ToList());
        }

        //抓資料-課程申請人資料
        [HttpGet]
        public ActionResult ApplicantInfo(int id = 0)
        {
            return View(db.C_Class.Find(id));
        }

        //抓資料-圓餅圖會員性別
        public ActionResult PieChartforGender()
        {
            int male = db.U_User.Where(x => x.Gender == "男").Count();
            int female = db.U_User.Where(x => x.Gender == "女").Count();
            Ratio obj = new Ratio();
            obj.Male = male;
            obj.Female = female;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public class Ratio
        {
            public int Male { get; set; }
            public int Female { get; set; }
        }

        //抓資料-圓餅圖課審狀況
        public ActionResult PieChartforClassEdit()
        {
            int havingclass = db.C_Class.Where(x => x.CreateTypeID == 1).Count();
            int passedclass = db.C_Class.Where(x => x.CreateTypeID == 2).Count();
            int failedclass = db.C_Class.Where(x => x.CreateTypeID == 3).Count();
            int waitingtoexaming = db.C_Class.Where(x => x.CreateTypeID == 4).Count();
            int notpassedclass = db.C_Class.Where(x => x.CreateTypeID == 5).Count();
            ExamingRatio obj1 = new ExamingRatio();
            obj1.HavingClass = havingclass;
            obj1.PassedClass = passedclass;
            obj1.FailedClass = failedclass;
            obj1.WaitingToExaming = waitingtoexaming;
            obj1.NotpassedClass = notpassedclass;
            return Json(obj1, JsonRequestBehavior.AllowGet);
        }
        public class ExamingRatio
        {
            public int HavingClass { get; set; }
            public int PassedClass { get; set; }
            public int FailedClass { get; set; }
            public int WaitingToExaming { get; set; }
            public int NotpassedClass { get; set; }
        }

        //抓資料-長條圖課程類型分佈
        public ActionResult BarChartforClass()
        {
            int language = db.C_Class.Where(x => x.ClassTypeID == 1).Count();
            int financial = db.C_Class.Where(x => x.ClassTypeID == 2).Count();
            int chemistry = db.C_Class.Where(x => x.ClassTypeID == 3).Count();
            int society = db.C_Class.Where(x => x.ClassTypeID == 4).Count();
            int exercise = db.C_Class.Where(x => x.ClassTypeID == 5).Count();
            int gender = db.C_Class.Where(x => x.ClassTypeID == 6).Count();
            int game = db.C_Class.Where(x => x.ClassTypeID == 7).Count();
            int music = db.C_Class.Where(x => x.ClassTypeID == 8).Count();
            int program = db.C_Class.Where(x => x.ClassTypeID == 9).Count();
            int art = db.C_Class.Where(x => x.ClassTypeID == 10).Count();
            int design = db.C_Class.Where(x => x.ClassTypeID == 11).Count();
            int life = db.C_Class.Where(x => x.ClassTypeID == 12).Count();
            int littlething = db.C_Class.Where(x => x.ClassTypeID == 13).Count();
            int marketing = db.C_Class.Where(x => x.ClassTypeID == 14).Count();
            int others = db.C_Class.Where(x => x.ClassTypeID == 15).Count();
            ClassRatio obj2 = new ClassRatio();
            obj2.Language = language;
            obj2.Financial = financial;
            obj2.Chemistry = chemistry;
            obj2.Society = society;
            obj2.Exercise = exercise;
            obj2.Gender = gender;
            obj2.Game = game;
            obj2.Music = music;
            obj2.Program = program;
            obj2.Art = art;
            obj2.Design = design;
            obj2.Life = life;
            obj2.Littlething = littlething;
            obj2.Marketing = marketing;
            obj2.Others = others;
            return Json(obj2, JsonRequestBehavior.AllowGet);
        }
        public class ClassRatio
        {
            public int Language { get; set; }
            public int Financial { get; set; }
            public int Chemistry { get; set; }
            public int Society { get; set; }
            public int Exercise { get; set; }
            public int Gender { get; set; }
            public int Game { get; set; }
            public int Music { get; set; }
            public int Program { get; set; }
            public int Art { get; set; }
            public int Design { get; set; }
            public int Life { get; set; }
            public int Littlething { get; set; }
            public int Marketing { get; set; }
            public int Others { get; set; }
        }

        [HttpGet]
        public ActionResult EditSkill(int id = 0)
        {
            ViewBag.UID = id;
            ViewBag.SkillTable = db.U_Skill;
            ViewBag.SkillTypeTable = db.U_SkillType;
            if (Request.Cookies["UserID"].Value != id.ToString())
            {
                return Content("<script language='javascript' type='text/javascript'>alert('帳戶錯誤，請勿更改網址!');location.href='/User/Account';</script>");
            }

            else
            {
                return View(repositoryUS.GetById(id));
            }
        }

        [HttpPost]
        public ActionResult EditSkill(U_UserSkill US)
        {
            US.UserID = Convert.ToInt32(Request.Cookies["UserID"].Value);

            //ViewBag.SkillTable = db.U_Skill;
            //ViewBag.SkillTypeTable = db.U_SkillType;
            repositoryUS.Create(US);
            return RedirectToAction("Account", "User");
        }

        public ActionResult _EditSkill(int stid)
        {
            return PartialView(db.U_Skill.Where(c => c.SkillTypeID == stid));
        }
    }
}
