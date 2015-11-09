using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtualCampus.Models;


namespace VirtualCampus.Controllers
{
    public class WorkspaceController : Controller
    {
        Virtual_CampusEntities dbobj = new Virtual_CampusEntities();
        //General Methods
        public ActionResult findapplication(string workspacename, string from) {
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
        public ActionResult Index(string k)
        {
            string frommethod = "Index";
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
            findapplication(k,frommethod);
            finduser(k,frommethod);
            return View();
        }
        public ActionResult AddUser(string workspace) {
            string frommethod = "AddUser";
            ViewBag.pagename = "Add User to: " + workspace;
            ViewBag.resultmessage = "";
            ViewBag.workspacename = workspace;
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
            findapplication(workspace,frommethod);
            finduser(workspace, frommethod);
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(FormCollection collection) {
            string workspace = collection[0].ToString();
            string frommethod = "AddUser";
            ViewBag.resultmessage = "Search has no results. Try with proper name of the user.";
            ViewBag.pagename = "Add User to: " + workspace;
            ViewBag.workspacename = workspace;
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
            findapplication(workspace, frommethod);
            finduser(workspace, frommethod);
            
            string name = collection[1].ToString();
            int index = 0;
            var staffresult = (from m in dbobj.VC_Staff where m.Staff_Name == name select m);
            var studentresult = (from m in dbobj.VC_Student where m.Student_Name == name select m);
            string[] resultname = new string[staffresult.Count()+studentresult.Count()];
            int[] resultid = new int[staffresult.Count()+studentresult.Count()];
            ViewBag.searchcount = staffresult.Count() + studentresult.Count();
            if (staffresult == null && studentresult == null)
            {
                ViewData["searchresult"] = "There are no users found.";
            }
            else { 
                foreach(var staff in staffresult){
                    resultname[index]=staff.Staff_Name;
                    resultid[index] = staff.User_Id;
                    ViewData["searchresult"] = resultname;
                    ViewData["searchid"] = resultid;
                    index++;
                }
                foreach (var student in studentresult) {
                    resultname[index] = student.Student_Name;
                    resultid[index] = student.User_Id;
                    ViewData["searchresult"] = resultname;
                    ViewData["searchid"] = resultid;
                    index++;
                }
            }
            return View();
        }
        public ActionResult usertoworkspace(int id, string workspace) {
            VC_WorkspaceUser tableobj = new VC_WorkspaceUser();
            var workspacedetails = (from m in dbobj.VC_Workspace where m.Workspace_Name == workspace select m).FirstOrDefault();
            int workspaceid = workspacedetails.Workspace_Id;
            tableobj.Workspace_Id = workspaceid;
            tableobj.User_Id = id;
            tableobj.Created_TS = DateTime.Now;
            tableobj.Updated_TS = DateTime.Now;
            dbobj.AddToVC_WorkspaceUser(tableobj);
            dbobj.SaveChanges();
            return RedirectToAction("AddUser", new { workspace = workspace });
        }
        public ActionResult AddWorkspace()
        {
            ViewBag.name = HttpContext.Session["username"];
            ViewBag.privilage = HttpContext.Session["privilage"];
            ViewBag.userid = HttpContext.Session["userid"];
            int i = 0;
            string[] arr = new string[Convert.ToInt32(HttpContext.Session["wspacelength"])];
            foreach (string a in HttpContext.Session["workspacelist"] as string[])
            {
                arr[i] = a;
                ViewData["userwspace"] = arr;
                i++;
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddWorkspace(FormCollection collection) {
            VC_Workspace wspobj = new VC_Workspace();
            VC_WorkspaceUser wsuser = new VC_WorkspaceUser();
            wspobj.Owner_User_Id = Convert.ToInt32(collection[0]);
            wspobj.Workspace_Name = collection[1].ToString();
            wspobj.Created_TS = DateTime.Now;
            wspobj.Updated_TS = DateTime.Now;
            dbobj.AddToVC_Workspace(wspobj);
            dbobj.SaveChanges();
            int inserted = wspobj.Workspace_Id;
            wsuser.Workspace_Id = inserted;
            wsuser.User_Id = Convert.ToInt32(HttpContext.Session["userid"]);
            wsuser.Created_TS = DateTime.Now;
            wsuser.Updated_TS = DateTime.Now;
            dbobj.AddToVC_WorkspaceUser(wsuser);
            dbobj.SaveChanges();
            return RedirectToAction("AddWorkspace");
        }
    }
}
