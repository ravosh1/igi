using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using costoracle2.Models;
using Classes;
using System.Data;
using costoracle2.Mailers;
namespace costoracle2.Controllers
{

    public class AccountController : Controller
    {
        DataLayerFunctions dl = new DataLayerFunctions();
        AccountDataLayer Acdl = new AccountDataLayer();
        EncryptDecrypt enc = new EncryptDecrypt();
        // UserMailer um = new UserMailer();
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
            if (loginCookie_Costoracle_USER != null)
            {
                loginCookie_Costoracle_USER.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(loginCookie_Costoracle_USER);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResetPassword(string id = "")
        {
            ForgetPasswordModel model = new ForgetPasswordModel();
            if (id != "")
            {
                model.RegistrationId = id;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ResetPassword(ForgetPasswordModel p)
        {

            //    EncryptDecrypt enc = new EncryptDecrypt();
            //// string User_Id = uid;
            try
            {
                dl.Inline_ExecuteNonQry("update [Registration_Tbl] set Password='" + p.ConfirmPassword + "' where RegistrationId='" + p.RegistrationId + "'");
                TempData["changepasswordSUCCESS"] = "Password changed successfully! Login to continue.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["changepassworderror"] = "Sorry!Password not changed";
            }
            return View();

        }

        public ActionResult ForgotPassword()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgetPasswordModel model)
        {
            try
            {
                //string RegistrationId = loginCookie_Costoracle_USER["UserId"].ToString();

                DataTable dt = new DataTable();
                dt = dl.Inline_Process("select * from [Registration_Tbl] where EmailId='" + model.EmailId + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        UserMailer um = new UserMailer();
                        um.Userforgetpassword(dt.Rows[0]["Password"].ToString(), enc.Encrypt(dt.Rows[0]["RegistrationId"].ToString()), enc.Encrypt(dt.Rows[0]["EmailId"].ToString()), dt.Rows[0]["Name"].ToString(), "").Send();
                        ModelState.Clear();
                        TempData["forgotsuccess"] = "Link sent to your registered email.";
                    }
                    catch (Exception ex)
                    {
                        TempData["forgoterror"] = "Failed to send mail.Try Again!";
                    }
                }
                else
                {
                    TempData["forgoterror"] = "Please enter your registered email.";
                }

            }
            catch (Exception ex)
            {
                TempData["forgoterror"] = "Failed to send mail.Try Again!";
            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(RegisterModel model)
        {
            DataSet ds = new DataSet();
            //DataTable dt = ds.Tables[0];
            ds = Acdl.usp_UserLogin(model);//Inline_Process("select * from [Registration_Tbl] where Emailid='" + model.EmailId + "' and Password='" + model.Password + "'").Tables[0];
            if (ds.Tables[0].Rows[0]["LoginStatus"].ToString() == "True")
            {
                //if (ds.Tables[1].Rows[0]["EmailId"].ToString() == model.EmailId)
                //{
                HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
                loginCookie_Costoracle_USER = new HttpCookie("loginCookie_Costoracle_USER");
                loginCookie_Costoracle_USER["UserId"] = ds.Tables[1].Rows[0]["userid"].ToString();
                loginCookie_Costoracle_USER["Name"] = ds.Tables[1].Rows[0]["Name"].ToString();
                // loginCookie_Costoracle_USER["UserType"] = ds.Tables[1].Rows[0]["UserType"].ToString();
                //  loginCookie_Costoracle_USER.Values[];
                loginCookie_Costoracle_USER.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(loginCookie_Costoracle_USER);

                HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];
                    if (insuranceRequestCookies != null)
                    {
                        InsuranceRequestModel InsReqModel = new InsuranceRequestModel();

                        //Session.Clear();
                        InsReqModel.UserId = loginCookie_Costoracle_USER["UserId"];
                        //  InsReqModel.UserId ="1";

                        InsReqModel.VehicleTypeId = insuranceRequestCookies["VehicleTypeId"].ToString();
                        // insuranceRequestModel.ProductName = insuranceRequestCookies["ProductName"].ToString(); 
                        InsReqModel.RegistrationNumber = insuranceRequestCookies["RegistrationNumber"].ToString();
                        InsReqModel.MakeId = insuranceRequestCookies["Make"].ToString();
                        InsReqModel.ModelId = insuranceRequestCookies["Model"].ToString();
                        InsReqModel.VehicleAge = insuranceRequestCookies["VehicleAge"].ToString();
                        InsReqModel.TransmissionType = insuranceRequestCookies["TransmissionType"].ToString();
                        InsReqModel.FuelType = insuranceRequestCookies["FuelType"].ToString();
                        InsReqModel.ParkingId = insuranceRequestCookies["carkept"].ToString();
                        InsReqModel.StateId = insuranceRequestCookies["StateId"].ToString();
                        InsReqModel.LGAId = insuranceRequestCookies["LGAId"].ToString();
                        InsReqModel.Mileage = insuranceRequestCookies["Mileage"].ToString();
                        InsReqModel.DriverFirstName = insuranceRequestCookies["Firstname"].ToString();
                        InsReqModel.DriverLastName = insuranceRequestCookies["Lastname"].ToString();

                        InsReqModel.DriverDOB = insuranceRequestCookies["DriverDOB"].ToString();
                        string dd = InsReqModel.DriverDOB.Split('-')[0];
                        string mm = InsReqModel.DriverDOB.Split('-')[1];
                        string yy = InsReqModel.DriverDOB.Split('-')[2];
                        string dmy = yy + "-" + mm + "-" + dd;
                        InsReqModel.DriverDOB = dmy;
                        InsReqModel.CoverStartDate = insuranceRequestCookies["CoverStartDate"].ToString();
                        string dd1 = InsReqModel.CoverStartDate.Split('-')[0];
                        string mm1 = InsReqModel.CoverStartDate.Split('-')[1];
                        string yy1 = InsReqModel.CoverStartDate.Split('-')[2];
                        string dmy1 = yy1 + "-" + mm1 + "-" + dd1;
                        InsReqModel.CoverStartDate = dmy1;
                        InsReqModel.CoverLableId = insuranceRequestCookies["coverlevel"].ToString();
                        InsReqModel.Duration = insuranceRequestCookies["insuranceduration"].ToString();
                        InsReqModel.NoClaimYearId = insuranceRequestCookies["noclaimyr"].ToString();
                        InsReqModel.VoluntaryExcess = insuranceRequestCookies["voluntaryexcess"].ToString();
                        InsReqModel.CarValue = insuranceRequestCookies["CarValue"].ToString();
                        InsReqModel.Mileunit = insuranceRequestCookies["Mileunit"].ToString();
                        Acdl.usp_GetInsurenceRequest(InsReqModel);
                    }
                HttpCookie itemdeliverycookies = Request.Cookies["itemdeliverycookies"];
                if (itemdeliverycookies != null)
                {
                    Itemdelivery d = new Models.Itemdelivery();
                    itemdeliverycookies["userid"] = loginCookie_Costoracle_USER["UserId"];
                    Response.Cookies.Add(itemdeliverycookies);
                    // d.itemtypeid = itemdeliverycookies["itemtypeid"];
                    //// d.itemtype = ds.Tables[0].Rows[0]["itemtype"].ToString();
                    // d.Addresspick = itemdeliverycookies["Addresspick"];
                    // d.Latitudepick = itemdeliverycookies["Latitudepick"];
                    // d.Longitudepick = itemdeliverycookies["Longitudepick"];
                    // d.Citypick = itemdeliverycookies["Citypick"];
                    // d.Countrypick = itemdeliverycookies["Countrypick"];
                    // d.Name = itemdeliverycookies["Name"];
                    // d.Address = itemdeliverycookies["Address"];
                    // d.Latitude = itemdeliverycookies["Latitude"];
                    // d.Longitude = itemdeliverycookies["Longitude"];
                    // d.City = itemdeliverycookies["City"];
                    // d.Country = itemdeliverycookies["Country"];
                    // d.noofitem = itemdeliverycookies["noofitem"];
                    // d.width = itemdeliverycookies["width"];
                    // d.height = itemdeliverycookies["height"];
                    // d.length = itemdeliverycookies["length"];
                    // d.weight = itemdeliverycookies["weight"];
                    return RedirectToAction(itemdeliverycookies["Action"], itemdeliverycookies["Controller"]);
                }
                    HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];

                    if (PrevPagePageCookie != null)
                    {
                        return Redirect(PrevPagePageCookie["PageURL"]);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "User");
                    }
            }
            else
            {
                TempData["loginerror"] = ds.Tables[0].Rows[0]["message"].ToString();
            }
            //}
            //else
            //{
            //    TempData["loginerror"] = "Please provide valid username and password!";
            //}
            return View();
        }

