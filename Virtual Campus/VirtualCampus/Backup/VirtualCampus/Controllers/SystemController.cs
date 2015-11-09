using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using VirtualCampus.Models;
using System.Configuration;
using System.Text;
namespace VirtualCampus.Controllers
{
    public class SystemController : Controller
    {   
        private Virtual_CampusEntities dbobj = new Virtual_CampusEntities();
        public static string error;
        public ActionResult Index()
        {
            HttpContext.Session["name"] = "Prawal Sharma";
            @ViewBag.message = "You have reached the internal portal of the University. Please Sign In to proceed.";
            ViewBag.error = error ;
            return View();
        }
        public ActionResult loginuser(FormCollection logindetails) {
           string username = logindetails[0].ToString();
           string password = logindetails[1].ToString();
            var systemuser = dbobj.VC_User.Where(m => m.User_Name == username && m.User_Password == password).FirstOrDefault();
            if (systemuser != null)
            {
                int privilage = systemuser.User_Privilage;
                HttpContext.Session["userid"] = systemuser.User_Id;
                HttpContext.Session["privilage"] = privilage;
                var q = (from m in dbobj.VC_WorkspaceUser where m.User_Id == systemuser.User_Id select m.Workspace_Id);
                int i = 0;
                int length = q.Count();
                HttpContext.Session["wspacelength"] = length;
                string[] val = new string[length];
                foreach (var wrkspc in q)
                {
                    var q1 = (from m in dbobj.VC_Workspace where m.Workspace_Id == wrkspc select m).Single();
                    val[i] = q1.Workspace_Name.ToString();
                    HttpContext.Session["workspacelist"] = val;
                    i++;
                }
                if (privilage == 4)
                {
                    var student = (from m in dbobj.VC_Student where m.User_Id == systemuser.User_Id select m).FirstOrDefault();
                    HttpContext.Session["username"] = student.Student_Name;
                    HttpContext.Session["valid"] = true;
                }
                else { 
                    var staff = (from m in dbobj.VC_Staff where m.User_Id == systemuser.User_Id select m).FirstOrDefault();
                    HttpContext.Session["username"] = staff.Staff_Name;
                    HttpContext.Session["valid"] = true;
                }
            }
            else {
                error = "invalid userame or password";
                return RedirectToAction("index","system");
            }
            error = "";
            return RedirectToAction("index","home");
        }
        public ActionResult logoutuser() {
            HttpContext.Session["username"] = null;
            HttpContext.Session["privilage"] = null;
            HttpContext.Session["valid"] = false;
            HttpContext.Session["workspacelist"] = null ;
            HttpContext.Session["wspacelength"] = 0;
            return RedirectToAction("index","system");
        }
        public ActionResult Register1(int Privi, string EmailId, int Wid) 
        {
            @ViewBag.email = EmailId;
            @ViewBag.priv = Privi;
            @ViewBag.wkid = Wid;
            return View();
        }
        [HttpPost]
        public ActionResult Register1(FormCollection collection) 
        {
            VC_User userobj = new VC_User();
            userobj.User_Privilage = Convert.ToInt32(collection[0]);
            userobj.User_Name = collection[1].ToString().Trim();
            userobj.User_Password = collection[2].ToString().Trim();
            userobj.Created_TS = DateTime.Now;
            userobj.Updated_TS = DateTime.Now;
            dbobj.AddToVC_User(userobj);
            dbobj.SaveChanges();
            int inserted = userobj.User_Id;
            VC_WorkspaceUser wsobj = new VC_WorkspaceUser();
            wsobj.User_Id = inserted;
            wsobj.Workspace_Id = Convert.ToInt16(collection[4]);
            wsobj.Created_TS = DateTime.Now;
            wsobj.Updated_TS = DateTime.Now;
            dbobj.AddToVC_WorkspaceUser(wsobj);
            dbobj.SaveChanges();
            if (userobj.User_Privilage == 4)
            {
                return RedirectToAction("Student", new { k = inserted});
            }
            else {
                return RedirectToAction("RegisterStaff", new { k = inserted });
            }
        }
        public ActionResult RegisterStaff(int k)
        {
            ViewBag.userid = k;
            return View();
        }

        
        public ActionResult SaveRegisterStaff(VirtualCampus.Models.StaffModel model) {
            VC_Staff staffobj = new VC_Staff();
            staffobj.User_Id = Convert.ToInt32(model.User_Id);
            staffobj.Staff_Name = model.Staff_Name.ToString();
            staffobj.Staff_Gender = model.Staff_Gender.ToString();
            staffobj.Staff_College = model.Staff_College.ToString();
            staffobj.Staff_Department = model.Staff_Department.ToString();
            staffobj.Staff_Designation = model.Staff_Designation.ToString();
            staffobj.Staff_LibraryId = Convert.ToInt32(model.Staff_LibraryId);
            staffobj.Staff_ContactNo = Convert.ToInt32(model.Staff_ContactNo);
            staffobj.Staff_EmailId = model.Staff_EmailId.ToString();
            staffobj.Staff_Birthdate = Convert.ToDateTime(model.Staff_BirthDate);
            staffobj.Staff_Security_Que = model.Staff_Security_Ques.ToString();
            staffobj.Staff_Security_Ans = model.Staff_Security_Ans.ToString();
            staffobj.Created_TS = DateTime.Now;
            staffobj.Updated_TS = DateTime.Now;
            dbobj.AddToVC_Staff(staffobj);
            dbobj.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult Student(int k)
        {
            ViewBag.userid = k;
            return View();
        }

        public ActionResult SaveStudent(VirtualCampus.Models.StudentModel model)
        {
            VC_Student studentobj = new VC_Student();

            studentobj.User_Id = Convert.ToInt32(model.User_Id);
            studentobj.Student_Name = model.Student_Name.ToString();
            studentobj.Student_Department = model.Student_Department.ToString();
            studentobj.Student_College = model.Student_College.ToString();
            studentobj.Student_Gender = model.Student_Gender.ToString();
            studentobj.Student_EnrollYear = Convert.ToInt32(model.Student_EnrollYear);
            studentobj.Student_LibraryId = Convert.ToInt32(model.Student_LibraryId);
            studentobj.Student_EnrollmentNo = Convert.ToInt32(model.Student_EnrollmentNo);
            studentobj.Student_EmailId = model.Student_EmailId.ToString();
            studentobj.Student_ContactNo = Convert.ToInt32(model.Student_ContactNo);
            studentobj.Student_Birthdate = Convert.ToDateTime(model.Student_Birthdate);
            studentobj.Student_Hometown = model.Student_Hometown.ToString();
            studentobj.Student_Security_Que = model.Student_Security_Ques.ToString();
            studentobj.Student_Security_Ans = model.Student_Security_Ans.ToString();
            studentobj.Created_TS = DateTime.Now;
            studentobj.Updated_TS = DateTime.Now;

            dbobj.AddToVC_Student(studentobj);
            dbobj.SaveChanges();

            return RedirectToAction("index");

        }



        public ActionResult AddNewUser()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-select workspace-", Value = "default" });
            var q = (from m in dbobj.VC_Workspace select m).ToArray();
            for (int i = 0; i < q.Length; i++)
            {
                list.Add(new SelectListItem { Text = q[i].Workspace_Name, Value = q[i].Workspace_Id.ToString() });
            }
            ViewData["wname"] = list;
            return View();
        }


