using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surseed.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: AboutUs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OurStory()
        {
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }
    }
}