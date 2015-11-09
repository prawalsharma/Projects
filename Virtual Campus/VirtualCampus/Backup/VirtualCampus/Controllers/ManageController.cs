using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtualCampus.Models;

namespace VirtualCampus.Controllers
{
    public class ManageController : Controller
    {
        private Virtual_CampusEntities dbobj = new Virtual_CampusEntities();
        private Virtual_CampusEntities obj = new Virtual_CampusEntities();
        public List<SelectListItem> list;
        //    Virtual_CampusEntities obj = new Virtual_CampusEntities();

        public ActionResult Index()
        {
            ViewBag.name = "Prawal Sharma";
            ViewBag.id = "1";
            ViewBag.list = "Prawal Workspace";
            return View();
        }
        public ActionResult ViewNewsCategory()
        {
            ViewBag.name = "Prawal Sharma";
            ViewBag.id = "1";
            ViewBag.list = "Prawal Workspace";
            ViewBag.Categories = obj.VC_News_Category.ToList();

            return View();
        }
        public ActionResult AddNews()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Prawal Sharma";
            return View();
        }

        [HttpPost]
        public ActionResult AddNews(VirtualCampus.Models.NewsCategoryModel model)
        {
            try
            {
                VC_News_Category n = new VC_News_Category();
                n.Created_TS = DateTime.Now;
                n.Updated_TS = DateTime.Now;
                n.CategoryName = model.CategoryName.ToString();
                obj.AddToVC_News_Category(n);
                obj.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewNewsCategory", "Manage");
        }
        public ActionResult AddNewsSubCat()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Prawal Sharma";
            /*try
            {
                IEnumerable<SelectListItem> categories = obj.VC_News_Category.Select(c => new SelectListItem
                {
                    //Value = c.NewsCategory_Id.ToString(), 
                    Text = c.CategoryName
                });
                ViewData["Categories"] = categories;
                return View();
            }

            catch
            {
                return View();
            } */

            list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
            var cat = (from m in obj.VC_News_Category select m).ToArray();
            for (int i = 0; i < cat.Length; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = cat[i].CategoryName,
                    Value = cat[i].NewsCategory_Id.ToString(),
                    Selected = (cat[i].NewsCategory_Id == 0)
                });
            }
            ViewData["cat"] = list;
            return View();
        }

        [HttpPost]
        public ActionResult AddNewsSubCat(VirtualCampus.Models.NewsSubCategoryModel model)
        {


            VC_News_Link n = new VC_News_Link();
            n.News_Link = model.NewsLink.ToString();
            n.NewsCategory_Id = model.NewsCategory_Id;
            n.News_Description_Name = model.News_Description_Name.ToString();
            n.Created_TS = DateTime.Now;
            n.Updated_TS = DateTime.Now;
            obj.AddToVC_News_Link(n);
            obj.SaveChanges();
            //return View();

            /* list = new List<SelectListItem>();
             list.Add(new SelectListItem { Text = "-Please select-", Value = "Selects items" });
             var cat = (from m in obj.VC_News_Category select m).ToArray();
             for (int i = 0; i < cat.Length; i++)
             {
                 list.Add(new SelectListItem
                 {
                     Text = cat[i].CategoryName,
                     Value = cat[i].NewsCategory_Id.ToString(),
                     Selected = (cat[i].NewsCategory_Id == 0)
                 });
             }
             ViewData["cat"] = list; */
            //return View();

            return RedirectToAction("ViewNewsCategory");
        }

        public ActionResult ViewNews(int k)
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Ashmi Bhanushali";
            VC_News_Category newscat = new VC_News_Category();


            /* join query
             * 
             * var q = (from p in obj.VC_News_Category join a in obj.VC_News on p.NewsCategory_id equals a.NewsCategory_id where a.NewsCategory_id==k select new {p.CategoryName,a.});
             * 
             * */
            var n = (from a in obj.VC_News_Category where a.NewsCategory_Id == k select a.CategoryName);
            ViewBag.catname = n.SingleOrDefault();
            var q = (from m in obj.VC_News_Link where m.NewsCategory_Id == k select m);
            ViewBag.newslist = q;
            //ViewBag.newscatname = from(m in obj.VC_News_Category select m where)
            return View();
        }


        public ActionResult EditNewsCategory(int k)
        {
            var q = (from m in obj.VC_News_Category where m.NewsCategory_Id == k select m.NewsCategory_Id);
            var r = (from n in obj.VC_News_Category where n.NewsCategory_Id == k select n.CategoryName);
            ViewBag.catid = q.SingleOrDefault();
            ViewBag.editablecatname = r.SingleOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult EditNewscategory(int k, VirtualCampus.Models.NewsCategoryModel model)
        {
            try
            {
                VC_News_Category n = obj.VC_News_Category.Where(m => m.NewsCategory_Id == k).First();

                n.CategoryName = model.CategoryName.ToString();
                obj.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewNewsCategory", "Manage");
        }

        public ActionResult EditNewsSubCat(int k, int j)
        {
            var q = (from m in obj.VC_News_Category where m.NewsCategory_Id == j select m.CategoryName);
            ViewBag.editsub_catname = q.SingleOrDefault();
            var r = (from n in obj.VC_News_Link where n.News_Id == k select n).SingleOrDefault();
            ViewBag.edit_newslink = r.News_Link;
            ViewBag.edit_newsDescription = r.News_Description_Name;
            return View();
        }
        [HttpPost]
        public ActionResult EditNewsSubCat(int k, int j, VirtualCampus.Models.EditNewsSubCategoryModel model)
        {
            try
            {
                VC_News_Link n = obj.VC_News_Link.Where(m => m.News_Id == k).First();

                n.News_Link = model.NewsLink.ToString();
                n.News_Description_Name = model.News_Description_Name.ToString();
                obj.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewNews", "Manage", new { k = j });
        }

        public object DeleteNewsCategory(int k, VirtualCampus.Models.NewsCategoryModel model)
        {

            VC_News_Category n = obj.VC_News_Category.Where(m => m.NewsCategory_Id == k).First();
            //var q = (from m in obj.VC_News_Category where m.NewsCategory_Id == k select m);

            obj.DeleteObject(n);
            obj.SaveChanges();
            return RedirectToAction("ViewNewsCategory");
        }

        public object DeleteNewsSubCat(int k, VirtualCampus.Models.NewsSubCategoryModel model)
        {
            VC_News_Link n = obj.VC_News_Link.Where(m => m.News_Id == k).First();
            obj.DeleteObject(n);
            obj.SaveChanges();
            return RedirectToAction("ViewNewsCategory");
        }
        //Interest     
        public ActionResult AddInterestCategory()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Prawal Sharma";
            return View();

        }
        [HttpPost]
        public ActionResult AddInterestCategory(VirtualCampus.Models.InterestCategoryModel model)
        {
            try
            {
                VC_Interest_Category interest = new VC_Interest_Category();
                interest.Category_Name = model.Category_Name;
                interest.Created_TS = DateTime.Now;
                interest.Updated_TS = DateTime.Now;
                obj.AddToVC_Interest_Category(interest);
                obj.SaveChanges();

            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewInterestCategory", "Manage");
        }

        public ActionResult ViewInterestCategory()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Prawal Sharma";
            ViewBag.IntCat = obj.VC_Interest_Category.ToList();
            return View();


        }

        public ActionResult ViewInterestCategory1()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Prawal Sharma";
            ViewBag.IntCat = obj.VC_Interest_Category.ToList();
            return View();


        }

        public ActionResult EditInterestCategory(int k)
        {
            var q = (from m in obj.VC_Interest_Category where m.InterestCategory_Id == k select m.InterestCategory_Id);
            var r = (from m in obj.VC_Interest_Category where m.InterestCategory_Id == k select m.Category_Name);
            ViewBag.intCatId = q.SingleOrDefault();
            ViewBag.editableIntCatName = r.SingleOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult EditInterestcategory(int k, VirtualCampus.Models.InterestCategoryModel model)
        {
            try
            {
                VC_Interest_Category n = obj.VC_Interest_Category.Where(m => m.InterestCategory_Id == k).First();
                n.Category_Name = model.Category_Name.ToString();
                obj.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewInterestCategory", "Manage");
        }

        public object DeleteInterestCategory(int id, VirtualCampus.Models.InterestCategoryModel model)
        {

            VC_Interest_Category n = obj.VC_Interest_Category.Where(m => m.InterestCategory_Id == id).First();
            //var q = (from m in obj.VC_News_Category where m.NewsCategory_Id == k select m);

            obj.DeleteObject(n);
            obj.SaveChanges();
            return RedirectToAction("ViewInterestCategory", "Manage");
        }

        public ActionResult ViewInterest(int k)
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Ashmi Bhanushali";
            var n = (from a in obj.VC_Interest_Category where a.InterestCategory_Id == k select a.Category_Name);
            ViewBag.intCatName = n.SingleOrDefault();
            var q = (from m in obj.VC_Interest_Link where m.InterestCategory_Id == k select m);
            ViewBag.intList = q;
            //ViewBag.newscatname = from(m in obj.VC_News_Category select m where)
            return View();

        }

        public ActionResult AddIntSubCat()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Ashmi Bhanushali";
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Please Select-" });
            var cat = (from m in obj.VC_Interest_Category select m).ToArray();
            for (int i = 0; i < cat.Length; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = cat[i].Category_Name,
                    Value = cat[i].InterestCategory_Id.ToString(),
                    Selected = (cat[i].InterestCategory_Id == 0)
                });
            }
            ViewData["intCat"] = list;
            return View();
        }

        [HttpPost]
        public ActionResult AddIntSubCat(VirtualCampus.Models.InterestSubCatModel model)
        {

            VC_Interest_Link n = new VC_Interest_Link();
            n.Interest_Link = model.Interest.ToString();
            n.InterestCategory_Id = model.InterestCategory_Id;
            n.Interest_Description_Name = model.Interest_Description_Name.ToString();
            n.Created_TS = DateTime.Now;
            n.Updated_TS = DateTime.Now;
            obj.AddToVC_Interest_Link(n);
            obj.SaveChanges();

            return RedirectToAction("ViewInterestCategory");
        }

        public ActionResult EditIntSubCat(int k, int j)
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Ashmi Bhanushali";
            var q = (from m in obj.VC_Interest_Category where m.InterestCategory_Id == j select m.Category_Name);
            ViewBag.editsub_intCat = q.SingleOrDefault();
            var r = (from n in obj.VC_Interest_Link where n.Interest_Id == k select n).SingleOrDefault();
            ViewBag.edit_interestlink = r.Interest_Link;
            ViewBag.edit_interestDescription = r.Interest_Description_Name;

            return View();
        }

        [HttpPost]
        public ActionResult EditIntSubCat(int k, int j, VirtualCampus.Models.InterestSubCatModel model)
        {
            try
            {
                VC_Interest_Link n = obj.VC_Interest_Link.Where(m => m.Interest_Id == k).First();

                n.Interest_Link = model.Interest.ToString();
                n.Interest_Description_Name = model.Interest_Description_Name.ToString();
                obj.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewInterest", "Manage", new { k = j });
        }
        public object DeleteIntSubCat(int k)
        {
            VC_Interest_Link n = obj.VC_Interest_Link.Where(m => m.Interest_Id == k).First();

            obj.DeleteObject(n);
            obj.SaveChanges();
            return RedirectToAction("ViewInterestCategory", "Manage");

        }


        //Area of study

        public ActionResult AddAOSCategory()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Prawal Sharma";
            return View();

        }
        [HttpPost]
        public ActionResult AddAOSCategory(VirtualCampus.Models.AOSCategoryModel model)
        {
            try
            {
                VC_Areaofstudy_Category a = new VC_Areaofstudy_Category();
                a.Category_Name = model.category_Name;
                a.Created_TS = DateTime.Now;
                a.Updated_TS = DateTime.Now;

                obj.AddToVC_Areaofstudy_Category(a);
                obj.SaveChanges();

            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewAOSCategory", "Manage");
        }

        public ActionResult ViewAOSCategory()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Prawal Sharma";
            ViewBag.AOSCat = obj.VC_Areaofstudy_Category.ToList();
            return View();


        }


        public ActionResult EditAOSCategory(int k)
        {
            var q = (from m in obj.VC_Areaofstudy_Category where m.Areaofstudy_Category_Id == k select m.Areaofstudy_Category_Id);
            var r = (from m in obj.VC_Areaofstudy_Category where m.Areaofstudy_Category_Id == k select m.Category_Name);
            ViewBag.AOSCatId = q.SingleOrDefault();
            ViewBag.editableAOSCatName = r.SingleOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult EditAOSCategory(int k, VirtualCampus.Models.AOSCategoryModel model)
        {
            try
            {
                VC_Areaofstudy_Category a = obj.VC_Areaofstudy_Category.Where(m => m.Areaofstudy_Category_Id == k).First();
                a.Category_Name = model.category_Name.ToString();
                obj.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewAOSCategory", "Manage");
        }

        public object DeleteAOSCategory(int id, VirtualCampus.Models.AOSCategoryModel model)
        {

            VC_Areaofstudy_Category n = obj.VC_Areaofstudy_Category.Where(m => m.Areaofstudy_Category_Id == id).First();
            //var q = (from m in obj.VC_News_Category where m.NewsCategory_Id == k select m);

            obj.DeleteObject(n);
            obj.SaveChanges();
            return RedirectToAction("ViewAOSCategory", "Manage");
        }

        public ActionResult ViewAOS(int k)
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Ashmi Bhanushali";
            var n = (from a in obj.VC_Areaofstudy_Category where a.Areaofstudy_Category_Id == k select a.Category_Name);
            ViewBag.AOSCatName = n.SingleOrDefault();
            var q = (from m in obj.VC_Areaofstudy where m.Areaofstudy_Category_Id == k select m);
            ViewBag.AOSList = q;
            //ViewBag.newscatname = from(m in obj.VC_News_Category select m where)
            return View();

        }

        public ActionResult AddAOS()
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Ashmi Bhanushali";
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Please Select-" });
            var cat = (from m in obj.VC_Areaofstudy_Category select m).ToArray();
            for (int i = 0; i < cat.Length; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = cat[i].Category_Name,
                    Value = cat[i].Areaofstudy_Category_Id.ToString(),
                    Selected = (cat[i].Areaofstudy_Category_Id == 0)
                });
            }
            ViewData["AOSCat"] = list;
            return View();
        }

        [HttpPost]
        public ActionResult AddAOS(VirtualCampus.Models.AOSSubCatModel model)
        {

            VC_Areaofstudy n = new VC_Areaofstudy();
            n.Areaofstudy = model.Areaofstudy.ToString();
            n.Areaofstudy_Category_Id = model.Areaofstudy_Category_Id;

            n.Created_TS = DateTime.Now;
            n.Updated_TS = DateTime.Now;
            obj.AddToVC_Areaofstudy(n);
            obj.SaveChanges();

            return RedirectToAction("ViewAOSCategory");
        }

        public ActionResult EditAOS(int k, int j)
        {
            @ViewBag.id = "1";
            @ViewBag.name = "Ashmi Bhanushali";
            var q = (from m in obj.VC_Areaofstudy_Category where m.Areaofstudy_Category_Id == j select m.Category_Name);
            ViewBag.editsub_AOSCat = q.SingleOrDefault();
            var r = (from n in obj.VC_Areaofstudy where n.Areaofstudy_Id == k select n).SingleOrDefault();
            ViewBag.edit_AOS = r.Areaofstudy;

            return View();
        }

        [HttpPost]
        public ActionResult EditAOS(int k, int j, VirtualCampus.Models.AOSSubCatModel model)
        {
            try
            {
                VC_Areaofstudy n = obj.VC_Areaofstudy.Where(m => m.Areaofstudy_Id == k).First();

                n.Areaofstudy = model.Areaofstudy.ToString();

                obj.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("ViewAOS", "Manage", new { k = j });
        }

        public object DeleteAOS(int k)
        {
            VC_Areaofstudy n = obj.VC_Areaofstudy.Where(m => m.Areaofstudy_Id == k).First();

            obj.DeleteObject(n);
            obj.SaveChanges();
            return RedirectToAction("ViewAOSCategory", "Manage");

        }
    
    }
}
