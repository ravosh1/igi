using YourWorldWithin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YourWorldWithin.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

       Datalayer dl = new Datalayer();
     //   EncryptDecrypt enc = new EncryptDecrypt();

        public ActionResult Index()
        {
           
            return View();
        }
        public void FillVideoCategory()
        {
            List<SelectListItem> Catlist = new List<SelectListItem>();
            DataSet ds = dl.Inline_Process("select * from [dbo].[M10_Category]");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SelectListItem sl = new SelectListItem();
                    sl.Text = ds.Tables[0].Rows[i]["Category"].ToString();
                    sl.Value = ds.Tables[0].Rows[i]["CategoryId"].ToString();
                    Catlist.Add(sl);
                }
            }

            ViewBag.VideoCategorylist = new SelectList(Catlist, "Value", "Text");
        }
        public void Fillsubscriptionplan()
        {
            Property p = new Models.Property();
            List<SelectListItem> subscriptionlist = new List<SelectListItem>();
            DataSet ds = dl.usp_getSubscription(p);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SelectListItem sl = new SelectListItem();
                    sl.Text = ds.Tables[0].Rows[i]["subscriptionname"].ToString();
                    sl.Value = ds.Tables[0].Rows[i]["subscriptionid"].ToString();
                    subscriptionlist.Add(sl);
                }
            }

            ViewBag.subscriptionplanlist = new SelectList(subscriptionlist, "Value", "Text");
        }
        
        public ActionResult UserList()
        {
            Property p = new Property();
            List<Property> plist = new List<Property>();
            DataSet ds = dl.usp_getUser(p);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property pp = new Property();
                    pp.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                    pp.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    pp.Email = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    pp.Phone = ds.Tables[0].Rows[i]["Phone"].ToString();
                    pp.EmailVerified =Convert.ToBoolean(ds.Tables[0].Rows[i]["EmailVerified"].ToString());
                   string status = ds.Tables[0].Rows[i]["Status"].ToString();
                    if(status=="True")
                    {
                        pp.Status = "Active";
                    }
                    else
                    {
                        pp.Status = "InActive";
                    }
                    plist.Add(pp);
                }

                ViewBag.UserList = plist;
            }

            return View();
        }
        public ActionResult EditVideo(string Id)
        {
            FillVideoCategory();
            Property pp = new Property();
            pp.VideoId = Id;
            DataSet ds = dl.usp_getVideo(pp);
            if (ds.Tables[0].Rows.Count > 0)
            {                    
                    pp.VideoId = ds.Tables[0].Rows[0]["videoid"].ToString();
                    pp.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    pp.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    pp.Tags = ds.Tables[0].Rows[0]["Tags"].ToString();
                    pp.VideoFile = ds.Tables[0].Rows[0]["VideoFile"].ToString();
                    pp.ImageFile = ds.Tables[0].Rows[0]["ImageFile"].ToString();
                pp.CategoryId = ds.Tables[0].Rows[0]["Category"].ToString();
                pp.creationdate = ds.Tables[0].Rows[0]["createdatetime"].ToString();
                         if(pp.VideoFile!=null)
                {
                    ViewBag.image = "~/DataImages/Images/"+ pp.ImageFile;
                }
                else
                {
                    ViewBag.image = "~/Content/defaultvideo.png";
                }
            }
            
            return View(pp);
        }
        public ActionResult VideoList()
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
                        pp.CategoryId = ds.Tables[0].Rows[i]["Category"].ToString();

                        plist.Add(pp);
                    }

                    ViewBag.videolist = plist;
                }
          
           
            return View();
        }


        public ActionResult AddVideo()
        {
            Fillsubscriptionplan();
            FillVideoCategory();
            return View();
        }


        [HttpPost]
        public ActionResult AddVideo(Property p, HttpPostedFileBase ImageFile, HttpPostedFileBase VideoFile)
        {
            FillVideoCategory();
            Fillsubscriptionplan();
            string Image = "", Video = "";
            try
            {
                if (ImageFile != null)
                {
                    Image = dl.NewSaveSingleImages("~/DataImages/Images/", ImageFile, Image);
                }
                if (VideoFile != null)
                {
                    Video = dl.NewSaveSingleImages("~/DataImages/Videos/", VideoFile, Image);
                }
                p.ImageFile = Image;
                p.VideoFile = Video;
                int i = 0;
                i = dl.usp_setVideo(p);
                if (i > 0)
                {
                    TempData["success"] = "Video Uploaded Successfully..";
                }
                else
                {
                    TempData["error"] = "Video Not Uploaded!";
                }
             
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Video Not Uploaded!"; 
            }
            
            return View();

        }
        [HttpPost]
        public ActionResult EditVideo(string Id,Property p, HttpPostedFileBase ImageFile, HttpPostedFileBase VideoFile)
        {
            string Image = "", Video = "";
            try
            {
                if (ImageFile != null)
                {
                    Image = dl.NewSaveSingleImages("~/DataImages/Images/", ImageFile, Image);
                }
                if (VideoFile != null)
                {
                    Video = dl.NewSaveSingleImages("~/DataImages/Videos/", VideoFile, Image);
                }
                p.VideoId = Id;
                p.ImageFile = Image;
                p.VideoFile = Video;
                int i = 0;
                i = dl.usp_setVideo(p);
                if (i > 0)
                {
                    TempData["success"] = "Video Uploaded Successfully..";
                }
                else
                {
                    TempData["error"] = "Video Not Uploaded!";
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Video Not Uploaded!";
            }

            return RedirectToAction("VideoList","Admin"); ;

        }
        //public ActionResult vehicle()
        //{
        //    Property p = new Models.Property();
        //    DataSet ds = new DataSet();
        // //   p.donorid = null;
        // //   p.vehicleid = null;
        //    ds = dl.usp_getVehicleDonation(p);
        //    ViewBag.vehicledonation = ds;
        //    return View();
        //}

        //public ActionResult donordetails(string id,string id1,string id2)
        //{
        //    Property p = new Models.Property();
        //    DataSet ds = new DataSet();
        //       p.DonorId = id1;
        //    if (id2 == "vehicle")
        //    {
        //        p.VehicleId = id;
        //    }
        //    if (id2 == "boat")
        //    {
        //        p.BoatId = id;

        //    }
        //    if (id2 == "property") { 
        //        p.PropertyId = id;
        //    }
        //    if (id2 == "electronics")
        //    {
        //        p.ElectronicsId = id;
        //    }
        //    ds = dl.usp_getDonorDonation(p);
        //    // ViewBag.donordetails = ds;
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        p = new Property();
        //        p.DonorId = ds.Tables[0].Rows[0]["donorid"].ToString();
        //        p.FName = ds.Tables[0].Rows[0]["name"].ToString();
        //        p.Address = ds.Tables[0].Rows[0]["Address"].ToString();
        //        p.City = ds.Tables[0].Rows[0]["City"].ToString();
        //        p.Zip = ds.Tables[0].Rows[0]["zip"].ToString();
        //        p.PickUpAddress = ds.Tables[0].Rows[0]["pickupaddress"].ToString();
        //        p.PickUpCity = ds.Tables[0].Rows[0]["pickupcity"].ToString();
        //      //  p.vehicletype = ds.Tables[0].Rows[0]["vehicletype"].ToString();
        //        p.PickUpZip = ds.Tables[0].Rows[0]["pickupzip"].ToString();
        //        p.Mobile = ds.Tables[0].Rows[0]["mobile"].ToString();
        //        p.Phone = ds.Tables[0].Rows[0]["phone"].ToString();
        //        p.EmailID = ds.Tables[0].Rows[0]["email"].ToString();
        //        p.VehicleIdNumber = ds.Tables[0].Rows[0]["vehicleidnumber"].ToString();
        //        p.UseEmail = Convert.ToBoolean(ds.Tables[0].Rows[0]["useemail"].ToString());
        //        p.UseMobileNumber = Convert.ToBoolean(ds.Tables[0].Rows[0]["usemobilenumber"].ToString());
        //        p.HowYouHearAbout = ds.Tables[0].Rows[0]["howyouhearabout"].ToString();
        //    }

        //    return View(p);
        //}


        //public ActionResult boat()
        //{
        //    Property p = new Models.Property();
        //    DataSet ds = new DataSet();
        //    //   p.donorid = null;
        //    //   p.vehicleid = null;
        //    ds = dl.usp_getBoatDonation(p);
        //    ViewBag.boatdonation = ds;
        //    return View();
        //}


        //public ActionResult electronics()
        //{
        //    Property p = new Models.Property();
        //    DataSet ds = new DataSet();
        //    //   p.donorid = null;
        //    //   p.vehicleid = null;
        //    ds = dl.usp_getElectronicsDonation(p);
        //    ViewBag.electronicsdonation = ds;
        //    return View();
        //}


        //public ActionResult property()
        //{
        //    Property p = new Models.Property();
        //    DataSet ds = new DataSet();
        //      // p.donorid = null;
        //    //   p.vehicleid = null;
        //    ds = dl.usp_getPropertyDonation(p);
        //    ViewBag.PropertyDonation = ds;
        //    return View();
        //}



        ////===Get Volunter List

        //public ActionResult GetVolunterList()
        //{
        //    Property.VolunterJoinModel p = new Property.VolunterJoinModel();
        //    DataSet ds = new DataSet();
        //    //   p.donorid = null;
        //    //   p.vehicleid = null;
        //    ds = dl.usp_getVolunteer(p);
        //    ViewBag.GetVolunterList = ds;
        //    return View();

        //}
        //public ActionResult Volunterdetails(string id)
        //{
        //    Property.VolunterJoinModel p = new Property.VolunterJoinModel();
        //    DataSet ds = new DataSet();
        //       p.VolunteerId = id;
        //    //   p.vehicleid = null;
        //    ds = dl.usp_getVolunteer(p);

        //    if(ds.Tables[0].Rows.Count>0)
        //    {
        //        p.FName = ds.Tables[0].Rows[0]["FName"].ToString();
        //        p.LName = ds.Tables[0].Rows[0]["LName"].ToString();
        //        p.Email = ds.Tables[0].Rows[0]["Email"].ToString();
        //        p.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
        //        p.Address = ds.Tables[0].Rows[0]["Address"].ToString();
        //        p.State = ds.Tables[0].Rows[0]["State"].ToString();
        //        p.City = ds.Tables[0].Rows[0]["City"].ToString();
        //        p.Zip = ds.Tables[0].Rows[0]["Zip"].ToString();
        //        p.VolunteeringFor = ds.Tables[0].Rows[0]["VolunteeringFor"].ToString();
        //        p.AccessCar = ds.Tables[0].Rows[0]["AccessCar"].ToString();
        //        p.DaysAvailabl = ds.Tables[0].Rows[0]["DaysAvailabl"].ToString();
        //        p.DescribeYourSelf = ds.Tables[0].Rows[0]["Details"].ToString();

        //    }

        //    return View(p);
        //}

        ////Contact us
        //public ActionResult GetContactList()
        //{
        //    Property.ContactUs p = new Property.ContactUs();
        //    DataSet ds = new DataSet();
        //    //   p.donorid = null;
        //    //   p.vehicleid = null;
        //    ds = dl.usp_getContact(p);
        //    ViewBag.GetContactList = ds;
        //    return View();
        //}




    }
}
