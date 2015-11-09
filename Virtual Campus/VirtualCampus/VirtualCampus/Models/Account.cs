using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

using System.Data.Entity;
namespace VirtualCampus.Models
{
    public class UsersModel {
        public int User_Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string User_Name { get; set; }
        public string User_Privilage { get; set; }

        public int Workspace_id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string User_Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [CompareAttribute("User_Password", ErrorMessage = "Emails mismatch")]
        public string ConfirmPassword { get; set; }

    }
    public class StudentModel {
        public int Student_Id { get; set; }
        public int User_Id { get; set; }
        public string Student_Name { get; set; }
        public string Student_Department { get; set; }
        public string Student_College { get; set; }
        public string Student_Gender { get; set; }
        public int Student_EnrollYear { get; set; }
        public int Student_LibraryId { get; set; }
        public int Student_EnrollmentNo { get; set; }
        public string Student_EmailId { get; set; }
        public string Student_ContactNo { get; set; }
        public string Student_Birthdate { get; set; }
        public string Student_Hometown { get; set; }
        public string Student_Security_Ques { get; set; }
        public string Student_Security_Ans { get; set; }
    }
    public class StaffModel {
        public int Staff_Id { get; set; }
        public int User_Id { get; set; }
        public string Staff_Name { get; set; }
        public string Staff_Department { get; set; }
        public string Staff_College { get; set; }
        public string Staff_Gender { get; set; } 
        public string Staff_LibraryId { get; set; }
        public string Staff_Designation { get; set; }
        public string Staff_ContactNo { get; set; }
        public string Staff_BirthDate { get; set; }
        public string Staff_EmailId { get; set; }
        public string Staff_Security_Ques { get; set; }
        public string Staff_Security_Ans { get; set; }
    }
}