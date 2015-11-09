using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
namespace VirtualCampus.Models
{
    public class AddNewUser
    {
        [Required]

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "EmailId")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Privilage")]
        public int Privilage { get; set; }

        [Required]
        [Display(Name = "Workspace")]
        public string Workspace { get; set; }
    }

    //private Virtual_CampusEntities obj = new Virtual_CampusEntities();
    public class NewsCategoryModel
    {
        public int NewsCategory_Id { get; set; }
        [Required(ErrorMessage = "can not be blank")]
        public string CategoryName { get; set; }

    }

    public class EditNewsCategoryModel
    {
        //var q = (from m in obj.VC_News_Category where m.NewsCategory_Id == k select m.NewsCategory_Id);
        [Required(ErrorMessage = "can not be blank")]
        public string CategoryName
        {
            get;
            set;
        }

    }

    public class NewsSubCategoryModel
    {

        public int NewsCategory_Id { get; set; }
        public int News_Id { get; set; }

        [Required(ErrorMessage = "required")]
        public string NewsLink { get; set; }

        public string News_Description_Name
        {
            get;
            set;
        }
        /*private readonly List<VC_News_Category> _category;
        [Display(Name = "select Category")]
        public int NewsCategory_id { get; set; }

        public IEnumerable<SelectListItem> NewsCategory
        {
            get { return new SelectList(_category, "NewsCategory_id", "CategoryName"); }
        } */
    }


    public class EditNewsSubCategoryModel
    {

        public int NewsCategory_Id { get; set; }
        public int News_Id { get; set; }

        [Required(ErrorMessage = "required")]
        public string NewsLink { get; set; }

        public string News_Description_Name
        {
            get;
            set;
        }
        /*
        @Html.TextBoxFor(model => model.NewsLink, new { value = @ViewBag.edit_newslink})
        @Html.ValidationMessageFor(model => model.NewsLink)*/
    }

    //Interests

    public class InterestCategoryModel
    {
        public int InterestCategory_Id { get; set; }
        [Required(ErrorMessage = "required")]
        public string Category_Name { get; set; }

    }

    public class InterestSubCatModel
    {
        public int Interest_Id { get; set; }
        public int InterestCategory_Id { get; set; }
        public string Interest { get; set; }
        public string Interest_Description_Name { get; set; }
    }

    //AOS

    public class AOSCategoryModel
    {
        public int Areaofstudy_Category_Id { get; set; }
        public string category_Name { get; set; }

    }

    public class AOSSubCatModel
    {
        public int Areaofstudy_Id { get; set; }
        public int Areaofstudy_Category_Id { get; set; }
        public string Areaofstudy { get; set; }

    }
}