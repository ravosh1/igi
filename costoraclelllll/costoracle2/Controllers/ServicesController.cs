using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using costoracle2.Models;
using Classes;
using System.Data;
using System.Net;
using System.Xml;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using costoracle2.Mailers;
using System.Net.Mail;


namespace costoracle2.Controllers
{
    public class ServicesController : Controller
    {
        DataLayerFunctions dl = new DataLayerFunctions();
        AccountDataLayer Acdl = new AccountDataLayer();
        UserMailer um = new UserMailer();
        InsuranceRequestModel insuranceRequestModel = new InsuranceRequestModel();
        // HttpCookie insuranceRequestCookies = new HttpCookie("insuranceRequestCookies");
        public ActionResult Insurance()
        {
            try
            {
                // InsuranceRequestModel insuranceRequestModel = new InsuranceRequestModel();
                //InsuranceModel model = new Models.InsuranceModel();
                HttpCookie insuranceRequestCookies = new HttpCookie("insuranceRequestCookies");
                //HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];

                insuranceRequestCookies["VehicleTypeId"] = "";
                // insuranceRequestCookies["ProductName"] = insuranceRequestModel.ProductName;
                insuranceRequestCookies["RegistrationNumber"] = "";
                insuranceRequestCookies["Make"] = "";
                insuranceRequestCookies["Model"] = "";
                insuranceRequestCookies["VehicleAge"] = "";
                insuranceRequestCookies["TransmissionType"] = "";
                insuranceRequestCookies["FuelType"] = "";

                //////////////////

                insuranceRequestCookies["carkept"] = "";
                insuranceRequestCookies["StateId"] = "";
                insuranceRequestCookies["LGAId"] = "";
                insuranceRequestCookies["Mileage"] = "";
                insuranceRequestCookies["Mileunit"] = "";
                insuranceRequestCookies["CarValue"] = "";
                /////////////////////
                insuranceRequestCookies["Firstname"] = "";
                insuranceRequestCookies["Lastname"] = "";
                insuranceRequestCookies["DriverDOB"] = "";
                /////////////////////////
                insuranceRequestCookies["CoverStartDate"] = "";
                insuranceRequestCookies["coverlevel"] = "";
                insuranceRequestCookies["insuranceduration"] = "";
                insuranceRequestCookies["noclaimyr"] = "";
                insuranceRequestCookies["voluntaryexcess"] = "";
                Response.Cookies.Add(insuranceRequestCookies);

                if (insuranceRequestCookies["VehicleTypeId"] != "")
                {
                    insuranceRequestModel.VehicleTypeId = insuranceRequestCookies["VehicleTypeId"].ToString();
                    insuranceRequestModel.ProductName = insuranceRequestCookies["ProductName"].ToString();
                    insuranceRequestModel.RegistrationNumber = insuranceRequestCookies["RegistrationNumber"].ToString();
                    insuranceRequestModel.Make = insuranceRequestCookies["Make"].ToString();
                    insuranceRequestModel.Models = insuranceRequestCookies["Model"].ToString();
                    insuranceRequestModel.VehicleAge = insuranceRequestCookies["VehicleAge"].ToString();
                    insuranceRequestModel.TransmissionType = insuranceRequestCookies["TransmissionType"].ToString();
                    insuranceRequestModel.FuelType = insuranceRequestCookies["FuelType"].ToString();

                    //TempData["session"] = "Sorry";

                    ////////////// Vehicle Type //////////////////
                    getVehicleType gvt = new Models.getVehicleType();
                    gvt.VehicalTypeId = null;
                    DataSet dsVehicleType = Acdl.usp_getVehicleType(gvt);
                    ViewBag.VehicleType = dsVehicleType;
                    ////////End Of Vehicle Type /////////////




                    ///////////////////////////////////// Make List ////////////////////////////////////////

                    //HttpWebRequest request = null;
                    //HttpWebResponse response = null;
                    //String Xml;

                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    //request = WebRequest.Create("https://vpic.nhtsa.dot.gov/api/vehicles/getallmakes?format=xml") as HttpWebRequest;

                    //using (response = request.GetResponse() as System.Net.HttpWebResponse)
                    //{
                    //    StreamReader reader = new StreamReader(response.GetResponseStream());

                    //    Xml = reader.ReadToEnd();
                    //}

                    //XmlTextReader XMLreader = new XmlTextReader(new StringReader(Xml));


                    InsuranceRequestModel model = new InsuranceRequestModel();

                    DataSet ds = new DataSet();

                    ds = Acdl.usp_getMake(model);

                    //  ds.Tables[0].DefaultView.Sort = "Make asc";

                    DataTable dt = ds.Tables[0];

                    List<SelectListItem> MakeList = new List<SelectListItem>();
                    //MakeList.Add(new SelectListItem { Text = insuranceRequestModel.Make, Value = insuranceRequestModel.Make });
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["MakeID"]) < 600)
                        {

                            MakeList.Add(new SelectListItem { Text = dr["Make"].ToString(), Value = dr["MakeId"].ToString() });
                        }
                    }

                    ViewBag.make = MakeList;

                    /////////////////////////////////////End Of Make List////////////////////////////////////////

                    /////////////////////////////////////Transmission_List////////////////////////////////////////

                    HttpWebRequest requestTransmission = null;
                    HttpWebResponse responseTransmission = null;
                    String XmlTransmission;

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    requestTransmission = WebRequest.Create("https://databases.one/api/?format=xml&select=gearbox&make_id=140&model_id=876&api_key=Your_Database_Api_Key") as HttpWebRequest;

                    using (responseTransmission = requestTransmission.GetResponse() as System.Net.HttpWebResponse)
                    {
                        StreamReader readerTransmission = new StreamReader(responseTransmission.GetResponseStream());

                        XmlTransmission = readerTransmission.ReadToEnd();
                    }

                    XmlTextReader XMLreaderTransmission = new XmlTextReader(new StringReader(XmlTransmission));

                    DataSet dsTransmission = new DataSet();

                    dsTransmission.ReadXml(XMLreaderTransmission);

                    List<SelectListItem> TransmissionList = new List<SelectListItem>();
                    TransmissionList.Add(new SelectListItem { Text = insuranceRequestModel.TransmissionType, Value = insuranceRequestModel.TransmissionType });
                    foreach (DataRow drTransmission in dsTransmission.Tables[0].Rows)
                    {
                        TransmissionList.Add(new SelectListItem { Text = drTransmission["gearbox"].ToString(), Value = drTransmission["gearbox"].ToString() });
                    }

                    ViewBag.TransmissionLst = TransmissionList;

                    ///////////////////////////////////// End of Transmission List ////////////////////////////////////////


                    return View(insuranceRequestModel);
                }

