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
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            if (loginCookie_AMS_Admin != null)
            {
                Property p = new Property();
                DataSet ds = new DataSet();
                p.UserID = loginCookie_AMS_Admin["UserId"];
                p.assignmentID = "0";
                ds = dl.usp_getAssignmentList(p);
                ViewBag.assignmentlist = ds;
            }
            else
            {
                return RedirectToAction("Index", "Admin");

            }
            return View();
        }

        public ActionResult Question(string Id, string Id1)
        {

            Property p = new Property();
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];

            if (loginCookie_AMS_Admin != null)
            {
                if (Id1 != null)
                {
                    p.QuestionId = Convert.ToInt32(Id1);
                }
                else
                {
                    p.QuestionId = 0;
                }
                string UserID = loginCookie_AMS_Admin["UserId"];
                if (Id != null)
                {
                    p.UserID = loginCookie_AMS_Admin["UserId"];
                    p.assignmentID = Id;
                    DataSet ds = dl.usp_getAssignmentList(p);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.Assignment = ds.Tables[0].Rows[0]["AssignmentName"].ToString();
                    }
                    DataSet ds2 = dl.usp_getQuestion(p);
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.Question = ds2.Tables[0];
                    }
                    if (Id != null && Id1 != null)
                    {
                        DataSet ds3 = dl.Inline_Process("select * from [M02_Question] where QuestionId='" + p.QuestionId + "' and M01_AssignmentId='" + p.assignmentID + "'");
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            p.Question = ds3.Tables[0].Rows[0]["Question"].ToString();
                            p.MaxMark = ds3.Tables[0].Rows[0]["MaxMark"].ToString();
                        }
                        return View(p);
                    }
                }
                else
                {
                    TempData["Questionerror"] = "Please Select  Assignment";
                }

            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Question(Property p, string Id, string Id1)
        {
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            if (loginCookie_AMS_Admin != null)
            {
                if (Id == null)
                {
                    return RedirectToAction("Index");
                }

                int MaxMark = !string.IsNullOrEmpty(p.MaxMark) ? Convert.ToInt32(p.MaxMark) : 0;
                if (MaxMark > 20 || MaxMark == 0)
                {
                    TempData["Questionerror"] = "Full Marks Range 1 to 20";
                    return RedirectToAction("Question", "Assignment", new { id = Id });
                }
                int i = 0;
                if (Id1 != null)
                {
                    p.QuestionId = Convert.ToInt32(Id1);
                }
                else
                {
                    p.QuestionId = 0;
                }
                p.assignmentID = Id;
                i = dl.usp_setQuestion(p);
                if (i > 0)
                {
                    TempData["Questionsuccess"] = "Question Saved Successfully";
                    ModelState.Clear();
                    return RedirectToAction("Question", "Assignment", new { id = Id });

                }
                else
                {
                    TempData["Questionerror"] = "Question Not Saved";
                }
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult assign(Property p)
        {
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            if (loginCookie_AMS_Admin != null)
            {
                p.UserID = loginCookie_AMS_Admin["UserId"];
                p.assignmentID = "0";
                int i = dl.usp_setAssignmentList(p);
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Index", "Admin");

            }
        }
        public ActionResult StudentReport(string Id, string Id1)
        {
            Property p = new Property();
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            if (loginCookie_AMS_Admin != null)
            {
                p.UserID = loginCookie_AMS_Admin["UserId"];
                studentlist();
                getassignmentlist();
                if (Id != null && Id1 != null)
                {
                    p.studentid = Id1;
                    p.assignmentID = Id;
                    DataSet dsuser = dl.Inline_Process("select * from AdminLogin_tbl where UserId='" + p.UserID + "'");
                    if (dsuser.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.User = dsuser.Tables[0].Rows[0]["Name"].ToString();

                    }
                    DataSet dsStudentReport = dl.usp_getStudentReport(p);
                    if (dsStudentReport.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.FullName = dsStudentReport.Tables[0].Rows[0]["StudentName"].ToString();
                        ViewBag.assignmentname = dsStudentReport.Tables[0].Rows[0]["AssignMentName"].ToString();
                        ViewBag.LateDays = dsStudentReport.Tables[0].Rows[0]["LateDays"].ToString();
                        ViewBag.TotalMark = dsStudentReport.Tables[0].Rows[0]["TotalMark"].ToString();
                        ViewBag.StudentMark = dsStudentReport.Tables[0].Rows[0]["StudentMark"].ToString();
                        ViewBag.TotalPenalty = dsStudentReport.Tables[0].Rows[0]["TotalPenalty"].ToString();
                        ViewBag.StudentMarksPenalty = dsStudentReport.Tables[0].Rows[0]["TotalPenalty"].ToString();
                        ViewBag.StudentMarksPer = dsStudentReport.Tables[0].Rows[0]["TotalPenalty"].ToString();
                        ViewBag.Status = dsStudentReport.Tables[0].Rows[0]["Status"].ToString();

                    }
                    if (dsStudentReport.Tables[1].Rows.Count > 0)
                    {
                        ViewBag.StudentReport = dsStudentReport.Tables[1];
                    }

                }

            }
            else
            {
                return RedirectToAction("Index", "Admin");

            }
            return View(p);
        }
        [HttpPost]
        public ActionResult StudentReport(Property p)
        {
            studentlist();
            getassignmentlist();
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            if (loginCookie_AMS_Admin != null)
            {

                //DataSet dsStudent = dl.usp_getStudent(p);
                //DataSet dsAssignment = dl.usp_getAssignmentList(p);
                //if(dsStudent.Tables[0].Rows.Count>0)
                //{
                //    ViewBag.assignmentlist = ds.Tables[0].Rows[i]["AssignmentName"].ToString();

                //}
                //if (dsAssignment.Tables[0].Rows.Count > 0)
                //{
                //    ViewBag.assignmentlist = ds.Tables[0].Rows[i]["AssignmentName"].ToString();

                //}

            }
            else
            {
                return RedirectToAction("Index", "Admin");

            }
            return RedirectToAction("StudentReport", "Assignment", new { id = p.assignmentID, id1 = p.studentid });
        }
        public ActionResult getassignmentlist()
        {
            HttpCookie loginCookie_AMS_Admin = Request.Cookies["loginCookie_AMS_Admin"];
            if (loginCookie_AMS_Admin != null)
            {
                Property p = new Property();
                DataSet ds = new DataSet();
                List<SelectListItem> pp = new List<SelectListItem>();
                p.UserID = loginCookie_AMS_Admin["UserId"];
                p.assignmentID = "0";
                ds = dl.usp_getAssignmentList(p);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        pp.Add(new SelectListItem { Text = ds.Tables[0].Rows[i]["AssignmentName"].ToString(), Value = ds.Tables[0].Rows[i]["AssignmentId"].ToString() });

                    }
                    ViewBag.assignmentlist = new SelectList(pp, "Value", "Text");

                }

                ViewBag.assignmentlist = pp;
            }
            else
            {
                return RedirectToAction("Index", "Admin");

            }
            return View();
        }

        public void studentlist()
        {
            DataSet ds = new DataSet();
            Property p = new Property();

            ds = dl.usp_getStudent(p);
            List<SelectListItem> Student = new List<SelectListItem>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Student.Add(new SelectListItem { Text = ds.Tables[0].Rows[i]["name"].ToString(), Value = ds.Tables[0].Rows[i]["StudentId"].ToString() });

            }
            ViewBag.Studentlist = new SelectList(Student, "Value", "Text");

        }
    }
}