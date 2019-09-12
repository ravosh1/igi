using AMSS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMSS.Controllers
{
    public class AssignmentController : Controller
    {
        Datalayer dl = new Datalayer();
        // GET: Assignment
        public ActionResult Index()
        {
            Property p = new Property();
            DataSet ds = new DataSet();
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            p.UserID = loginCookie_AMS_Admin["UserId"];
            p.assignmentID = "0";
            ds = dl.usp_getAssignmentList(p);
            ViewBag.assignmentlist = ds;
            return View();
        }
        public ActionResult Question(string Id)
        {
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            if(loginCookie_AMS_Admin!=null)
            {
                string UserID = loginCookie_AMS_Admin["UserId"];
                if(Id!=null)
                {
                    DataSet ds = dl.Inline_Process("select * from [M01_Assignment] where AssignmentId='" + Id + "' and U01_UserId='" + UserID + "'");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.Assignment = ds.Tables[0].Rows[0]["AssignmentName"].ToString();
                    }
                }
                
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult assign(Property p)
        {
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            p.UserID = loginCookie_AMS_Admin["UserId"];
           p.assignmentID = "0";
            int i = dl.usp_setAssignmentList(p);
            return RedirectToAction("Index");
        }
    }
}