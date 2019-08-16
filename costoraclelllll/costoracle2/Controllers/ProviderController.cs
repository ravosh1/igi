using Classes;
using costoracle2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace costoracle2.Controllers
{
    public class ProviderController : Controller
    {
        // GET: Provider
        AccountDataLayer dl = new AccountDataLayer();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult SetQuote()
        {

            HttpCookie loginCookie_Costoracle_PROVIDER = Request.Cookies["loginCookie_Costoracle_PROVIDER"];

            providermodellist model = new providermodellist();

            DataSet ds = new DataSet();

            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();
            DataSet ds5 = new DataSet();
            DataSet ds6 = new DataSet();
            DataSet ds7 = new DataSet();


            if (loginCookie_Costoracle_PROVIDER != null)
            {

                string ServiceProviderId = loginCookie_Costoracle_PROVIDER["UserId"];

                ds = dl.usp_GetVehicleValueGroup(ServiceProviderId);

                ds1 = dl.usp_GetVehicleMileageGroup(ServiceProviderId);

                ds2 = dl.usp_GetVehicleParking(ServiceProviderId);

                ds3 = dl.usp_GetVehicleCoverTypeFactor(ServiceProviderId);
                ds4 = dl.usp_GetVehicleTypeFactor(ServiceProviderId);
                ds5 = dl.usp_getLGAPrice(ServiceProviderId);
                ds6 = dl.usp_GetNoClaimYearFactor(ServiceProviderId);
                ds7 = dl.usp_GetDriverAgeGroup(ServiceProviderId);
            }


            if (ds.Tables[0].Rows.Count > 0)
            {


                ViewBag.vehiclevaluegroup = ds;

            }
           
            if (ds1.Tables[0].Rows.Count > 0)
            {


                ViewBag.vehiclemileagegroup = ds1;

            }
          
            if (ds2.Tables[0].Rows.Count > 0)
            {


                ViewBag.vehicleparking = ds2;

            }
        
            if (ds3.Tables[0].Rows.Count > 0)
            {


                ViewBag.vehiclecovertype = ds3;

            }
            
            if (ds4.Tables[0].Rows.Count > 0)
            {


                ViewBag.vehicletype = ds4;

            }
          
            if (ds5.Tables[0].Rows.Count > 0)
            {


                ViewBag.getlgaprice = ds5;

            }
            if (ds6.Tables[0].Rows.Count > 0)
            {


                ViewBag.ClaimYearFactor = ds6;

            }
            if (ds7.Tables[0].Rows.Count > 0)
            {


                ViewBag.GetDriverAgeGroup = ds7;

            }
            else
            {
                return Redirect(Url.Action("serviceproviderlogin", "Account"));
            }
           
                return View();
        }


        [HttpPost]
        public ActionResult SetQuote(providermodellist model)
        {

            HttpCookie loginCookie_Costoracle_PROVIDER = Request.Cookies["loginCookie_Costoracle_PROVIDER"];
            // string ServiceProviderId = "";
            if (loginCookie_Costoracle_PROVIDER != null)
            {
                string ServiceProviderId = loginCookie_Costoracle_PROVIDER["UserId"];

                string str1 = "";
                string str2 = "";
                string str3 = "";
                string str4 = "";
                string str5 = "";
                string str6 = "";
                string str7 = "";
                string str8 = "";

                foreach (var item in model.ServiceProvider)
                {
                    if (item.vehiclevaluegroupid != null)
                    {
                        if (str1 == "")
                        {

                            str1 += item.vehiclevaluegroupid + "," + item.ServiceProviderprice;
                        }
                        else
                        {
                            str1 += "|" + item.vehiclevaluegroupid + "," + item.ServiceProviderprice;

                        }
                    }
                    else if (item.vehiclemileagegroupid != null)
                    {
                        if (str2 == "")
                        {

                            str2 += item.vehiclemileagegroupid + "," + item.ServiceProvidercost;
                        }
                        else
                        {
                            str2 += "|" + item.vehiclemileagegroupid + "," + item.ServiceProvidercost;

                        }
                    }

                    else if (item.parkingid != null)
                    {
                        if (str3 == "")
                        {

                            str3 += item.parkingid + "," + item.ServiceProviderparkingprice;
                        }
                        else
                        {
                            str3 += "|" + item.parkingid + "," + item.ServiceProviderparkingprice;

                        }
                    }
                    else if (item.vehiclecovertypeid != null)
                    {
                        if (str4 == "")
                        {

                            str4 += item.vehiclecovertypeid + "," + item.ServiceProvidercoverFactor;
                        }
                        else
                        {
                            str4 += "|" + item.vehiclecovertypeid + "," + item.ServiceProvidercoverFactor;

                        }
                    }

                    else if (item.VehicleTypeId != null)
                    {
                        if (str5 == "")
                        {

                            str5 += item.VehicleTypeId + "," + item.ServiceProvidervtypeFactor;
                        }
                        else
                        {
                            str5 += "|" + item.VehicleTypeId + "," + item.ServiceProvidervtypeFactor;

                        }
                    }
                    else if (item.lgaid != null)
                    {
                        if (item.lgaprice != null)
                        {
                            if (str6 == "")
                            {

                                str6 += item.lgaid + "," + item.lgaprice;
                            }
                            else
                            {
                                str6 += "|" + item.lgaid + "," + item.lgaprice;

                            }
                        }
                        else
                        {
                           
                                str6 += "|" + item.lgaid + "," + 0;

                           
                        }

                    }

                    else if (item.noclaimyrid != null)
                    {
                        if (str7 == "")
                        {

                            str7 += item.noclaimyrid + "," + item.noclaimfactor;
                        }
                        else
                        {
                            str7 += "|" + item.noclaimyrid + "," + item.noclaimfactor;

                        }
                    }
                    else if (item.driverageid != null)
                    {
                        if (str8 == "")
                        {

                            str8 += item.driverageid + "," + item.driveragefactor;
                        }
                        else
                        {
                            str8 += "|" + item.driverageid + "," + item.driveragefactor;

                        }
                    }

                }
                if (str1 != "")
                {

                    dl.usp_SetVehicleValueGroup(ServiceProviderId, str1);
                }
                else if (str2 != "")
                {
                    dl.usp_SetVehicleMileageGroup(ServiceProviderId, str2);
                }
                else if (str3 != "")
                {
                    dl.usp_SetVehicleParking(ServiceProviderId, str3);
                }
                else if (str4 != "")
                {
                    dl.usp_SetVehicleCoverTypeFactor(ServiceProviderId, str4);
                }
                else if (str5 != "")
                {
                    dl.usp_SetVehicleTypeFactor(ServiceProviderId, str5);
                }
                else if (str6 != "")
                {
                    dl.usp_SetLGAPrice(ServiceProviderId, str6);
                }
                else if (str7 != "")
                {
                    dl.usp_SetNoClaimYearFactor(ServiceProviderId, str7);
                }
                else if (str8 != "")
                {
                    dl.usp_SetDriverAgeGroupFactor(ServiceProviderId, str8);
                }
            }
            SetQuote();
            return View();
        }


    }
}