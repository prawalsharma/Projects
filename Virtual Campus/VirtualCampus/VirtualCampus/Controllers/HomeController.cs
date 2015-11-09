using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VirtualCampus.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            ViewBag.privilage = HttpContext.Session["privilage"];
            ViewBag.name = HttpContext.Session["username"];
            int i = 0;
            string[] arr = new string[Convert.ToInt32(HttpContext.Session["wspacelength"])];
            foreach(string a in HttpContext.Session["workspacelist"] as string[]){
                arr[i] = a;
                ViewData["userwspace"] = arr;
                i++;
            }
                return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
