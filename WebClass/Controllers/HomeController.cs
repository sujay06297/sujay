using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClass.Models;
//using WebClass.Servce;
using WebClass.Service;
using WebClass.ViewModel;

namespace WebClass.Controllers
{
    public class HomeController : Controller
    {
        AOnlineClassEntities db = new AOnlineClassEntities();
        ShoppingCarService car = new ShoppingCarService();
        OrderService OS = new OrderService();
        HomeService HS = new HomeService();
        SmallBellService SB = new SmallBellService();

        public ActionResult Index()/*首頁*/
        {
            ViewBag.NAV = db.C_ClassType;
            return View(HS);
        }

        public ActionResult About()/*之後做*/
        {
            return View();
        }

        public ActionResult Browse(int id = 0, string ClassName = "")
        {
            ViewBag.typename = db.C_ClassType.Where(c => c.ClassTypeID == id).Select(c => c.ClassTypeName).FirstOrDefault();
            ViewBag.type = id;
            ViewBag.search = ClassName;
            return View();
        }

        public ActionResult _ClassBrowse(int id = 0, string ClassName = "", int? page = 1, string SortType = "")
        {

            ViewBag.SortType = SortType;

            var ClassCount = db.C_Class.Where(c => c.CreateTypeID == 1 || c.CreateTypeID == 2).OrderByDescending(t => t.RegisterTime).Count();
            IQueryable<C_Class> _class = db.C_Class.Where(c => c.CreateTypeID == 1 || c.CreateTypeID == 2).OrderByDescending(t => t.RegisterTime);
            int NowPageCount;
            int PageSize = 8;


            if (ClassName != "")
            {
                _class = db.C_Class.Where(c => c.ClassName.Contains(ClassName));
                ClassCount = _class.Count();
                NowPageCount = ((ClassCount + PageSize) - 1) / PageSize;
                ViewBag.pagecount = NowPageCount;

                List<C_Class> sereach = _class.OrderBy(o => o.ClassName).Skip(page.Value * PageSize - PageSize).Take(PageSize).ToList();
                return PartialView(sereach);
            }

            if (SortType == "HotClass")
            {
                var order = db.B_OrderDetail.GroupBy(g => g.ClassID).OrderByDescending(L => L.Count()).Select(L => L.FirstOrDefault().C_Class);
                ClassCount = order.Count();
                List<C_Class> c_Classes = order.Skip((page.Value * PageSize) - PageSize).Take(PageSize).ToList();
                NowPageCount = ((ClassCount + PageSize) - 1) / PageSize;
                ViewBag.pagecount = NowPageCount;

                return PartialView(c_Classes);
            }

            if (SortType == "HighestRating")
            {
                var timeclass = db.C_Class.Where(c => c.CreateTypeID == 1 || c.CreateTypeID == 2).OrderByDescending(L => L.UC_Score.Average(s => s.Score));
                ClassCount = timeclass.Count();


                List<C_Class> c_Classes = timeclass.Skip((page.Value * PageSize) - PageSize).Take(PageSize).ToList();

                NowPageCount = ((ClassCount + PageSize) - 1) / PageSize;
                ViewBag.pagecount = NowPageCount;

                return PartialView(c_Classes);
            }


            if (SortType == "NewClass" || id == 0)
            {
                var timeclass = db.C_Class.Where(c => c.CreateTypeID == 1 || c.CreateTypeID == 2).OrderByDescending(t => t.RegisterTime);
                ClassCount = timeclass.Count();

                List<C_Class> c_Classes = timeclass.Skip((page.Value * PageSize) - PageSize).Take(PageSize).ToList();

                NowPageCount = ((ClassCount + PageSize) - 1) / PageSize;
                ViewBag.pagecount = NowPageCount;

                return PartialView(c_Classes);
            }


            if (id != 0)
            {
                _class = db.C_Class.Where(c => c.ClassTypeID == id && (c.CreateTypeID == 1 || c.CreateTypeID == 2));
                ClassCount = _class.Count();



                NowPageCount = ((ClassCount + PageSize) - 1) / PageSize;
                ViewBag.pagecount = NowPageCount;

                List<C_Class> c_Classes = _class.OrderBy(o => o.ClassName).Skip(page.Value * PageSize - PageSize).Take(PageSize).ToList();
                return PartialView(c_Classes);
            }

            






            NowPageCount = ((ClassCount + PageSize) - 1) / PageSize;
            ViewBag.pagecount = NowPageCount;

            List<C_Class> result = _class.OrderBy(o => o.ClassName).Skip(page.Value * PageSize - PageSize).Take(PageSize).ToList();

            return PartialView(result);
        }



