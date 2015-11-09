using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using VirtualCampus.Models;

namespace VirtualCampus.Controllers
{
    public class NewsController : Controller
    {
        private Virtual_CampusEntities obj = new Virtual_CampusEntities();
        public ActionResult Index()
        {
            //ViewBag.SubscribedCat = obj.VC_UserToNews.ToList();
            var q = (from m in obj.VC_UserToNews where m.User_Id == 10 select m.NewsCategory_Id);
            int i = 0;
            int length = q.Count();
            string[] val = new string[length];
            foreach (var NewsCatId in q)
            {
                var q1 = (from m in obj.VC_News_Category where m.NewsCategory_Id == NewsCatId select m).Single();
                val[i] = q1.CategoryName.ToString();
                ViewData["ashmi"] = val;
                i++;
            }
            return View();
        }

        public ActionResult NewsDetails(string k)
        {

            var q = (from m in obj.VC_News_Category where m.CategoryName == k select m.NewsCategory_Id).SingleOrDefault();
            int newscatid = q;
            var r = (from n in obj.VC_News_Link where n.NewsCategory_Id == newscatid select n.News_Id);

            return View();
        }
        public ActionResult RssTry(string k) {
            var tonewscat = (from p in obj.VC_News_Category where p.CategoryName == k select p).SingleOrDefault();
            var q = (from m in obj.VC_News_Link where m.NewsCategory_Id == tonewscat.NewsCategory_Id select m.News_Link);
            int numberoflinks = q.Count();
            ViewData["nolinks"] = numberoflinks;
            string[] rsslinks = new string[numberoflinks];
            int no = 0;
            foreach (string temprss in q) {
                rsslinks[no] = temprss;
                no++;
            }
            List<List<List<string>>> source = new List<List<List<string>>>();
            for (Int32 times = 0; times < numberoflinks; times++)
            {
                string rss = rsslinks[times];
                // Read the RSS feed
                WebRequest rssRequest = WebRequest.Create(rss);
                WebResponse rssResponse = rssRequest.GetResponse();
                Stream rssStream = rssResponse.GetResponseStream();
                // Load XML Document
                XmlDocument rssDocument = new XmlDocument();
                rssDocument.Load(rssStream);
                XmlNodeList rssList = rssDocument.SelectNodes("rss/channel/item");
                // Loop through RSS Feed items
                List<List<string>> item = new List<List<string>>();
                for (Int32 i = 0; i < 10; i++)
                {
                    
                    List<string> linktitle = new List<string>();
                    XmlNode rssDetail;
                    rssDetail = rssList.Item(i).SelectSingleNode("title");
                    if (rssDetail != null)
                    {
                        linktitle.Add(rssDetail.InnerText.ToString());
                    }
                  
                    /*
                    rssDetail = rssList.Item(i).SelectSingleNode("link");
                    if (rssDetail != null)
                    {
                        link[times,i] = rssDetail.InnerText;
                        //ViewData["link"] = link;
                    }
                    else
                    {
                        ViewData["link"] = "";
                    }
                    */
                    rssDetail = rssList.Item(i).SelectSingleNode("description");
                    if (rssDetail != null)
                    {
                        linktitle.Add(rssDetail.InnerText.ToString());
                        //description.Add(rssDetail.InnerText.ToString());
                    }
                    item.Add(linktitle);
                }
                source.Add(item);
            }
            ViewBag.display = source;
            return View();
        }
    }
}