        public ActionResult ResToGetPrices()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResToGetPrices(RegisterModel model)
        {
            int i = 0;
            try
            {
              //  ModelState.Remove();
               // if (ModelState.IsValid)
               // {
                   // string RegistrationId = dl.Geenrate4Randomnumber("R");
                  //  model.RegistrationId = RegistrationId;
                  //  model.UserType = "user";
                    //model.UserName = model.EmailId;
                    model.EmailVerified = true.ToString();
                    if (Acdl.checkUserEmailExists(model.EmailId))
                    {
                        TempData["registersuccess"] = "EmailId Already Exists!";
                    }
                    else
                    {
                        i = Acdl.UserRegistration(model);

                        if (i > 0)
                        {
                            DataTable dt = new DataTable();

                            dt = dl.Inline_Process("select * from usr.U01_User where Emailid='" + model.EmailId + "'").Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["EmailId"].ToString() == model.EmailId)
                                {
                                    HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
                                    loginCookie_Costoracle_USER = new HttpCookie("loginCookie_Costoracle_USER");

                                    loginCookie_Costoracle_USER["UserId"] = dt.Rows[0]["userid"].ToString();
                                    loginCookie_Costoracle_USER["Name"] = dt.Rows[0]["Name"].ToString();
                                    //loginCookie_Costoracle_USER["UserType"] = dt.Rows[0]["UserType"].ToString();
                                    loginCookie_Costoracle_USER.Expires = DateTime.Now.AddHours(1);
                                    Response.Cookies.Add(loginCookie_Costoracle_USER);
                                }
                            }

                            TempData["registersuccess"] = "You have successfully registed!";
                            //ModelState.Clear();

                            if (Session["VID"] != null)
                            {
                                HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
                                HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
                             

                                InsuranceRequestModel InsReqModel = new InsuranceRequestModel();

                                InsReqModel.UserId = loginCookie_Costoracle_USER["UserId"];
                                InsReqModel.VehicleTypeId = insurancecookies["VID"].ToString();
                                InsReqModel.RegistrationNumber = insurancecookies["VehicleRegNo"].ToString();
                                InsReqModel.Make = insurancecookies["Make"].ToString();
                                InsReqModel.Models = insurancecookies["Models"].ToString();
                                InsReqModel.VehicleAge = insurancecookies["VehicleAge"].ToString();
                                InsReqModel.TransmissionType = insurancecookies["TransmissionType"].ToString();
                                InsReqModel.FuelType = insurancecookies["FuelType"].ToString();
                                InsReqModel.ParkingId = insurancecookies["carkept"].ToString();
                                InsReqModel.StateId = insurancecookies["StateId"].ToString();
                                InsReqModel.LGAId = insurancecookies["LGAId"].ToString();
                                InsReqModel.Mileage = insurancecookies["Mileage"].ToString();
                                InsReqModel.DriverFirstName = insurancecookies["Firstname"].ToString();
                                InsReqModel.DriverLastName = insurancecookies["Lastname"].ToString();
                                InsReqModel.DriverDOB = insurancecookies["DriverDOB"].ToString();
                                InsReqModel.CoverStartDate = insurancecookies["CoverStartDate"].ToString();
                                InsReqModel.CoverLableId = insurancecookies["coverlevel"].ToString();
                                InsReqModel.Duration = insurancecookies["insuranceduration"].ToString();
                                InsReqModel.NoClaimYearId = insurancecookies["noclaimyr"].ToString();
                                InsReqModel.VoluntaryExcess = insurancecookies["voluntaryexcess"].ToString();

                                Acdl.usp_GetInsurenceRequest(InsReqModel);

                                Session.Clear();
                            }

                            HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];

                            if (PrevPagePageCookie != null)
                            {
                                return Redirect(PrevPagePageCookie["PageURL"]);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            TempData["registersuccess"] = "Failed To register!";
                        return View(model);
                    }
                    }
                //}
                //else
                //{
                //    TempData["registererror"] = "Oops! Please fill all the mandatory fields.";
                //    return View(model);
                //}

            }
            catch (Exception ex)
            {
                TempData["registererror"] = ex.Message;
            }

