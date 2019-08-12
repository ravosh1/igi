
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourWorldWithin.Models;

namespace YourWorldWithin.Controllers
{
    public class MotivationalVedioController : Controller
    {
        Datalayer dl = new Datalayer();
        // GET: MotivationalVedio
        public ActionResult Index()
        {
            Property p = new Property();
            List<Property> plist = new List<Property>();
            DataSet ds = dl.usp_getVideo(p);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property pp = new Property();
                    pp.VideoId = ds.Tables[0].Rows[i]["videoid"].ToString();
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

        public ActionResult MVDetails(string id = "")
        {
            Property pp = new Models.Property();
            if (id == "")
            {
                pp.VideoId = "0";
            }
            else
            {

            pp.VideoId = id;
            }
            DataSet ds = dl.usp_getVideo(pp);
            if (ds.Tables[0].Rows.Count > 0)
            {
                pp = new Property();
                pp.VideoId = ds.Tables[0].Rows[0]["videoid"].ToString();
                pp.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                pp.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                pp.Tags = ds.Tables[0].Rows[0]["Tags"].ToString();
                pp.VideoFile = ds.Tables[0].Rows[0]["VideoFile"].ToString();
                pp.ImageFile = ds.Tables[0].Rows[0]["ImageFile"].ToString();
                pp.creationdate = ds.Tables[0].Rows[0]["createdatetime"].ToString();

            }
            return View(pp);
        }
    }
}