using AMSS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMSS.Controllers
{
    public class StudentController : Controller
    {
        Datalayer dl = new Datalayer();
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            Property p = new Property();
            DataSet ds = new DataSet();
            ds = dl.usp_getStudent(p);
            ViewBag.studentlist = ds;
            return View();
        }


        [HttpPost]
        public ActionResult Adds(Property p)
        {
            //p.studentid = "0";
            int i = dl.usp_setStudent(p);
            if (i > 0)
            {
                TempData["msg"] = "Data Saved Successfully!!!";
                ModelState.Clear();
            }
            else
            {
                TempData["msg"] = "Data Not Saved!!!";
            }
            return RedirectToAction("Add");
        }

        [HttpPost]
        public ActionResult AddMarks()
        {
            Property p = new Property();
            DataSet ds = new DataSet();
            ds = dl.usp_getStudent(p);
            ViewBag.studentlist = ds;
            return View();
        }


        public void studentlist()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p = new Property();

            ds = dl.usp_getStudent(p);
            List<SelectListItem> Student = new List<SelectListItem>();

            Student.Add(new SelectListItem { Text = "-Select Student-", Value = "" });

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Student.Add(new SelectListItem { Text = ds.Tables[0].Rows[i]["name"].ToString(), Value = ds.Tables[0].Rows[i]["StudentId"].ToString() });

            }
            ViewBag.Studentlist = new SelectList(Student, "Value", "Text");

        }

        public void assignmentlist()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p = new Property();

            ds = dl.usp_getAssignmentList(p);
            List<SelectListItem> assignment = new List<SelectListItem>();

            assignment.Add(new SelectListItem { Text = "-Select Assignment-", Value = "" });

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                assignment.Add(new SelectListItem { Text = ds.Tables[0].Rows[i]["AssignmentName"].ToString(), Value = ds.Tables[0].Rows[i]["AssignmentId"].ToString() });

            }
            ViewBag.assignmentlist = new SelectList(assignment, "Value", "Text");

        }


        public ActionResult Marks(string id = "", string id1 = "")
        {
            Property p = new Models.Property();
            // Add();

            if (id != "")
            {
                p.studentid = id1;
                p.assignmentID = id;
                DataSet ds = dl.usp_getQuestion(p);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.Question = ds;
                }

                DataSet ds1 = dl.usp_getStudentReport(p);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ViewBag.getquestionmarks = ds1;
                }
            }
            assignmentlist();
            studentlist();
            return View(p);
        }

        [HttpPost]
        public ActionResult Markss(Property p)
        {
            studentlist();
            // Property p = new Property();
            if (p.assignmentID != null)
            {
                Marks(p.assignmentID, p.studentid);
            }
            else
            {
                TempData["Questionerror"] = "Please Select Assignment";
            }

            return Redirect(Url.Action("marks", "student", new { Id = p.assignmentID,id1=p.studentid }));
        }

        public JsonResult studentmarks(string id,string id1,string id2,string id3)
        {
            string studentmarksid = "0";
            int i = dl.usp_setStudentMarks(studentmarksid,id,id1,id2,id3);
            if (i > 0)
            {
                TempData["msg"]="Success";
            }
            else
            {
                TempData["msg"] = "Fail";
            }
            return Json(TempData["msg"],JsonRequestBehavior.AllowGet);
        }
    }
}