            return RedirectToAction("/", "Services");

        }

        public ActionResult ServProvdrTyp()
        {
            return View();
        }

        public ActionResult ResasServProvider(String id)
        {
            if (id != null)
            {
                TempData["SrvcTyp"] = id;
            }
            else
            {
                TempData["SrvcTyp"] = "Back & Select Your Service!";
            }
            return View();
        }


        public ActionResult ServiceproviderLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ServiceproviderLogin(ServiceprovideREGrModel model)
        {
            DataSet ds = new DataSet();
            //DataTable dt = ds.Tables[0];
            ds = Acdl.usp_ProviderLogin(model);//Inline_Process("select * from [Registration_Tbl] where Emailid='" + model.EmailId + "' and Password='" + model.Password + "'").Tables[0];
            if (ds.Tables[0].Rows[0]["LoginStatus"].ToString() == "True")
            {
                HttpCookie loginCookie_Costoracle_PROVIDER = Request.Cookies["loginCookie_Costoracle_PROVIDER"];
                loginCookie_Costoracle_PROVIDER = new HttpCookie("loginCookie_Costoracle_PROVIDER");
                loginCookie_Costoracle_PROVIDER["UserId"] = ds.Tables[1].Rows[0]["userid"].ToString();
                loginCookie_Costoracle_PROVIDER["Name"] = ds.Tables[1].Rows[0]["Name"].ToString();
                loginCookie_Costoracle_PROVIDER.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(loginCookie_Costoracle_PROVIDER);
                
                return RedirectToAction("Dashboard", "Provider");
                
            }
            else
            {
                TempData["loginerror"] = ds.Tables[0].Rows[0]["message"].ToString();
            }
            //}
            //else
            //{
            //    TempData["loginerror"] = "Please provide valid username and password!";
            //}
            return View();
        }

        public ActionResult selectlogin()
        {
            return View();
        }

    }
}