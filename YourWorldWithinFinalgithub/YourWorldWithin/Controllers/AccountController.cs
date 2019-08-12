using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using YourWorldWithin.Models;

namespace YourWorldWithin.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/


        Datalayer dl = new Datalayer();

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            HttpCookie loginCookie = Request.Cookies["login_cookie"];
            if (loginCookie != null)
            {
                loginCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(loginCookie);
            }
            HttpCookie lockingCookie = Request.Cookies["locking_cookie"];
            if (lockingCookie != null)
            {
                lockingCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(lockingCookie);
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Index()
        {

            return View();
        }
      
        public ActionResult UserRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRegister(Property p)
        {
            try
            {

                int i = 0;
                if(p.Password==p.ConPassword)
                {
                    i = dl.usp_SetUser(p);
                }
                else
                {
                    TempData["error"] = "Password and Confirm Password Not Match!";
                }
                
                if (i > 0)
                {
                    TempData["success"] = "Registered Successfully..";
                    TempData["msg"] = "1";
                }
                else
                {
                    TempData["error"] = "Not Registered!";
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Not Registered!";
            }

            return RedirectToAction("Index","Home");
        }
       
        [HttpPost]
        public ActionResult Index(Property p)
        {
            DataSet ds = new DataSet();

            //ds = dl.FETCH_LOGIN_DETAILS(p);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            if (p.EmailID == "Admin" && p.Password == "123456")
            {

                HttpCookie loginCookie = Request.Cookies["login_cookie"];
                if (loginCookie == null)
                {
                    loginCookie = new HttpCookie("login_cookie");
                    loginCookie["UserName"] = p.EmailID;
                    loginCookie["UserFullName"] = "Adam";
                    loginCookie["UserImg"] = "";
                    loginCookie["UserType"] = "ADMIN";
                    loginCookie.Expires = DateTime.Now.AddMinutes(20);
                    Response.Cookies.Add(loginCookie);
                }
                else
                {

                    loginCookie.Expires = DateTime.Now.AddDays(-1);

                    Response.Cookies.Add(loginCookie);
                    loginCookie = new HttpCookie("login_cookie");
                    loginCookie["UserName"] = p.EmailID;
                    loginCookie["UserFullName"] = "Adam";
                    loginCookie["UserImg"] = "";
                    loginCookie["UserType"] = "ADMIN";
                    loginCookie.Expires = DateTime.Now.AddMinutes(20);
                    Response.Cookies.Add(loginCookie);
                }

                return Redirect(Url.Action("Index","Admin"));

            }


            else
            {
                TempData["MSG"] = "Your Account is not activated!";
                return View();
            }

        }
        [HttpPost]
        public ActionResult UserLogin(Property p)
        {
            //DataSet ds = new DataSet();
            ////DataTable dt = ds.Tables[0];
            //ds = dl.usp_UserLogin(p);//Inline_Process("select * from [Registration_Tbl] where Emailid='" + model.EmailId + "' and Password='" + model.Password + "'").Tables[0];
            //if (ds.Tables[0].Rows[0]["LoginStatus"].ToString() == "True")
            //{
            //    //if (ds.Tables[1].Rows[0]["EmailId"].ToString() == model.EmailId)
            //    //{
            //    HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
            //    loginCookie_Costoracle_USER = new HttpCookie("loginCookie_Costoracle_USER");
            //    loginCookie_Costoracle_USER["UserId"] = ds.Tables[1].Rows[0]["userid"].ToString();
            //    loginCookie_Costoracle_USER["Name"] = ds.Tables[1].Rows[0]["Name"].ToString();
            //    // loginCookie_Costoracle_USER["UserType"] = ds.Tables[1].Rows[0]["UserType"].ToString();
            //    //  loginCookie_Costoracle_USER.Values[];
            //    loginCookie_Costoracle_USER.Expires = DateTime.Now.AddHours(1);
            //    Response.Cookies.Add(loginCookie_Costoracle_USER);

            //    HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];
            //    if (insuranceRequestCookies != null)
            //    {
            //        InsuranceRequestModel InsReqModel = new InsuranceRequestModel();

            //        //Session.Clear();
            //        InsReqModel.UserId = loginCookie_Costoracle_USER["UserId"];
            //        //  InsReqModel.UserId ="1";

            //        InsReqModel.VehicleTypeId = insuranceRequestCookies["VehicleTypeId"].ToString();
            //        // insuranceRequestModel.ProductName = insuranceRequestCookies["ProductName"].ToString(); 
            //        InsReqModel.RegistrationNumber = insuranceRequestCookies["RegistrationNumber"].ToString();
            //        InsReqModel.MakeId = insuranceRequestCookies["Make"].ToString();
            //        InsReqModel.ModelId = insuranceRequestCookies["Model"].ToString();
            //        InsReqModel.VehicleAge = insuranceRequestCookies["VehicleAge"].ToString();
            //        InsReqModel.TransmissionType = insuranceRequestCookies["TransmissionType"].ToString();
            //        InsReqModel.FuelType = insuranceRequestCookies["FuelType"].ToString();
            //        InsReqModel.ParkingId = insuranceRequestCookies["carkept"].ToString();
            //        InsReqModel.StateId = insuranceRequestCookies["StateId"].ToString();
            //        InsReqModel.LGAId = insuranceRequestCookies["LGAId"].ToString();
            //        InsReqModel.Mileage = insuranceRequestCookies["Mileage"].ToString();
            //        InsReqModel.DriverFirstName = insuranceRequestCookies["Firstname"].ToString();
            //        InsReqModel.DriverLastName = insuranceRequestCookies["Lastname"].ToString();

            //        InsReqModel.DriverDOB = insuranceRequestCookies["DriverDOB"].ToString();
            //        string dd = InsReqModel.DriverDOB.Split('-')[0];
            //        string mm = InsReqModel.DriverDOB.Split('-')[1];
            //        string yy = InsReqModel.DriverDOB.Split('-')[2];
            //        string dmy = yy + "-" + mm + "-" + dd;
            //        InsReqModel.DriverDOB = dmy;
            //        InsReqModel.CoverStartDate = insuranceRequestCookies["CoverStartDate"].ToString();
            //        string dd1 = InsReqModel.CoverStartDate.Split('-')[0];
            //        string mm1 = InsReqModel.CoverStartDate.Split('-')[1];
            //        string yy1 = InsReqModel.CoverStartDate.Split('-')[2];
            //        string dmy1 = yy1 + "-" + mm1 + "-" + dd1;
            //        InsReqModel.CoverStartDate = dmy1;
            //        InsReqModel.CoverLableId = insuranceRequestCookies["coverlevel"].ToString();
            //        InsReqModel.Duration = insuranceRequestCookies["insuranceduration"].ToString();
            //        InsReqModel.NoClaimYearId = insuranceRequestCookies["noclaimyr"].ToString();
            //        InsReqModel.VoluntaryExcess = insuranceRequestCookies["voluntaryexcess"].ToString();
            //        InsReqModel.CarValue = insuranceRequestCookies["CarValue"].ToString();
            //        InsReqModel.Mileunit = insuranceRequestCookies["Mileunit"].ToString();
            //        Acdl.usp_GetInsurenceRequest(InsReqModel);
            //    }

            //    HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];

            //    if (PrevPagePageCookie != null)
            //    {
            //        return Redirect(PrevPagePageCookie["PageURL"]);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Dashboard", "User");
            //    }
            //}
            //else
            //{
            //    TempData["loginerror"] = ds.Tables[0].Rows[0]["message"].ToString();
            //}
            ////}
            ////else
            ////{
            ////    TempData["loginerror"] = "Please provide valid username and password!";
            ////}
            return View();
        }

        public ActionResult Register()
        {
            //SubCategoryMenu();
            return View();
        }

    

        public void sendMail(string fName, string Email, string status, string vid, string code)
        {
            try
            {
                MailAddress mailfrom = new MailAddress("enquiry@hbitm.in", "Alampforeverytrade");
                MailAddress mailto = new MailAddress(Email);

                MailMessage newmsg = new MailMessage(mailfrom, mailto);
                newmsg.IsBodyHtml = true;
                newmsg.Subject = "Account Verification";
                newmsg.Body = "<html><head>    <title></title></head><body> <center><div style='width: 600px; background-color: #F2711F; margin: 30px 0 0 0; border: 1px solid #747474; color: white'><h1>Alampforeverytrade</h1><br /><br /><div style='width:90%; text-align:left; font-size:25px;'>Hello " + fName + "!<br /><br />Your id signed up at this site, Your new account is almost ready, but before you can login you need to confirm your email id by visiting the link below:<br /><a style='color:#002cff!Important;' href='http://Beauty4afro.com/Verification/Index/" + vid + "/" + status + "/" + code + "'>http://Beauty4afro.com/Verification/Index/" + vid + "/" + status + "/" + code + "</a><br /><br />Once you have visited the verification URL, your account will be activated.<br /><br />If you have any problems or questions, please reply to this email.<br /><br />Thanks!<br /><br /></div></div></center></body></html>";
                SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 25);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("enquiry@hbitm.in", "Gowebbi@123");
                smtp.EnableSsl = false;
                smtp.Send(newmsg);

                TempData["MSG"] = "";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
        }


        //[AllowAnonymous]
        //public string Ext_EmailID_Check(string Id)
        //{
        //    DataSet ds = new DataSet();
        //    Property p1 = new Property();
        //    p1.Condition1 = Id;
        //    p1.Condition2 = "";
        //    p1.onTable = "REG_EMAILID_CHECK";
        //    ds = dl.FETCH_CONDITIONAL_QUERY(p1);

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        return "Email ID Already Exists.";
        //    }
        //    else
        //    {
        //        return "Available";
        //    }

        //}



        [HttpPost]
        public ActionResult Change_Pass(Property p)
        {
            SqlConnection con = new SqlConnection(p.Con);
            con.Open();
            HttpCookie loginCookie = Request.Cookies["login_cookie"];
            string UserID = loginCookie.Values["UserName"];
            string str = "Update tbl_Registration set Password='" + p.Password + "' where EmailID='" + UserID + "' and Password='" + p.OldPassword + "'";
            try
            {
                SqlCommand cmd = new SqlCommand(str, con);
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    TempData["MSG"] = "Password Changed Successfully!!!";
                }
                else
                {
                    TempData["MSG"] = "Password Not Changed.";
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
            con.Close();
            return Redirect("/Settings");
        }


        public ActionResult Forget_Password()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Forget_Password(Property p)
        //{
        //    DataSet ds = new DataSet();
        //    p.Condition1 = p.EmailID;
        //    p.Condition2 = "";
        //    p.onTable = "FORGET_PASS";
        //    ds = dl.FETCH_CONDITIONAL_QUERY(p);

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        sendPasswordMail(ds.Tables[0].Rows[0]["FullName"].ToString(), ds.Tables[0].Rows[0]["EmailID"].ToString(), ds.Tables[0].Rows[0]["Password"].ToString());
        //        //TempData["MSG"] = "Check your mail. Your password send to your E-mail ID.";
        //    }
        //    else
        //    {
        //        TempData["MSG"] = "Email ID does not exists.";
        //    }
        //    return View();
        //}

        public void sendPasswordMail(string fName, string Email, string pass)
        {
            try
            {
                MailAddress mailfrom = new MailAddress("enquiry@hbitm.in", "Alampforeverytrade");
                MailAddress mailto = new MailAddress(Email);

                MailMessage newmsg = new MailMessage(mailfrom, mailto);

                newmsg.Subject = "Your Login Details";
                newmsg.Body = "Dear " + fName + Environment.NewLine + Environment.NewLine + "Your Login details for Beauty4afro  is:" + Environment.NewLine + Environment.NewLine + " Email ID : " + Email + Environment.NewLine + Environment.NewLine + "Password : " + pass + Environment.NewLine + Environment.NewLine + "For login, Click this link http://Beauty4afro.com/Account";
                SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 25);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("enquiry@hbitm.in", "Gowebbi@123");
                smtp.EnableSsl = false;
                smtp.Send(newmsg);

                TempData["MSG"] = "Check your mail. Your password send to your E-mail ID.";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
        }

        


    }
}
