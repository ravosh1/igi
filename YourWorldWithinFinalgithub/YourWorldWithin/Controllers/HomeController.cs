using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourWorldWithin.Models;

namespace YourWorldWithin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home 
        Datalayer dl = new Datalayer();
        public ActionResult Index()
        {
            Property p = new Property();
            List<Property> plist = new List<Property>();
            DataSet ds = dl.usp_getVideo(p);           
                int j = 0;
                if (j < 8)
                    j = ds.Tables[0].Rows.Count;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < j; i++)
                    {
                        Property pp = new Property();
                        pp.VideoId = ds.Tables[0].Rows[i]["VideoId"].ToString();
                        pp.Title = ds.Tables[0].Rows[i]["Title"].ToString();
                        pp.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                        pp.Tags = ds.Tables[0].Rows[i]["Tags"].ToString();
                        pp.VideoFile = ds.Tables[0].Rows[i]["VideoFile"].ToString();
                        pp.ImageFile = ds.Tables[0].Rows[i]["ImageFile"].ToString();
                        pp.creationdate = ds.Tables[0].Rows[i]["createdatetime"].ToString();
                        plist.Add(pp);
                    }

                    ViewBag.videolist = plist;
                }
          
           return View();
        }
    }
}