                else
                {
                    TempData["session"] = "Sorry";

                    InsuranceRequestModel model = new InsuranceRequestModel();
                    List<SelectListItem> listofvehicleyear = new List<SelectListItem>();
                    listofvehicleyear.Add(new SelectListItem { Text = "I don't Know Vehicle Age", Value = "-1" });
                    for (int i = 1970; i <= 2019; i++)
                    {
                        listofvehicleyear.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });

                    }
                    ViewBag.VehicleYear = listofvehicleyear.OrderByDescending(b => b.Text).ToList();


                    ////////////// VehicleType //////////////////
                    getVehicleType gvt = new Models.getVehicleType();
                    gvt.VehicalTypeId = null;
                    DataSet dsVehicleType = Acdl.usp_getVehicleType(gvt);
                    List<SelectListItem> LstVehicleType = new List<SelectListItem>();

                    foreach (DataRow dr in dsVehicleType.Tables[0].Rows)
                    {
                        LstVehicleType.Add(new SelectListItem { Text = dr["VehicalType"].ToString(), Value = dr["VehicalTypeId"].ToString() });
                        //  LstVehicleType.Add(new SelectListItem { Text = dr["VehicalType"].ToString(), Value = dr["VehicalType"].ToString() });

                    }
                    ViewBag.VehicleType = LstVehicleType;
                    //////// End Of VehicleType ///////////


                    ///////////////////////////////////// Make List ////////////////////////////////////////

                    DataSet ds = new DataSet();

                    // model.Make = null;

                    ds = Acdl.usp_getMake(model);

                    //  ds.Tables[0].DefaultView.Sort = "Make asc";

                    DataTable dt = ds.Tables[0];

                    List<SelectListItem> MakeList = new List<SelectListItem>();
                    //MakeList.Add(new SelectListItem { Text = insuranceRequestModel.Make, Value = insuranceRequestModel.Make });
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["MakeID"]) < 600)
                        {

                            MakeList.Add(new SelectListItem { Text = dr["Make"].ToString(), Value = dr["MakeId"].ToString() });
                        }
                    }

                    ViewBag.make = MakeList;

                    /////////////////////////////////////End of Make_List////////////////////////////////////////

                    /////////////////////////////////////Transmission_List////////////////////////////////////////

                    HttpWebRequest requestTransmission = null;
                    HttpWebResponse responseTransmission = null;
                    String XmlTransmission;

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    requestTransmission = WebRequest.Create("https://databases.one/api/?format=xml&select=gearbox&make_id=140&model_id=876&api_key=Your_Database_Api_Key") as HttpWebRequest;

                    using (responseTransmission = requestTransmission.GetResponse() as System.Net.HttpWebResponse)
                    {
                        StreamReader readerTransmission = new StreamReader(responseTransmission.GetResponseStream());

                        XmlTransmission = readerTransmission.ReadToEnd();
                    }

                    XmlTextReader XMLreaderTransmission = new XmlTextReader(new StringReader(XmlTransmission));

                    DataSet dsTransmission = new DataSet();

                    dsTransmission.ReadXml(XMLreaderTransmission);

                    List<SelectListItem> TransmissionList = new List<SelectListItem>();
                    //TransmissionList.Add(new SelectListItem { Text = model.Transmission, Value = model.Transmission });
                    foreach (DataRow drTransmission in dsTransmission.Tables[0].Rows)
                    {
                        TransmissionList.Add(new SelectListItem { Text = drTransmission["gearbox"].ToString(), Value = drTransmission["gearbox"].ToString() });
                    }

                    ViewBag.TransmissionLst = TransmissionList;
                    // Session["Transmission"]= TransmissionList;
                    /////////////////////////////////////Transmission_List////////////////////////////////////////


                    return View();

                }
            }
            catch (Exception ex)
            {
                TempData["SesonTimout"] = "Sorry, Your Session Is Timeout! Try Again.";
                return View();
            }
        }

        public List<SelectListItem> GetModelList(string MakeId)
        {
            try
            {

                HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
                InsuranceRequestModel model = new InsuranceRequestModel();

                DataSet ds = new DataSet();
                model.Make = MakeId;
                ds = Acdl.usp_getModel(model);

                ds.Tables[0].DefaultView.Sort = "Model asc";

                DataTable dtModel = ds.Tables[0].DefaultView.ToTable();

                List<SelectListItem> ModlList = new List<SelectListItem>();
                if (insurancecookies != null)
                {
                    ModlList.Add(new SelectListItem { Text = insurancecookies["Model"], Value = insurancecookies["Model"] });
                }
                foreach (DataRow dr in dtModel.Rows)
                {
                    ModlList.Add(new SelectListItem { Text = dr["Model"].ToString(), Value = dr["Modelid"].ToString() });
                }

                return ModlList;
            }
            catch (Exception ex)
            {
                List<SelectListItem> ModlList = new List<SelectListItem>();
                ModlList.Add(new SelectListItem { Text = "", Value = "" });
                return ModlList;

            }
        }

        public JsonResult GetModelByMake(string id)
        {
            List<SelectListItem> MBM = GetModelList(id);

            return Json(MBM, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Insurance(InsuranceRequestModel model)
        {
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];

            try
            {

                if (model.ageother == "-1")
                {
                    model.VehicleAge = model.ageother;
                }

                insuranceRequestCookies["VehicleTypeId"] = model.VehicleTypeId;

                insuranceRequestCookies["RegistrationNumber"] = model.RegistrationNumber;
                insuranceRequestCookies["Make"] = model.MakeId;
                insuranceRequestCookies["Model"] = model.ModelId;
                insuranceRequestCookies["VehicleAge"] = model.VehicleAge;
                insuranceRequestCookies["TransmissionType"] = model.TransmissionType;
                insuranceRequestCookies["FuelType"] = model.FuelType;

                Response.Cookies.Add(insuranceRequestCookies);

                // MailMessage mail = new MailMessage();
                // mail.To.Add("web5@goigi.com");

                // mail.From = new MailAddress("no-reply@goigi.technology");
                // mail.Subject = "Test Email";
                // string Body = "Hi";
                // mail.Body = Body;

                // SmtpClient smtp = new SmtpClient();
                // smtp.Host = "relay-hosting.secureserver.net"; //Or Your SMTP Server Address
                // smtp.Port = 25;
                // smtp.UseDefaultCredentials = false;
                // smtp.Credentials = new System.Net.NetworkCredential
                // ("no-reply@goigi.technology", "reply@2017");
                // //Or your Smtp Email ID and Password
                // smtp.EnableSsl = false;
                //// smtp.authentication = ;

                // smtp.Send(mail);


                return Redirect(Url.Action("AddVehicleInfo", "Services" /*, new { id = model.VehicleId }*/));


            }
            catch (Exception ex)
            {
                TempData["VehicleInfoerror"] = ex.Message;
            }
            return View();
        }

        public ActionResult VehicleInfo()
        {
            return RedirectToAction("Insurance", "Services");
        }

        public List<SelectListItem> GetLgaBYstate(string LGAstate)
        {
            getlga lg = new Models.getlga();
            lg.stateId = LGAstate;
            DataSet ds = Acdl.USP_GETLGA(lg);

            List<SelectListItem> LGAList = new List<SelectListItem>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LGAList.Add(new SelectListItem { Text = dr["LGAName"].ToString(), Value = dr["LGAId"].ToString() });
                }
            }

            return LGAList;
        }

        public JsonResult LGAlistByState(string id)
        {
            List<SelectListItem> LGAADD = GetLgaBYstate(id);

            return Json(LGAADD, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddVehicleInfo()
        {
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];

            getstate gs = new Models.getstate();
            if (insuranceRequestCookies["carkept"] != "")
            {

                insuranceRequestModel.ParkingId = insuranceRequestCookies["carkept"].ToString();
                insuranceRequestModel.StateId = insuranceRequestCookies["StateId"].ToString();
                insuranceRequestModel.LGAId = insuranceRequestCookies["LGAId"].ToString();
                insuranceRequestModel.Mileage = insuranceRequestCookies["Mileage"].ToString();
                insuranceRequestModel.CarValue = insuranceRequestCookies["CarValue"].ToString();
                insuranceRequestModel.Mileunit = insuranceRequestCookies["Mileunit"].ToString();
                //// Car parking //////

                getParking gp = new getParking();
                gp.ParkingId = null;
                DataSet dsParking = Acdl.usp_getParking(gp);
                List<SelectListItem> LstParking = new List<SelectListItem>();


                foreach (DataRow dr in dsParking.Tables[0].Rows)
                {
                    LstParking.Add(new SelectListItem { Text = dr["Parking"].ToString(), Value = dr["ParkingId"].ToString() });

                }
                ViewBag.Parking = LstParking;

                ////End Of Car parking/////


                DataSet ds = Acdl.USP_GETSTATE(gs);//dl.Inline_Process("Select distinct([State]) from [MTR].[M01_LGA]");

                List<SelectListItem> LGAList = new List<SelectListItem>();
                //  LGAList.Add(new SelectListItem { Text = model.LGA_Address, Value = model.LGA_Address});
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        LGAList.Add(new SelectListItem { Text = dr["StateName"].ToString(), Value = dr["StateId"].ToString() });
                    }
                }

                ViewBag.ListLGA = LGAList;

                return View(insuranceRequestModel);
            }
            else
            {
                //TempData["SesonTimout"] = "Sorry, Your Session Is Timeout! Try Again.";
                insuranceRequestModel.ParkingId = "";
                insuranceRequestModel.StateId = "";
                insuranceRequestModel.LGAId = "";
                insuranceRequestModel.Mileage = "";
                insuranceRequestModel.CarValue = "";
                insuranceRequestModel.Mileunit = "";
                //return RedirectToAction("Insurance", "Services");
                getParking gp = new getParking();
                gp.ParkingId = null;
                DataSet dsParking = Acdl.usp_getParking(gp);
                List<SelectListItem> LstParking = new List<SelectListItem>();


                foreach (DataRow dr in dsParking.Tables[0].Rows)
                {
                    LstParking.Add(new SelectListItem { Text = dr["Parking"].ToString(), Value = dr["ParkingId"].ToString() });

                }
                ViewBag.Parking = LstParking;

                ////End Of Car parking/////




                DataSet ds = Acdl.USP_GETSTATE(gs);//dl.Inline_Process("Select distinct([State]) from [MTR].[M01_LGA]");

                List<SelectListItem> LGAList = new List<SelectListItem>();
                //  LGAList.Add(new SelectListItem { Text = model.LGA_Address, Value = model.LGA_Address});
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        LGAList.Add(new SelectListItem { Text = dr["StateName"].ToString(), Value = dr["StateId"].ToString() });
                    }
                }

                ViewBag.ListLGA = LGAList;

                return View(insuranceRequestModel);
            }
        }

        [HttpPost]
        public ActionResult AddVehicleInfo(InsuranceRequestModel model)
        {
            //int i = 0;
            // HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];
            try
            {

                if (model.ParkingId != null)
                {
                    //insurancecookies["carkept"] = model.carkept;
                    //insurancecookies["State"] = model.LGA_AddressState;
                    //insurancecookies["Mileage"] = model.Mileage;
                    //insurancecookies["Mileunit"] = model.Mileunit;
                    //insurancecookies["Address"] = model.LGA_Address;
                    insuranceRequestCookies["carkept"] = model.ParkingId;
                    insuranceRequestCookies["StateId"] = model.StateId;
                    insuranceRequestCookies["LGAId"] = model.LGAId;
                    insuranceRequestCookies["Mileage"] = model.Mileage;
                    insuranceRequestCookies["CarValue"] = model.CarValue;
                    insuranceRequestCookies["Mileunit"] = model.Mileunit;

                    Response.Cookies.Add(insuranceRequestCookies);
                    return Redirect(Url.Action("YourInfo", "Services" /*, new { id = model.VehicleId }*/));
                }
                else
                {

                    TempData["SesonTimout"] = "Sorry, Your Session Is Timeout! Try Again.";

                    return RedirectToAction("Insurance", "Services");
                }


            }
            catch (Exception ex)
            {
                TempData["VehicleInfoerror"] = ex.Message;
            }
            return View();

        }

        public JsonResult AutoCompleteLGA(string term)
        {
            // WebClient httpClient = new WebClient();
            HttpWebRequest requestFueltype = null;
            HttpWebResponse responseFueltype = null;



            WebClient web = new WebClient();
            string url = string.Format("http://locationsng-api.herokuapp.com/api/v1/lgas");
            string response = web.DownloadString(url);
            string Xmll = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(response), new XmlDictionaryReaderQuotas())).ToString();


            List<SelectListItem> FueltypeList = new List<SelectListItem>();

            //foreach (DataRow drFueltype in dsFueltype.Tables[0].Rows)
            //{
            //    FueltypeList.Add(new SelectListItem { Text = drFueltype["engine_type"].ToString(), Value = drFueltype["engine_type"].ToString() });
            //}

            ViewBag.FueltypeLst = FueltypeList;


            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult YourInfo()
        {
            //  HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
            //  yourinfoModel model = new yourinfoModel();
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];
            if (insuranceRequestCookies["Firstname"] != "")
            {


                insuranceRequestModel.DriverFirstName = insuranceRequestCookies["Firstname"].ToString();
                insuranceRequestModel.DriverLastName = insuranceRequestCookies["Lastname"].ToString();

                insuranceRequestModel.Day = insuranceRequestCookies["day"].ToString();
                insuranceRequestModel.Month = insuranceRequestCookies["month"].ToString();
                insuranceRequestModel.Year = insuranceRequestCookies["year"].ToString();

                int i = 1;

                List<SelectListItem> listofdays = new List<SelectListItem>();
                for (i = 1; i <= 31; i++)
                {
                    listofdays.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });

                }
                ViewBag.Days = listofdays;

                List<SelectListItem> listofmonths = new List<SelectListItem>();


                for (i = 1; i <= 12; i++)
                {
                    if (i == 1)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jan", Value = "Jan" });
                    }
                    if (i == 2)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Feb", Value = "Feb" });
                    }
                    if (i == 3)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Mar", Value = "Mar" });
                    }
                    if (i == 4)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Apr", Value = "Apr" });
                    }
                    if (i == 5)
                    {
                        listofmonths.Add(new SelectListItem { Text = "May", Value = "May" });
                    }
                    if (i == 6)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jun", Value = "Jun" });
                    }
                    if (i == 7)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jul", Value = "Jul" });
                    }
                    if (i == 8)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Aug", Value = "Aug" });
                    }
                    if (i == 9)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Sep", Value = "Sep" });
                    }
                    if (i == 10)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Oct", Value = "Oct" });
                    }
                    if (i == 11)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Nov", Value = "Nov" });
                    }
                    if (i == 12)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Dec", Value = "Dec" });
                    }
                }
                ViewBag.Months = listofmonths;


                List<SelectListItem> listofyears = new List<SelectListItem>();
                string ii = DateTime.Now.ToString("yyyy");
                int j = Convert.ToInt32(ii);
                for (j = 1950; j <= Convert.ToInt32(ii) - 18; j++)
                {
                    listofyears.Add(new SelectListItem { Text = j.ToString(), Value = j.ToString() });
                }
                ViewBag.Years = listofyears;

                return View(insuranceRequestModel);
            }
            else
            {

                int i = 1;

                List<SelectListItem> listofdays = new List<SelectListItem>();
                for (i = 1; i <= 31; i++)
                {
                    listofdays.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });

                }
                ViewBag.Days = listofdays;

                List<SelectListItem> listofmonths = new List<SelectListItem>();


                for (i = 1; i <= 12; i++)
                {
                    if (i == 1)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jan", Value = "Jan" });
                    }
                    if (i == 2)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Feb", Value = "Feb" });
                    }
                    if (i == 3)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Mar", Value = "Mar" });
                    }
                    if (i == 4)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Apr", Value = "Apr" });
                    }
                    if (i == 5)
                    {
                        listofmonths.Add(new SelectListItem { Text = "May", Value = "May" });
                    }
                    if (i == 6)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jun", Value = "Jun" });
                    }
                    if (i == 7)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jul", Value = "Jul" });
                    }
                    if (i == 8)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Aug", Value = "Aug" });
                    }
                    if (i == 9)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Sep", Value = "Sep" });
                    }
                    if (i == 10)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Oct", Value = "Oct" });
                    }
                    if (i == 11)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Nov", Value = "Nov" });
                    }
                    if (i == 12)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Dec", Value = "Dec" });
                    }
                }
                ViewBag.Months = listofmonths;


                List<SelectListItem> listofyears = new List<SelectListItem>();
                string ii = DateTime.Now.ToString("yyyy");
                //int j = 1950;
                int j = Convert.ToInt32(ii);
                for (j = 1950; j <= Convert.ToInt32(ii) - 18; j++)
                {
                    listofyears.Add(new SelectListItem { Text = j.ToString(), Value = j.ToString() });

                }
                ViewBag.Years = listofyears;
                return View();
            }
        }

        [HttpPost]
        public ActionResult YourInfo(InsuranceRequestModel model)
        {
            // HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];
            try
            {

                if (model.DriverFirstName != null)
                {
                    //insurancecookies["FirstName"] = model.fname;
                    //insurancecookies["LastName"] = model.lname;
                    //insurancecookies["agedriver"] = model.agedriver;
                    insuranceRequestCookies["Firstname"] = model.DriverFirstName;
                    insuranceRequestCookies["Lastname"] = model.DriverLastName;
                    insuranceRequestCookies["day"] = model.Day;
                    insuranceRequestCookies["month"] = model.Month;
                    insuranceRequestCookies["year"] = model.Year;
                    insuranceRequestCookies["DriverDOB"] = model.Day + "-" + model.Month + "-" + model.Year;
                    Response.Cookies.Add(insuranceRequestCookies);
                    return Redirect(Url.Action("CoverInfo", "Services"));
                }
                else
                {
                    TempData["SesonTimout"] = "Sorry, Your Session Is Timeout! Try Again.";

                    return RedirectToAction("Insurance", "Services");
                }


            }
            catch (Exception ex)
            {
                TempData["VehicleInfoerror"] = ex.Message;
            }
            return View();

        }

        public ActionResult CoverInfo()
        {
            // HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
            // CoverinfoModel model = new CoverinfoModel();
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];
            if (insuranceRequestCookies["coverlevel"] != "")
            {
                insuranceRequestModel.Day = insuranceRequestCookies["coverdays"].ToString();
                insuranceRequestModel.Month = insuranceRequestCookies["covermonths"].ToString();
                insuranceRequestModel.Year = insuranceRequestCookies["coveryears"].ToString();

                //insuranceRequestModel.CoverStartDate = insuranceRequestCookies["CoverStartDate"].ToString();
                insuranceRequestModel.CoverLableId = insuranceRequestCookies["coverlevel"].ToString();
                insuranceRequestModel.Duration = insuranceRequestCookies["insuranceduration"].ToString();
                insuranceRequestModel.NoClaimYearId = insuranceRequestCookies["noclaimyr"].ToString();
                insuranceRequestModel.VoluntaryExcess = insuranceRequestCookies["voluntaryexcess"].ToString();

                //Response.Cookies.Add(insuranceRequestModel);



                //// Cover Level //////

                getCoverLevel gcl = new getCoverLevel();
                gcl.CoverLevelId = null;
                DataSet dsCoverLevel = Acdl.usp_getCoverlevel(gcl);
                List<SelectListItem> LstCoverLevel = new List<SelectListItem>();


                foreach (DataRow dr in dsCoverLevel.Tables[0].Rows)
                {
                    LstCoverLevel.Add(new SelectListItem { Text = dr["CoverType"].ToString(), Value = dr["CoverTypeId"].ToString() });

                }
                ViewBag.CoverLevel = LstCoverLevel;


                int i = 1;

                List<SelectListItem> listofdays = new List<SelectListItem>();
                for (i = 1; i <= 31; i++)
                {
                    listofdays.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });

                }
                ViewBag.Days = listofdays;

                List<SelectListItem> listofmonths = new List<SelectListItem>();


                for (i = 1; i <= 12; i++)
                {
                    if (i == 1)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jan", Value = "Jan" });
                    }
                    if (i == 2)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Feb", Value = "Feb" });
                    }
                    if (i == 3)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Mar", Value = "Mar" });
                    }
                    if (i == 4)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Apr", Value = "Apr" });
                    }
                    if (i == 5)
                    {
                        listofmonths.Add(new SelectListItem { Text = "May", Value = "May" });
                    }
                    if (i == 6)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jun", Value = "Jun" });
                    }
                    if (i == 7)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jul", Value = "Jul" });
                    }
                    if (i == 8)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Aug", Value = "Aug" });
                    }
                    if (i == 9)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Sep", Value = "Sep" });
                    }
                    if (i == 10)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Oct", Value = "Oct" });
                    }
                    if (i == 11)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Nov", Value = "Nov" });
                    }
                    if (i == 12)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Dec", Value = "Dec" });
                    }
                }
                ViewBag.Months = listofmonths;


                List<SelectListItem> listofyears = new List<SelectListItem>();
                string ii = DateTime.Now.ToString("yyyy");
                //int j = 1950;
                int j = Convert.ToInt32(ii);
                for (j = 1950; j <= Convert.ToInt32(ii); j++)
                {
                    listofyears.Add(new SelectListItem { Text = j.ToString(), Value = j.ToString() });

                }
                ViewBag.Years = listofyears.OrderByDescending(d => d.Text).ToList();
                ////End Of Cover Level/////

                // InsuranceRequestModel model = new InsuranceRequestModel();
                DataSet ds1 = Acdl.Inline_Process("select * from [MTR].[M10_NoClaimYear]");
                List<SelectListItem> mm = new List<SelectListItem>();
                //  string ii = DateTime.Now.ToString("yyyy");
                //int j = 1950;
                //int j = Convert.ToInt32(ii);
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    mm.Add(new SelectListItem { Text = ds1.Tables[0].Rows[jj]["Noclaimyear"].ToString(), Value = ds1.Tables[0].Rows[jj]["Noclaimyearid"].ToString() });

                }
                ViewBag.noclaimlist = mm;

                return View(insuranceRequestModel);
            }
            else
            {

                DataSet ds1 = Acdl.Inline_Process("select * from [MTR].[M10_NoClaimYear]");
                List<SelectListItem> mm = new List<SelectListItem>();
                //  string ii = DateTime.Now.ToString("yyyy");
                //int j = 1950;
                //int j = Convert.ToInt32(ii);
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    mm.Add(new SelectListItem { Text = ds1.Tables[0].Rows[jj]["Noclaimyear"].ToString(), Value = ds1.Tables[0].Rows[jj]["Noclaimyearid"].ToString() });

                }
                ViewBag.noclaimlist = mm;
                //TempData["SesonTimout"] = "Must fillup vehicle details first.";
                int i = 1;

                List<SelectListItem> listofdays = new List<SelectListItem>();
                for (i = 1; i <= 31; i++)
                {
                    listofdays.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });

                }
                ViewBag.Days = listofdays;

                List<SelectListItem> listofmonths = new List<SelectListItem>();


                for (i = 1; i <= 12; i++)
                {
                    if (i == 1)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jan", Value = "Jan" });
                    }
                    if (i == 2)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Feb", Value = "Feb" });
                    }
                    if (i == 3)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Mar", Value = "Mar" });
                    }
                    if (i == 4)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Apr", Value = "Apr" });
                    }
                    if (i == 5)
                    {
                        listofmonths.Add(new SelectListItem { Text = "May", Value = "May" });
                    }
                    if (i == 6)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jun", Value = "Jun" });
                    }
                    if (i == 7)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Jul", Value = "Jul" });
                    }
                    if (i == 8)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Aug", Value = "Aug" });
                    }
                    if (i == 9)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Sep", Value = "Sep" });
                    }
                    if (i == 10)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Oct", Value = "Oct" });
                    }
                    if (i == 11)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Nov", Value = "Nov" });
                    }
                    if (i == 12)
                    {
                        listofmonths.Add(new SelectListItem { Text = "Dec", Value = "Dec" });
                    }
                }
                ViewBag.Months = listofmonths;


                List<SelectListItem> listofyears = new List<SelectListItem>();
                string ii = DateTime.Now.ToString("yyyy");
                //int j = 1950;
                int j = Convert.ToInt32(ii);
                for (j = 1950; j <= Convert.ToInt32(ii); j++)
                {
                    listofyears.Add(new SelectListItem { Text = j.ToString(), Value = j.ToString() });
                }
                ViewBag.Years = listofyears.OrderByDescending(d => d.Text).ToList();
                //return RedirectToAction("Insurance", "Services");

                getCoverLevel gcl = new getCoverLevel();
                gcl.CoverLevelId = null;
                DataSet dsCoverLevel = Acdl.usp_getCoverlevel(gcl);
                List<SelectListItem> LstCoverLevel = new List<SelectListItem>();

                foreach (DataRow dr in dsCoverLevel.Tables[0].Rows)
                {

                    LstCoverLevel.Add(new SelectListItem { Text = dr["CoverType"].ToString(), Value = dr["CoverTypeId"].ToString() });

                }
                ViewBag.CoverLevel = LstCoverLevel;

                ////End Of Cover Level/////

                return View();
            }
        }

        [HttpPost]
        public ActionResult CoverInfo(InsuranceRequestModel model)
        {
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];
            //int i = 0;
            // HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
            try
            {
                //model.VehicleId = id;
                //i = Acdl.INSERT_UPDATE_COVER_INFORMATION(model);
                //if (i > 0)
                //{
                //    TempData["VehicleInfosuccess"] = "Vehicle information save successfully!";
                //    ModelState.Clear();

                if (model.CoverLableId != null)
                {
                    insuranceRequestCookies["coverdays"] = model.Day;
                    insuranceRequestCookies["covermonths"] = model.Month;
                    insuranceRequestCookies["coveryears"] = model.Year;
                    insuranceRequestCookies["CoverStartDate"] = model.Day + "-" + model.Month + "-" + model.Year;
                    // Response.Cookies.Add(insuranceRequestModel);
                    insuranceRequestCookies["coverlevel"] = model.CoverLableId;
                    insuranceRequestCookies["insuranceduration"] = model.Duration;
                    insuranceRequestCookies["noclaimyr"] = model.NoClaimYearId;
                    insuranceRequestCookies["voluntaryexcess"] = model.VoluntaryExcess;
                    Response.Cookies.Add(insuranceRequestCookies);
                    return Redirect(Url.Action("Review", "Services" /*, new { id = model.VehicleId }*/));
                }
                else
                {
                    TempData["SesonTimout"] = "Sorry, Your Session Is Timeout! Try Again.";

                    return RedirectToAction("CoverInfo", "Services");

                    // return View();
                }

                //}
                //else
                //{
                //    TempData["VehicleInfoerror"] = "Failed To register!";
                //}
            }
            catch (Exception ex)
            {
                TempData["VehicleInfoerror"] = ex.Message;
            }
            return View();

        }

        public ActionResult Review()
        {
            HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];
            if (insuranceRequestCookies["VehicleTypeId"] != "")
            {
                int ii = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(insuranceRequestCookies["VehicleAge"].ToString());
                insuranceRequestModel.DriverDOB = insuranceRequestCookies["day"].ToString() + "-" + insuranceRequestCookies["month"].ToString() + "-" + insuranceRequestCookies["year"].ToString();

                insuranceRequestModel.CoverStartDate = insuranceRequestCookies["coverdays"].ToString() + "-" + insuranceRequestCookies["covermonths"].ToString() + "-" + insuranceRequestCookies["coveryears"].ToString(); ;

                insuranceRequestModel.VehicleTypeId = insuranceRequestCookies["VehicleTypeId"].ToString();
                DataSet ds = dl.Inline_Process("select [VehicalType] from [MTR].[M15_VehicleType] where [VehicalTypeId]='" + insuranceRequestModel.VehicleTypeId + "'");
                insuranceRequestModel.VehicleType = ds.Tables[0].Rows[0]["VehicalType"].ToString();
                insuranceRequestModel.RegistrationNumber = insuranceRequestCookies["RegistrationNumber"].ToString();
                insuranceRequestModel.MakeId = insuranceRequestCookies["Make"].ToString();
                DataSet ds5 = dl.Inline_Process("select [make] from [MTR].[M04_Make] where [makeid]='" + insuranceRequestModel.MakeId + "'");
                insuranceRequestModel.Make = ds5.Tables[0].Rows[0]["make"].ToString();
                insuranceRequestModel.ModelId = insuranceRequestCookies["Model"].ToString();
                DataSet ds6 = dl.Inline_Process("select [model] from [MTR].[M05_Model] where [Modelid]='" + insuranceRequestModel.ModelId + "'");
                insuranceRequestModel.Models = ds6.Tables[0].Rows[0]["model"].ToString();
                insuranceRequestModel.VehicleAge = ii.ToString();
                insuranceRequestModel.TransmissionType = insuranceRequestCookies["TransmissionType"].ToString();
                insuranceRequestModel.FuelType = insuranceRequestCookies["FuelType"].ToString();
                insuranceRequestModel.ParkingId = insuranceRequestCookies["carkept"].ToString();
                DataSet ds1 = dl.Inline_Process("select [parking] from [MTR].[M16_Parking] where [parkingId]='" + insuranceRequestModel.ParkingId + "'");
                insuranceRequestModel.Parking = ds1.Tables[0].Rows[0]["parking"].ToString();

                insuranceRequestModel.StateId = insuranceRequestCookies["StateId"].ToString();
                DataSet ds4 = dl.Inline_Process("select [statename] from [MTR].[M02_State] where [stateid]='" + insuranceRequestModel.StateId + "'");

                insuranceRequestModel.State = ds4.Tables[0].Rows[0]["statename"].ToString();


                insuranceRequestModel.LGAId = insuranceRequestCookies["LGAId"].ToString();
                DataSet ds3 = dl.Inline_Process("select [lganame] from [MTR].[M03_LGA] where [lgaid]='" + insuranceRequestModel.LGAId + "'");
                insuranceRequestModel.LGAaddress = ds3.Tables[0].Rows[0]["lganame"].ToString();
                insuranceRequestModel.Mileage = insuranceRequestCookies["Mileage"].ToString();
                insuranceRequestModel.Mileunit = insuranceRequestCookies["Mileunit"].ToString();
                insuranceRequestModel.DriverFirstName = insuranceRequestCookies["Firstname"].ToString();
                insuranceRequestModel.DriverLastName = insuranceRequestCookies["Lastname"].ToString();

                insuranceRequestModel.CoverLableId = insuranceRequestCookies["coverlevel"].ToString();
                DataSet ds2 = dl.Inline_Process("select [covertype] from [MTR].[M18_CoverType] where [covertypeId]='" + insuranceRequestModel.CoverLableId + "'");
                insuranceRequestModel.CoverLable = ds2.Tables[0].Rows[0]["covertype"].ToString();
                insuranceRequestModel.Duration = insuranceRequestCookies["insuranceduration"].ToString();
                insuranceRequestModel.NoClaimYearId = insuranceRequestCookies["noclaimyr"].ToString();
                insuranceRequestModel.VoluntaryExcess = insuranceRequestCookies["voluntaryexcess"].ToString();
                insuranceRequestModel.CarValue = insuranceRequestCookies["CarValue"].ToString();
                //Response.Cookies.Add(insuranceRequestCookies);
                return View(insuranceRequestModel);
            }
            else
            {
                insuranceRequestModel.VehicleAge = insuranceRequestCookies["VehicleAge"].ToString();
                return View(insuranceRequestModel);
            }
        }

        [HttpPost]
        public ActionResult Review(InsuranceRequestModel model)
        {
            HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
            //  HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
            HttpCookie insuranceRequestCookies = Request.Cookies["insuranceRequestCookies"];

            //if (loginCookie_Costoracle_USER != null)
            //{
            if (model.DriverDOB != null)
            {
                InsuranceRequestModel InsReqModel = new InsuranceRequestModel();
                if (loginCookie_Costoracle_USER != null)
                {
                    InsReqModel.UserId = loginCookie_Costoracle_USER["UserId"];
                }
                else
                {
                    InsReqModel.UserId = "1";

                }

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
                if (loginCookie_Costoracle_USER != null)
                {
                    Acdl.usp_GetInsurenceRequest(InsReqModel);
                    //return RedirectToAction("Results", "Services");
                }
                else
                {
                    //  return RedirectToAction("Login", "Account");
                }

                // insuranceRequestCookies.Expires = DateTime.Now.AddHours(-1);
                // Response.Cookies.Add(insuranceRequestCookies);


                // Session.Clear();

                return RedirectToAction("Results", "Services");
            }
            else
            {
                TempData["SesonTimout"] = "Sorry, Your Session Is Timeout! Try Again.";

                return RedirectToAction("Insurance", "Services");
            }
            //}
            //else
            //{
            //    return RedirectToAction("Results", "Services");
            //}
        }

        public ActionResult Results()
        {
            HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];

            if (loginCookie_Costoracle_USER != null)
            {
                HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];
                // HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
                if (PrevPagePageCookie != null)
                {
                    PrevPagePageCookie.Expires = DateTime.Now.AddHours(-1);
                    Response.Cookies.Add(PrevPagePageCookie);
                }


                ServiceprovideREGrModel model = new Models.ServiceprovideREGrModel();
                List<ServiceprovideREGrModel> mmlist = new List<Models.ServiceprovideREGrModel>();
                //    getinsurancequotelist model = new Models.getinsurancequotelist();
                //model.InsuranceId = null;
                model.UserId = loginCookie_Costoracle_USER["UserId"];
                DataSet ds = Acdl.usp_getInsuranceQuoteList(model);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        model = new Models.ServiceprovideREGrModel();
                        model.businessname = ds.Tables[0].Rows[i]["businessname"].ToString();
                        model.address = ds.Tables[0].Rows[i]["address"].ToString();
                        model.price = ds.Tables[0].Rows[i]["price"].ToString();
                        model.phone = ds.Tables[0].Rows[i]["phone"].ToString();
                        mmlist.Add(model);
                    }
                    ViewBag.quotelist = mmlist;
                }
                return View();
            }
            else
            {
                TempData["MustLogin"] = "You must login or register!";
                TempData["loginerror"] = "";

                HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];
                PrevPagePageCookie = new HttpCookie("StrPrevPageCookie");
                PrevPagePageCookie["PageURL"] = Url.Action("Results", "Services");
                Response.Cookies.Add(PrevPagePageCookie);

                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult ItemDelivery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ItemDelivery(Itemdelivery model)
        {
            return View();
        }

        public ActionResult itemform()
        {
            return View();
        }

        [HttpPost]
        public ActionResult itemform(Itemdelivery model)
        {
            HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
            if (loginCookie_Costoracle_USER == null)
            {
                //model.Id = loginCookie_Costoracle_USER["UserId"];
                HttpCookie itemdeliverycookies = Request.Cookies["itemdeliverycookies"];

                itemdeliverycookies = new HttpCookie("itemdeliverycookies");
                //itemdeliverycookies["UserId"] = model.Id;
                itemdeliverycookies["Name"] = model.Name;
                itemdeliverycookies["Address"] = model.Address;
                itemdeliverycookies["Addresspick"] = model.Addresspick;
                itemdeliverycookies["Latitude"] = model.Latitude;
                itemdeliverycookies["Latitudepick"] = model.Latitudepick;
                itemdeliverycookies["Longitude"] = model.Longitude;
                itemdeliverycookies["Longitudepick"] = model.Longitudepick;
                itemdeliverycookies["City"] = model.City;
                itemdeliverycookies["Citypick"] = model.Citypick;
                itemdeliverycookies["Country"] = model.Country;
                itemdeliverycookies["Countrypick"] = model.Countrypick;
                itemdeliverycookies["Citypick"] = model.Citypick;
                itemdeliverycookies["Action"] = "packageinfo";
                itemdeliverycookies["Controller"] = "Services";
                Response.Cookies.Add(itemdeliverycookies);
                return Redirect(Url.Action("Login", "Account"));
            }
            else
            {
                HttpCookie itemdeliverycookies = Request.Cookies["itemdeliverycookies"];
                itemdeliverycookies = new HttpCookie("itemdeliverycookies");
                itemdeliverycookies["userid"] = loginCookie_Costoracle_USER["UserId"];
                itemdeliverycookies["Name"] = model.Name;
                itemdeliverycookies["Address"] = model.Address;
                itemdeliverycookies["Addresspick"] = model.Addresspick;
                itemdeliverycookies["Latitude"] = model.Latitude;
                itemdeliverycookies["Latitudepick"] = model.Latitudepick;
                itemdeliverycookies["Longitude"] = model.Longitude;
                itemdeliverycookies["Longitudepick"] = model.Longitudepick;
                itemdeliverycookies["City"] = model.City;
                itemdeliverycookies["Citypick"] = model.Citypick;
                itemdeliverycookies["Country"] = model.Country;
                itemdeliverycookies["Countrypick"] = model.Countrypick;
                itemdeliverycookies["Citypick"] = model.Citypick;
                //itemdeliverycookies["Action"] = "packageinfo";
                //itemdeliverycookies["Controller"] = "Services";
                Response.Cookies.Add(itemdeliverycookies);
                return Redirect(Url.Action("packageinfo", "Services"));
            }
            // int i = Acdl.usp_SetItemDeliveryRequest(model);

           
            // }
            return RedirectToAction("packageinfo");
            // return View();
        }


        public ActionResult AddsInfo()
        {
            return RedirectToAction("ItemDelivery", "Services");
        }

        public ActionResult PackageInfo()
        {
            return View();
        }

        public ActionResult iteminfo(int? id = 0)
        {
            Itemdelivery d = new Itemdelivery();
            d.itemtypeid = id.ToString();
            if (id == 1)
            {
                d.itemtype = "Documents";
            }
            if (id == 2)
            {
                d.itemtype = "Parcel";
            }
            if (id == 3)
            {
                d.itemtype = "Heavy Items";
            }
            return View(d);
        }


        [HttpPost]
        public ActionResult iteminfo(Itemdelivery model, string id = "")
        {
            HttpCookie itemdeliverycookies = Request.Cookies["itemdeliverycookies"];

            itemdeliverycookies["itemtypeid"] = id;
            itemdeliverycookies["noofitem"] = model.noofitem;
            itemdeliverycookies["weight"] = model.weight;
            itemdeliverycookies["width"] = model.width;
            itemdeliverycookies["length"] = model.length;
            itemdeliverycookies["height"] = model.height;

            Response.Cookies.Add(itemdeliverycookies);
            return RedirectToAction("ItemReview");
        }



        public ActionResult Reviews()
        {
            return View();
        }



        public ActionResult Result()
        {
            return View();
        }


        // Item Delivery Part End
        //TradeProfessionals Part Starts

        public ActionResult TradeProfessionals()
        {
            //HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];

            //if (loginCookie_Costoracle_USER != null)
            //{
            //    HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];

            //    if (PrevPagePageCookie != null)
            //    {
            //        PrevPagePageCookie.Expires = DateTime.Now.AddHours(-1);
            //        Response.Cookies.Add(PrevPagePageCookie);
            //    }

            //    return View();
            //}
            //else
            //{
            //    TempData["MustLogin"] = "You must login or register!";
            //    TempData["loginerror"] = "";

            //    HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];
            //    PrevPagePageCookie = new HttpCookie("StrPrevPageCookie");
            //    PrevPagePageCookie["PageURL"] = Url.Action("TradeProfessionals", "Services");
            //    Response.Cookies.Add(PrevPagePageCookie);

            //    return RedirectToAction("Login", "Account");
            //}

            return View();
        }

        public ActionResult ServiceRequired()
        {
            return RedirectToAction("TradeProfessionals", "Service");
        }

        public ActionResult ServiceDescriptionon()
        {
            return View();
        }

        public ActionResult ReviewForTrade()
        {
            return View();
        }

        public ActionResult ResultsForTarde()
        {
            return View();
        }
        //TradeProfessionals Part Ends

        public ActionResult CommoditiesAndCurrency()
        {
            return View();
        }

        public List<SelectListItem> RevwGetModelList(string MakeId)
        {
            HttpCookie insurancecookies = Request.Cookies["insurancecookies"];
            InsuranceRequestModel model = new InsuranceRequestModel();

            DataSet ds = new DataSet();
            model.Make = MakeId;
            ds = Acdl.usp_getModel(model);

            ds.Tables[0].DefaultView.Sort = "Model asc";

            DataTable dtModel = ds.Tables[0].DefaultView.ToTable();

            List<SelectListItem> ModlList = new List<SelectListItem>();
            if (insurancecookies != null)
            {
                ModlList.Add(new SelectListItem { Text = insurancecookies["Model"], Value = insurancecookies["Model"] });
            }
            foreach (DataRow dr in dtModel.Rows)
            {
                ModlList.Add(new SelectListItem { Text = dr["Model"].ToString(), Value = dr["Modelid"].ToString() });
            }



            return ModlList;
        }

        [ValidateInput(false)]
        public JsonResult RevwGetModelByMake(string id)
        {
            List<SelectListItem> MBM = RevwGetModelList(id);

            return Json(MBM, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Provider()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Provider(ServiceprovideREGrModel model)
        {
            AccountDataLayer dl = new Classes.AccountDataLayer();
            int i = dl.INSERT_UPDATE_SERVICE_PROVIDER_REGISTER(model);
            if (i > 0)
            {
                TempData["msg"] = "Registered Successfully!!";
            }
            else
            {
                TempData["msg"] = "Registered Failed!!";
            }
            return RedirectToAction("Subscription");
        }

        [AllowAnonymous]
        public string Ext_EmailID_Check(string Id)
        {
            DataSet ds = new DataSet();
            //  Property p1 = new Property();
            ds = dl.Inline_Process("select email from tbl_service_provider where email='" + Id + "'");

            if (ds.Tables[0].Rows.Count > 0)
            {
                return "Email ID Already Exists.";
            }
            else
            {
                return "Available";
            }

        }


        [AllowAnonymous]
        public string Ext_EmailID_Checks(string Id)
        {
            DataSet ds = new DataSet();
            //  Property p1 = new Property();
            ds = dl.Inline_Process("select emailid from usr.U01_User where emailid='" + Id + "'");

            if (ds.Tables[0].Rows.Count > 0)
            {
                return "Email ID Already Exists.";
            }
            else
            {
                return "Available";
            }

        }

        public ActionResult Subscription()
        {
            return View();
        }


        public ActionResult ItemReview()
        {

            HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
            HttpCookie itemdeliverycookies = Request.Cookies["itemdeliverycookies"];
            DataSet ds = dl.Inline_Process("select itemtype from MTR.M31_Itemtype where itemtypeid='" + @itemdeliverycookies["itemtypeid"] + "'");
            Itemdelivery d = new Itemdelivery();
            if (loginCookie_Costoracle_USER != null)
            {
                d.userid = loginCookie_Costoracle_USER["UserId"];
                DataSet ds1 = dl.Inline_Process("select Name from USR.U01_User where userid='" + d.userid + "'");
                if (ds1.Tables[0].Rows.Count > 0)
                {

                    d.username = ds1.Tables[0].Rows[0]["Name"].ToString();
                }
                else
                {
                    d.username = "NONE";
                }

                d.itemtypeid = itemdeliverycookies["itemtypeid"];
                if (ds.Tables[0].Rows.Count > 0)
                {

                    d.itemtype = ds.Tables[0].Rows[0]["itemtype"].ToString();
                }
                else
                {
                    d.itemtype = "";
                }

                d.Addresspick = itemdeliverycookies["Addresspick"];
                d.Latitudepick = itemdeliverycookies["Latitudepick"];
                d.Longitudepick = itemdeliverycookies["Longitudepick"];
                d.Citypick = itemdeliverycookies["Citypick"];
                d.Countrypick = itemdeliverycookies["Countrypick"];
                d.Name = itemdeliverycookies["Name"];
                d.Address = itemdeliverycookies["Address"];
                d.Latitude = itemdeliverycookies["Latitude"];
                d.Longitude = itemdeliverycookies["Longitude"];
                d.City = itemdeliverycookies["City"];
                d.Country = itemdeliverycookies["Country"];
                d.noofitem = itemdeliverycookies["noofitem"];
                d.width = itemdeliverycookies["width"];
                d.height = itemdeliverycookies["height"];
                d.length = itemdeliverycookies["length"];
                d.weight = itemdeliverycookies["weight"];
                Response.Cookies.Add(itemdeliverycookies);
            }
            else
            {
                //HttpCookie itemdeliverycookies = Request.Cookies["itemdeliverycookies"];
                itemdeliverycookies["Action"] = "itemform";
                itemdeliverycookies["Controller"] = "Services";
                Response.Cookies.Add(itemdeliverycookies);
                return Redirect(Url.Action("Login","Account"));
            }
            
           
            // d.Country = itemdeliverycookies["Country"];
            return View(d);
        }

        [HttpPost]
        public ActionResult ItemReview(Itemdelivery d)
        {
            HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
            HttpCookie itemdeliverycookies = Request.Cookies["itemdeliverycookies"];
            DataSet ds = dl.Inline_Process("select itemtype from MTR.M31_Itemtype where itemtypeid='" + @itemdeliverycookies["itemtypeid"] + "'");
            //Itemdelivery d = new Itemdelivery();
            if (loginCookie_Costoracle_USER != null)
            {
                d.userid = loginCookie_Costoracle_USER["UserId"];

            }
            else
            {
                d.userid = "0";
            }
            DataSet ds1 = dl.Inline_Process("select Name from USR.U01_User where userid='" + d.userid + "'");
            if (ds1.Tables[0].Rows.Count > 0)
            {

                d.username = ds1.Tables[0].Rows[0]["Name"].ToString();
            }
            else
            {
                d.username = "";
            }
            d.itemtypeid = itemdeliverycookies["itemtypeid"];
            d.itemtype = ds.Tables[0].Rows[0]["itemtype"].ToString();
            d.Addresspick = itemdeliverycookies["Addresspick"];
            d.Latitudepick = itemdeliverycookies["Latitudepick"];
            d.Longitudepick = itemdeliverycookies["Longitudepick"];
            d.Citypick = itemdeliverycookies["Citypick"];
            d.Countrypick = itemdeliverycookies["Countrypick"];
            d.Name = itemdeliverycookies["Name"];
            d.Address = itemdeliverycookies["Address"];
            d.Latitude = itemdeliverycookies["Latitude"];
            d.Longitude = itemdeliverycookies["Longitude"];
            d.City = itemdeliverycookies["City"];
            d.Country = itemdeliverycookies["Country"];
            d.noofitem = itemdeliverycookies["noofitem"];
            d.width = itemdeliverycookies["width"];
            d.height = itemdeliverycookies["height"];
            d.length = itemdeliverycookies["length"];
            d.weight = itemdeliverycookies["weight"];
            int i = Acdl.usp_SetItemDeliveryRequest(d);
            if (i > 0)
            {
                TempData["msg"] = "Data Posted Successfully!!";
            }
            else
            {
                TempData["msg"] = "Data Not Posted!!";
            }
            return Redirect(Url.Action("ItemResults", "Services"));
        }


        public ActionResult ItemResults()
        {
            HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];

            if (loginCookie_Costoracle_USER != null)
            {
                HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];
                // HttpCookie loginCookie_Costoracle_USER = Request.Cookies["loginCookie_Costoracle_USER"];
                if (PrevPagePageCookie != null)
                {
                    PrevPagePageCookie.Expires = DateTime.Now.AddHours(-1);
                    Response.Cookies.Add(PrevPagePageCookie);
                }


                Itemdelivery model = new Itemdelivery();
                List<Itemdelivery> mmlist = new List<Models.Itemdelivery>();
                //    getinsurancequotelist model = new Models.getinsurancequotelist();
                //model.InsuranceId = null;
                model.userid = loginCookie_Costoracle_USER["UserId"];
                DataSet ds = Acdl.usp_getItemDeliveryQuoteList(model);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        model = new Models.Itemdelivery();
                        model.bussinessname = ds.Tables[0].Rows[i]["businessname"].ToString();
                        model.Address = ds.Tables[0].Rows[i]["address"].ToString();
                        model.price = ds.Tables[0].Rows[i]["price"].ToString();
                        model.phone = ds.Tables[0].Rows[i]["phone"].ToString();
                        mmlist.Add(model);
                    }
                    ViewBag.quotelist = mmlist;
                }
                else
                {
                    model = new Models.Itemdelivery();
                    model.bussinessname = "NONE";
                    model.Address = "NONE";
                    model.price = "NONE";
                    model.phone = "NONE";
                    mmlist.Add(model);
                    ViewBag.quotelist = mmlist;
                }
                return View();
            }
            else
            {
                TempData["MustLogin"] = "You must login or register!";
                TempData["loginerror"] = "";

                HttpCookie PrevPagePageCookie = Request.Cookies["StrPrevPageCookie"];
                PrevPagePageCookie = new HttpCookie("StrPrevPageCookie");
                PrevPagePageCookie["PageURL"] = Url.Action("Results", "Services");
                Response.Cookies.Add(PrevPagePageCookie);

                return RedirectToAction("Login", "Account");
            }
        }

    }
}