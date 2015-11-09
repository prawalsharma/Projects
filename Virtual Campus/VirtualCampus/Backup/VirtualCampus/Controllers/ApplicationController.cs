using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtualCampus.Models;

namespace VirtualCampus.Controllers
{
    public class ApplicationController : Controller
    {
        Virtual_CampusEntities dbobj = new Virtual_CampusEntities();
        //General Methods
        public ActionResult findapplication(string workspacename, string from)
        {
            var workspacedetails = (from m in dbobj.VC_Workspace where m.Workspace_Name == workspacename select m).FirstOrDefault();
            var applicationdetails = (from m in dbobj.VC_Application where m.Workspace_Id == workspacedetails.Workspace_Id select m);
            int j = 0;
            if (applicationdetails != null)
            {
                string[] appname = new string[applicationdetails.Count()];
                Int32[] appid = new Int32[applicationdetails.Count()];
                foreach (var apname in applicationdetails)
                {
                    appname[j] = apname.Application_Name;
                    appid[j] = apname.Application_Id;
                    j++;
                }
                ViewBag.noapp = applicationdetails.Count();
                ViewBag.appname = appname;
                ViewBag.appid = appid;
            }
            return View(from);
        }
        public ActionResult finduser(string workspacename, string from)
        {
            var workspacedetails = (from m in dbobj.VC_Workspace where m.Workspace_Name == workspacename select m).FirstOrDefault();
            var workspaceuserdetails = (from n in dbobj.VC_WorkspaceUser where n.Workspace_Id == workspacedetails.Workspace_Id select n.User_Id);
            if (workspaceuserdetails != null)
            {
                int index = 0;
                ViewBag.usercount = workspaceuserdetails.Count();
                string[] workspaceusername = new string[workspaceuserdetails.Count()];
                int[] workspaceuserid = new int[workspaceuserdetails.Count()];
                foreach (var userid in workspaceuserdetails)
                {
                    var value = (from l in dbobj.VC_User where l.User_Id == userid select l.User_Privilage);
                    foreach (int p in value)
                    {
                        if (p == 4)
                        {
                            var fromstudent = (from m in dbobj.VC_Student where m.User_Id == userid select m.Student_Name).SingleOrDefault();
                            workspaceusername[index] = fromstudent;
                            workspaceuserid[index] = userid;
                            ViewData["workspaceusername"] = workspaceusername;
                            ViewData["userid"] = workspaceuserid;
                            index++;
                        }
                        else
                        {
                            var fromstaff = (from n in dbobj.VC_Staff where n.User_Id == userid select n.Staff_Name).SingleOrDefault();
                            workspaceusername[index] = fromstaff;
                            workspaceuserid[index] = userid;
                            ViewData["workspaceusername"] = workspaceusername;
                            ViewData["userid"] = workspaceuserid;
                            index++;
                        }
                    }

                }
            }
            return View(from);
        }
        //Page methods
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult AddApplication(string k) {
            string frommethod = "AddApplication";
            ViewBag.pagename = "Workspace: " + k;
            ViewBag.workspacename = k;
            ViewBag.privilage = HttpContext.Session["privilage"];
            ViewBag.name = HttpContext.Session["username"];
            int i = 0;
            string[] wsl = new string[Convert.ToInt32(HttpContext.Session["wspacelength"])];
            foreach (string a in HttpContext.Session["workspacelist"] as string[])
            {
                wsl[i] = a;
                ViewData["userwspace"] = wsl;
                i++;
            }
            findapplication(k, frommethod);
            finduser(k, frommethod);
            return View();
        }
        [HttpPost]
        public ActionResult AddApplication(string k,VirtualCampus.Models.Application model) {
            VC_Application app = new VC_Application();
            Int16 userid = Convert.ToInt16(HttpContext.Session["userid"]);
            var workspaceuser = (from m in dbobj.VC_WorkspaceUser where m.User_Id == userid select m).FirstOrDefault();
            var workspacedetails = (from m in dbobj.VC_Workspace where m.Workspace_Name == k select m).FirstOrDefault();
            app.Workspace_Id = workspacedetails.Workspace_Id;
            app.WorkspaceUser_Id = workspaceuser.WorkspaceUser_Id;
            app.Application_Name = model.Application_Name.ToString();
            app.TextLabel_1 = model.TextLabel_1.ToString();
            if (model.TextLabel_2 != null)
            {
                app.TextLabel_2 = model.TextLabel_2.ToString();
            }
            else {
                app.TextLabel_2 = "empty";
            }
            if (model.TextLabel_3 != null)
            {
                app.TextLabel_3 = model.TextLabel_3.ToString();
            }
            else
            {
                app.TextLabel_3 = "empty";
            }
            if (model.TextLabel_4 != null)
            {
                app.TextLabel_4 = model.TextLabel_4.ToString();
            }
            else
            {
                app.TextLabel_4 = "empty";
            }
            if (model.TextLabel_5 != null)
            {
                app.TextLabel_5 = model.TextLabel_5.ToString();
            }
            else
            {
                app.TextLabel_5 = "empty";
            }
            if (model.TextArea_1 != null)
            {
                app.TextArea_1 = model.TextArea_1.ToString();
            }
            else
            {
                app.TextArea_1 = "empty";
            }
            if (model.TextArea_2 != null)
            {
                app.TextArea_2 = model.TextArea_2.ToString();
            }
            else
            {
                app.TextArea_2 = "empty";
            }
            if (model.TextArea_3 != null)
            {
                app.TextArea_3 = model.TextArea_3.ToString();
            }
            else
            {
                app.TextArea_3 = "empty";
            }
            if (model.DateLabel_1 != null)
            {
                app.DateLabel_1 = model.DateLabel_1.ToString();
            }
            else {
                app.DateLabel_1 = "empty";
            }
            app.Created_TS = DateTime.Now;
            app.Updated_TS = DateTime.Now;
            dbobj.AddToVC_Application(app);
            dbobj.SaveChanges();
            return RedirectToAction("Index", "Workspace", new { k = k});
        }
        public ActionResult ViewApplication(string k, int id) {
            string frommethod = "ViewApplication";
            ViewBag.applicationid = id;
            ViewBag.pagename = "Workspace: " + k;
            ViewBag.workspacename = k;
            ViewBag.privilage = HttpContext.Session["privilage"];
            ViewBag.name = HttpContext.Session["username"];
            int i = 0;
            string[] wsl = new string[Convert.ToInt32(HttpContext.Session["wspacelength"])];
            foreach (string a in HttpContext.Session["workspacelist"] as string[])
            {
                wsl[i] = a;
                ViewData["userwspace"] = wsl;
                i++;
            }
            findapplication(k, frommethod);
            finduser(k, frommethod);
            
            var applicationdetails = (from m in dbobj.VC_Application where m.Application_Id == id select m).SingleOrDefault();
            ViewBag.applicationname = applicationdetails.Application_Name;

            var postdetails = (from p in dbobj.VC_Post where p.Application_Id == id select p);
            string[] posttb = new string[postdetails.Count()];
            string[] postta = new string[postdetails.Count()];
            string[] postusername = new string[postdetails.Count()];
            Int32[] postid = new Int32[postdetails.Count()];
            ViewBag.postno = postdetails.Count();
            int index = 0, index1 = 0, index3 = 0, index4 = 0;
            DateTime[] postdate = new DateTime[postdetails.Count()];
            foreach (var datapost in postdetails) {
                postid[index4] = datapost.Post_Id;
                ViewData["postid"] = postid;
                index4++;
                posttb[index] = datapost.TextField_1;
                ViewData["tbpost"] = posttb;
                index++;
                postta[index1] = datapost.TextArea_1;
                ViewData["tapost"] = postta;
                index1++;
                var wsuser = (from s in dbobj.VC_WorkspaceUser where s.WorkspaceUser_Id == datapost.WorkspaceUser_Id select s.User_Id);
                foreach (var uid in wsuser) { 
                    var userprevilige = (from z in dbobj.VC_User where z.User_Id == uid select z.User_Privilage);
                    foreach (var p in userprevilige) {
                        if (p == 4)
                        {
                            string name = (from x in dbobj.VC_Student where x.User_Id == uid select x.Student_Name).SingleOrDefault();
                            postusername[index3] = name;
                            ViewData["postername"] = postusername ;
                            index3++;
                        }
                        else {
                            string name = (from x in dbobj.VC_Staff where x.User_Id == uid select x.Staff_Name).SingleOrDefault();
                            postusername[index3] = name;
                            ViewData["postername"] = postusername;
                            index3++;
                        }
                    }
                }
            }
            return View();
        }
        public ActionResult AddPost(string k, int id) {
            string frommethod = "AddPost";
            ViewBag.pagename = "Workspace: " + k;
            ViewBag.workspacename = k;
            ViewBag.privilage = HttpContext.Session["privilage"];
            ViewBag.name = HttpContext.Session["username"];
            int i = 0;
            string[] wsl = new string[Convert.ToInt32(HttpContext.Session["wspacelength"])];
            foreach (string a in HttpContext.Session["workspacelist"] as string[])
            {
                wsl[i] = a;
                ViewData["userwspace"] = wsl;
                i++;
            }
            findapplication(k, frommethod);
            finduser(k, frommethod);

            int counttb = 0,countta = 0, countdate = 0; 
            var applicationdetails = (from m in dbobj.VC_Application where m.Application_Id == id select m).SingleOrDefault();
            ViewBag.applicationname = applicationdetails.Application_Name;
            string[] tb = new string[5];
            string[] ta = new string[3];
            string[] date1 = new string[1]; 
            if (applicationdetails.TextLabel_1.ToString() != "empty")
            {
                tb[0] = applicationdetails.TextLabel_1.ToString();
                counttb++;
            }
            if (applicationdetails.TextLabel_2.ToString() != "empty")
            {
                tb[1] = applicationdetails.TextLabel_2.ToString();
                counttb++;
            }
            if (applicationdetails.TextLabel_3.ToString() != "empty")
            {
                tb[2] = applicationdetails.TextLabel_3.ToString();
                counttb++;
            }
            if (applicationdetails.TextLabel_4.ToString() != "empty")
            {
                tb[3] = applicationdetails.TextLabel_4.ToString();
                counttb++;
            }
            if (applicationdetails.TextLabel_5.ToString() != "empty")
            {
                tb[4] = applicationdetails.TextLabel_5.ToString();
                counttb++;
            }
            if (applicationdetails.TextArea_1.ToString() != "empty")
            {
                ta[0] = applicationdetails.TextArea_1.ToString();
                countta++;
            }
            if (applicationdetails.TextArea_2.ToString() != "empty")
            {
                ta[1] = applicationdetails.TextArea_2.ToString();
                countta++;
            }
            if (applicationdetails.TextArea_3.ToString() != "empty")
            {
                ta[2] = applicationdetails.TextArea_3.ToString();
                countta++;
            }
            if (applicationdetails.DateLabel_1.ToString() != "empty")
            {
                ++countdate;
                date1[0] = applicationdetails.DateLabel_1.ToString();
            }
            string[] textboxnames = new string[counttb];
            string[] textareanames = new string[countta];
            string[] datenames = new string[countdate];
            if(counttb != 0){
                for (int index = 0; index < counttb; index++) {
                    textboxnames[index] = tb[index];
                    ViewData["textboxnames"] = textboxnames;
                }
                ViewBag.textboxes = counttb;
            }
            else{
                ViewBag.textboxes = 0;
            }
            if (countta != 0)
            {
                for (int index2 = 0; index2 < countta; index2++)
                {
                    textareanames[index2] = ta[index2];
                    ViewData["textareanames"] = textareanames;
                }
                ViewBag.textareas = countta;
            }
            else {
                ViewBag.textareas = 0;
            }
            if (countdate != 0)
            {
                for (int index3 = 0; index3 < countdate; index3++)
                {
                    datenames[index3] = date1[index3];
                    ViewData["datename"] = datenames;
                }
                ViewBag.dates = countdate;
            }
            else {
                ViewBag.dates = 0;
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddPost(string k, int id, FormCollection data)
        {
            VC_Post objpost = new VC_Post();
            Int16 userid = Convert.ToInt16(HttpContext.Session["userid"]);
            var workspaceuser = (from m in dbobj.VC_WorkspaceUser where m.User_Id == userid select m).FirstOrDefault();
            int tbs = Convert.ToInt16(data[0]);
            int tas = Convert.ToInt16(data[1]);
            int dates = Convert.ToInt16(data[2]);
            objpost.Application_Id = id;
            objpost.WorkspaceUser_Id = workspaceuser.WorkspaceUser_Id;
            if (data[3].ToString() != "0")
            {
                objpost.TextField_1 = data[3].ToString();
            }
            else {
                objpost.TextField_1 = Convert.ToString("empty");
            }
            if (data[4].ToString() != "0")
            {
                objpost.TextField_2 = data[4].ToString();
            }
            else
            {
                objpost.TextField_2 = Convert.ToString("empty");
            }
            if (data[5].ToString() != "0")
            {
                objpost.TextField_3 = data[5].ToString();
            }
            else
            {
                objpost.TextField_3 = Convert.ToString("empty");
            }
            if (data[6].ToString() != "0")
            {
                objpost.TextField_4 = data[6].ToString();
            }
            else
            {
                objpost.TextField_4 = Convert.ToString("empty");
            }
            if (data[7].ToString() != "0")
            {
                objpost.TextField_5 = data[7].ToString();
            }
            else
            {
                objpost.TextField_5 = Convert.ToString("empty");
            }
            if (data[8].ToString() != "0")
            {
                objpost.TextArea_1 = data[8].ToString();
            }
            else
            {
                objpost.TextArea_1 = Convert.ToString("empty");
            }
            if (data[9].ToString() != "0")
            {
                objpost.TextArea_2 = data[9].ToString();
            }
            else
            {
                objpost.TextArea_2 = Convert.ToString("empty");
            }
            if (data[10].ToString() != "0")
            {
                objpost.TextArea_3 = data[10].ToString();
            }
            else
            {
                objpost.TextArea_3 = Convert.ToString("empty");
            }
            if (data[11].ToString() != "0")
            {
                objpost.Post_Date = Convert.ToDateTime(data[11]);
            }
            else
            {
                objpost.Post_Date = Convert.ToDateTime(04/04/1991);
            }
            objpost.Updated_TS = DateTime.Now;
            objpost.Created_TS = DateTime.Now;
            dbobj.AddToVC_Post(objpost);
            dbobj.SaveChanges();
            return RedirectToAction("viewapplication", new { k = k, id = id});
        }
        public ActionResult ViewPost(string k, int id, int pid) {
            string frommethod = "ViewPost";
            ViewBag.applicationid = id;
            ViewBag.pagename = "Workspace: " + k;
            ViewBag.workspacename = k;
            ViewBag.privilage = HttpContext.Session["privilage"];
            ViewBag.name = HttpContext.Session["username"];
            int i = 0;
            string[] wsl = new string[Convert.ToInt32(HttpContext.Session["wspacelength"])];
            foreach (string a in HttpContext.Session["workspacelist"] as string[])
            {
                wsl[i] = a;
                ViewData["userwspace"] = wsl;
                i++;
            }
            findapplication(k, frommethod);
            finduser(k, frommethod);
            var applicationdetails = (from m in dbobj.VC_Application where m.Application_Id == id select m).SingleOrDefault();
            ViewBag.applicationname = applicationdetails.Application_Name;


            var postdetails = (from p in dbobj.VC_Post where p.Post_Id == pid select p).SingleOrDefault();
            
            var wsuser = (from s in dbobj.VC_WorkspaceUser where s.WorkspaceUser_Id == postdetails.WorkspaceUser_Id select s.User_Id).SingleOrDefault();
            var user = (from x in dbobj.VC_User where x.User_Id == wsuser select x).SingleOrDefault();
            if(user.User_Privilage == 4){
                var name = (from m in dbobj.VC_Student where m.User_Id == user.User_Id select m).SingleOrDefault();
                ViewBag.postername = name.Student_Name.ToString();
            }else{
                var name = (from m in dbobj.VC_Staff where m.User_Id == user.User_Id select m).SingleOrDefault();
                ViewBag.postername = name.Staff_Name.ToString();
            }
            return View();
        } 
    }
}