        public ActionResult Nav()
        {
            ViewBag.X = db.C_Class.Where(c => c.CreateTypeID == 1 || c.CreateTypeID == 2).Count();
            return PartialView(db.C_ClassType.ToList());
        }

        public ActionResult _Cart()
        {
            if (Request.Cookies["UserID"] != null)
            {
                int f = Convert.ToInt32(Request.Cookies["UserID"].Value);
                List<B_ShoppingCar> sc = db.B_ShoppingCar.Where(id => id.UserID == f).ToList();

                return PartialView(sc);
            }
            else
            {
                return Content("");
            }
        }

        public ActionResult CreateClassCheck()
        {
            if (Request.Cookies["UserID"] == null)
            {
                Session["path"] = Request.Url.AbsolutePath;
                return RedirectToAction("Login", "User");
            }
            return View(Convert.ToInt32(Request.Cookies["UserID"].Value));
        }

        public ActionResult Search(string ClassName, string UserName, string ClassTypeName, DateTime? RegisterTimeMin, DateTime? RegisterTimeMax)
        {
            if (RegisterTimeMin == null)
            {
                RegisterTimeMin = DateTime.Now.AddYears(-100);
            }
            if (RegisterTimeMax == null)
            {
                RegisterTimeMax = DateTime.Now.AddYears(100);
            }

            var v = (from f in db.C_ClassType
                     select f.ClassTypeName).ToList();
            ViewBag.List = v;

            return View(db.C_Class.Where(x => x.ClassName.Contains(ClassName) && x.CreateTypeID < 3 && x.U_User.UserName.Contains(UserName) && x.C_ClassType.ClassTypeName.Contains(ClassTypeName) && x.RegisterTime >= RegisterTimeMin && x.RegisterTime <= RegisterTimeMax).ToList());
        }

        public ActionResult SmallBell()
        {
            ViewBag.Class = db.C_Class;
            return View();
        }

        public ActionResult AddClassNotice(int id = 0, string content = "")
        {
            SB.AddClassNotice(id, content);
            return Content("喵");
        }

        public ActionResult AddMessageNotice(int id = 0, string content = "")
        {
            SB.AddMessageNotice(id, content);
            return Content("喵");
        }


        public ActionResult AddToSmallBell(int id = 0, string content = "")
        {
            SB.AddSmallBell(id, content);
            return Content("喵");
        }

        public ActionResult RecommendClass()
        {
            SB.RecommendClass();
            return Content("喵");
        }


        public ActionResult _SmallBell(bool read = false)
        {
            int cookiesid = Convert.ToInt32(Request.Cookies["UserID"].Value);

            if (Request.Cookies["UserID"] != null)
            {
                if (read == true)
                {
                    SB.SmallBellRead(cookiesid);
                    List<U_SmallBell> SmallBellss = db.U_SmallBell.Where(i => i.UserID == cookiesid).OrderByDescending(t => t.CreatedDate).ToList();
                    return PartialView(SmallBellss);
                }

                List<U_SmallBell> SmallBells = db.U_SmallBell.Where(i => i.UserID == cookiesid).OrderByDescending(t => t.CreatedDate).ToList();
                return PartialView(SmallBells);

            }
            return Content("");
        }

        public ActionResult GetImage(int id)
        {
            var X = db.U_User.Find(id);
            byte[] img = X.UserPhoto;
            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        public ActionResult TopTeacherDetail(int? id)
        {
            List<TeachingVM> list = (from f in db.C_Class
                                     where f.CreateUserID == id && f.CreateTypeID < 3
                                     select new TeachingVM
                                     {
                                         ClassID = f.ClassID,
                                         ClassName = f.ClassName,
                                         Price = f.Price,
                                         RegisterTime = f.RegisterTime,
                                         CreateType = f.C_CreateType.CreateTypeName

                                     }).ToList();

            return View(list);
        }
    }
}