        [HttpPost]
        public ActionResult AddNewUser(VirtualCampus.Models.AddNewUser model)
        {
            
                MailMessage msg = new MailMessage();
                string ActivationUrl ;
                string emailId = string.Empty;
                string privilage = string.Empty;

                msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                emailId = model.EmailId.Trim();
                privilage = model.Privilage.ToString();
                Int32 workspace = Convert.ToInt32(model.Workspace);

                //sender email address
                msg.From = new MailAddress("admin@ronak-buildwell.com");



                //Receiver email address
                msg.To.Add(emailId);
                msg.Subject = "Confirmation email for account activation";
                //For testing replace the local host path with your lost host path and while making online replace with your website domain name
                // ActivationUrl = Server.HtmlEncode("http://localhost:15859/ActivateAccount.aspx?UserID=" + FetchUserId(emailId) + "&EmailId=" + emailId);
                ActivationUrl = HttpContext.Server.HtmlEncode("http://localhost:15859/System/Register1?Privi=" + privilage + "&EmailId=" + emailId + "&Wid=" + workspace);

                msg.Body = "Hi <strong><b>User</b></strong>!\n" +
                   "Thanks for showing interest and registring in VIRTUAL CAMPUS.....<br/>  " +
                   "<br/> Here this is the link with passing the attribute of different PRIVILAGE LEVEL as privi like 1,2,3,4 and EMAIL ID without storing the details in database... " +

                   " Please <a href='" + ActivationUrl + "'>click here to activate</a>  <br/>your account and enjoy our services. \nThanks! \n  CHEERS!!!!";
    
            msg.IsBodyHtml = true;
                //smtp.Credentials = new NetworkCredential("admin@ronak-buildwell.com", "escan123");
                //smtp.Port = 3535;
                //smtp.Host = "smtpout.secureserver.net";
                //smtp.EnableSsl = false;

                smtp.Credentials = new NetworkCredential("aarzoo93@gmail.com", "9824396591");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Send(msg);
                JavaScript("alert('Your mail has been sent.');");
             
            Response.Write("Emamil was sent");
            
            return RedirectToAction("AddNewUser");
        }
    }
}
