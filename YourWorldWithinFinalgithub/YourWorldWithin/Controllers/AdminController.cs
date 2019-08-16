﻿using YourWorldWithin.Models;
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
                    pp.EmailVerified = Convert.ToBoolean(ds.Tables[0].Rows[i]["EmailVerified"].ToString());
                    string status = ds.Tables[0].Rows[i]["Status"].ToString();
                    if (status == "True")
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
            Fillsubscriptionplan();
            FillVideoCategory();
            Property pp = new Property();
            pp.VideoId = Id;
            DataSet ds = dl.usp_getVideo(pp);
            if (ds.Tables[0].Rows.Count > 0)
            {
                pp.VideoId = ds.Tables[0].Rows[0]["videoid"].ToString();
                pp.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                pp.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                pp.planid = ds.Tables[0].Rows[0]["subscriptionid"].ToString();
                pp.Tags = ds.Tables[0].Rows[0]["Tags"].ToString();
                pp.VideoFile = ds.Tables[0].Rows[0]["VideoFile"].ToString();
                pp.ImageFile = ds.Tables[0].Rows[0]["ImageFile"].ToString();
                pp.CategoryId = ds.Tables[0].Rows[0]["Categoryid"].ToString();
                pp.creationdate = ds.Tables[0].Rows[0]["createdatetime"].ToString();
                if (pp.VideoFile != null)
                {
                    ViewBag.image = "~/DataImages/Images/" + pp.ImageFile;
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
        public ActionResult EditVideo(string Id, Property p, HttpPostedFileBase ImageFile, HttpPostedFileBase VideoFile)
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

            return RedirectToAction("VideoList", "Admin"); ;

        }


        public ActionResult AudioList()
        {
            Property p = new Property();
            List<Property> plist = new List<Property>();
            DataSet ds = dl.usp_getAudio(p);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property pp = new Property();
                    pp.AudioId = ds.Tables[0].Rows[i]["Audioid"].ToString();
                    pp.Title = ds.Tables[0].Rows[i]["Title"].ToString();
                    pp.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                    pp.Tags = ds.Tables[0].Rows[i]["Tags"].ToString();
                    pp.AudioFile = ds.Tables[0].Rows[i]["AudioFile"].ToString();
                    pp.ImageFile = ds.Tables[0].Rows[i]["ImageFile"].ToString();
                    pp.creationdate = ds.Tables[0].Rows[i]["createdatetime"].ToString();
                    pp.CategoryId = ds.Tables[0].Rows[i]["Category"].ToString();
                    plist.Add(pp);
                }
                ViewBag.Audiolist = plist;
            }
            return View();
        }


        public ActionResult AddAudio()
        {
            Fillsubscriptionplan();
            FillVideoCategory();
            return View();
        }


        [HttpPost]
        public ActionResult AddAudio(Property p, HttpPostedFileBase ImageFile, HttpPostedFileBase AudioFile)
        {
            FillVideoCategory();
            Fillsubscriptionplan();
            string Image = "", Audio = "";
            try
            {
                if (ImageFile != null)
                {
                    Image = dl.NewSaveSingleImages("~/DataImages/Images/", ImageFile, Image);
                }
                if (AudioFile != null)
                {
                    Audio = dl.NewSaveSingleImages("~/DataImages/Audio/", AudioFile, Image);
                }
                p.ImageFile = Image;
                p.AudioFile = Audio;
                int i = 0;
                i = dl.usp_setAudio(p);
                if (i > 0)
                {
                    TempData["success"] = "Audio Uploaded Successfully..";
                }
                else
                {
                    TempData["error"] = "Audio Not Uploaded!";
                }

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Audio Not Uploaded!";
            }

            return View();

        }


        public ActionResult AddProduct()
        {
            Fillcategory();
           // Fillcategory();
            return View();
        }


        [HttpPost]
        public ActionResult AddProduct(Property p, HttpPostedFileBase ImageFile)
        {
            Fillcategory();
            string Image = "";
            try
            {
                if (ImageFile != null)
                {
                    Image = dl.NewSaveSingleImages("~/DataImages/Images/", ImageFile, Image);
                }
               
                p.ImageFile = Image;
                
               // int i = 0;
                if (p.subCategoryId != "")
                {
                    p.CategoryId = p.subCategoryId;
                }
                else
                {
                    p.CategoryId = p.CategoryId;
                }

                int i = dl.usp_setProduct(p);
                if (i > 0)
                {
                    TempData["success"] = "Product Uploaded Successfully..";
                }
                else
                {
                    TempData["error"] = "Product Not Uploaded!";
                }

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product Not Uploaded!";
            }

            return View();
        }

        public void Fillcategory()
        {
            Property p = new Models.Property();
            List<SelectListItem> categorylist = new List<SelectListItem>();
            DataSet ds = dl.usp_getProductCategory(p);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SelectListItem sl = new SelectListItem();
                    sl.Text = ds.Tables[0].Rows[i]["producrcategory"].ToString();
                    sl.Value = ds.Tables[0].Rows[i]["productcategoryid"].ToString();
                    categorylist.Add(sl);
                }
            }

            ViewBag.Categorylist = new SelectList(categorylist, "Value", "Text");
        }

        public JsonResult Fillsubcategory(string id)
        {
            Property p = new Models.Property();
            List<SelectListItem> categorylist = new List<SelectListItem>();
            p.CategoryId = id;
            DataSet ds = dl.usp_getProductCategory(p);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SelectListItem sl = new SelectListItem();
                    sl.Text = ds.Tables[0].Rows[i]["producrcategory"].ToString();
                    sl.Value = ds.Tables[0].Rows[i]["productcategoryid"].ToString();
                    categorylist.Add(sl);
                }
            }

            ViewBag.subCategorylist = new SelectList(categorylist, "Value", "Text");
            return Json(ViewBag.subCategorylist,JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProductList()
        {
            Property p = new Property();
            List<Property> plist = new List<Property>();
            p.productid = "0";
            p.CategoryId = "0";
            DataSet ds = dl.usp_getProduct(p);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property pp = new Property();
                    pp.productid = ds.Tables[0].Rows[i]["ProductId"].ToString();
                    pp.CategoryId = ds.Tables[0].Rows[i]["ProductCategoryId"].ToString();
                    pp.Description = ds.Tables[0].Rows[i]["ProductDescription"].ToString();
                    pp.subCategoryId = ds.Tables[0].Rows[i]["ProductCategory"].ToString();
                    pp.product = ds.Tables[0].Rows[i]["ProductName"].ToString();
                    pp.ImageFile = ds.Tables[0].Rows[i]["Image"].ToString();
                    pp.displayprice = ds.Tables[0].Rows[i]["DisplayPrice"].ToString();
                    pp.price = ds.Tables[0].Rows[i]["Price"].ToString();
                    pp.discount = ds.Tables[0].Rows[i]["Discount"].ToString();
                    plist.Add(pp);
                }
                ViewBag.ProductList = plist;
            }
            return View();
        }

    }
}
