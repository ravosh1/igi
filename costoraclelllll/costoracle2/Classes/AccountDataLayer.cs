using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using costoracle2.Models;

namespace Classes
{
    public class AccountDataLayer : DataLayerFunctions
    {


        //-----------------------Check Email Exists--------------------------
        public bool checkExists(string Table, string Column, string Value)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        public bool checkUserEmailExists(string email)
        {
            try
            {
                DataSet ds = Inline_Process("select EmailId from  usr.U01_User where EmailId='" + email + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        //-------------End Check Email Exists-------------------



        public int UserRegistration(RegisterModel model)
        {
            try
            {
                string[] paraname = { "@UserId", "@Name", "@EmailId", "@Phone", "@Password", "@EmailVerified" };
                string[] paravalue = { model.Id, model.Name, model.EmailId, model.PhoneNo, model.Password, model.EmailVerified };
                return ExecuteNonproc("usp_SetUser", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public int VehicleInfo(InsuranceModel model)
        //{
        //    try
        //    {
        //        string[] paraname = { "@VehicleId", "@UserId", "@ProductName", "@VehicleRegNo", "@Make", "@VehicleAge", "@Model", "@Transmission", "@FuelType", "@Knw_Reg_No" };
        //        string[] paravalue = { model.VehicleId, model.UserId, model.ProductName, model.VehicleRegNo, model.Make, model.VehicleAge, model.Models, model.Transmission, model.FuelType, model.Knw_Reg_No };
        //        return ExecuteNonproc("INSERT_UPDATE_VEHICLEDETAILS", paraname, paravalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public int AddVehicleInfo(AddvehicleinfoModel model)
        {
            try
            {
                string[] paraname = { "@VehicleId", "@UserId", "@carkept", "@address", "@mileage", "@Mileageunit" };
                string[] paravalue = { model.VehicleId, model.UserId, model.carkept, model.LGA_Address, model.Mileage, model.Mileunit };
                return ExecuteNonproc("INSERT_UPDATE_ADDVEHICLEINFO", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public int INSERT_UPDATE_YOURINFORMATION(yourinfoModel model)
        //{
        //    try
        //    {
        //        string[] paraname = { "@VehicleId", "@UserId", "@fname", "@lname", "@Agedriver" };
        //        string[] paravalue = { model.VehicleId, model.UserId, model.fname, model.lname, model.agedriver };
        //        return ExecuteNonproc("INSERT_UPDATE_YOURINFORMATION", paraname, paravalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public int INSERT_UPDATE_COVER_INFORMATION(CoverinfoModel model)
        //{
        //    try
        //    {
        //        string[] paraname = { "@VehicleId", "@UserId", "@coverlevel", "@noclaimyr", "@voluntaryexcess", "@startdate", "@insuranceduration" };
        //        string[] paravalue = { model.VehicleId, model.UserId, model.coverlevel, model.noclaimyr, model.voluntaryexcess,model.startdate,model.insuranceduration };
        //        return ExecuteNonproc("INSERT_UPDATE_COVER_INFORMATION", paraname, paravalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public int INSERT_UPDATE_SERVICE_PROVIDER_REGISTER(ServiceprovideREGrModel model)
        {
            try
            {
                string[] paraname = { "@Id", "@businessname", "@maincontactname", "@contactno", "@address", "@website", "@division", "@rcno", "@subcattag", "@email", "@password" };
                string[] paravalue = { model.Id, model.businessname, model.maincontactname, model.contactno, model.address, model.website, model.division, model.Rcno, model.subcattag, model.EmailId, model.Password };
                return ExecuteNonproc("INSERT_UPDATE_SERVICE_PROVIDER_REGISTER", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet USP_GETLGA(getlga model)
        {
            try
            {
                string[] paraname = { "@LGAId", "@StateId"};
                string[] paravalue = { model.lgaId, model.stateId };
                return Executeproc("USP_GETLGA", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet USP_GETSTATE(getstate model)
        {
            try
            {
                string[] paraname = { "@StateId", "@CountryId" };
                string[] paravalue = { model.stateId, model.countryid };
                return Executeproc("USP_GETSTATE", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_getVehicleType(getVehicleType gvt)
        {
            try
            {
                string[] paraname = { "@VehicalTypeId"};
                string[] paravalue = { gvt.VehicalTypeId };
                return Executeproc("usp_getVehicleType", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataSet usp_UserLogin(RegisterModel model)
        {
            try
            {
                string[] paraname = { "@EmailId", "@Password" };
                string[] paravalue = { model.EmailId, model.Password };
                return Executeproc("usp_UserLogin", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_ProviderLogin(ServiceprovideREGrModel model)
        {
            try
            {
                string[] paraname = { "@EmailId", "@Password" };
                string[] paravalue = { model.EmailId, model.Password };
                return Executeproc("usp_ProviderLogin", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_getParking(getParking gp)
        {
            try
            {
                string[] paraname = { "@parkingId" };
                string[] paravalue = { gp.ParkingId };
                return Executeproc("usp_getParking", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_getCoverlevel(getCoverLevel gcl)
        {
            try
            {
                string[] paraname = { "@CoverTypeId" };
                string[] paravalue = { gcl.CoverLevelId };
                return Executeproc("usp_getCoverType", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public DataSet FETCH_CONDITIONAL_QUERY(ListinfoModel model)
        {
            try
            {
                string[] paraname = { "@condition1", "@condition2", "@condition3", "@ontable" };
                string[] paravalue = { model.condition1, model.condition2, model.condition3, model.ontable };
                return Executeproc("FETCH_CONDITIONAL_QUERY", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_GetInsurenceRequest(InsuranceRequestModel model)
        {
            try
            {
                string[] paraname = { "@UserId", "@VehicleTypeId", "@RegistrationNumber",  "@MakeId", "@ModelId", "@VehicleAge", "@CarValue", "@TransmissionType", "@FuelType", "@ParkingId", "@StateId", "@LGAId", "@Mileage", "@MileageUnit" ,"@DriverFirstName", "DriverLastName", "DriverDOB", "CoverStartDate", "CoverLableId", "Duration", "NoClaimYearId", "VoluntaryExcess"};
                string[] paravalue = { model.UserId, model.VehicleTypeId, model.RegistrationNumber, model.MakeId, model.ModelId, model.VehicleAge,model.CarValue,model.TransmissionType, model.FuelType, model.ParkingId,model.StateId, model.LGAId,model.Mileage,model.Mileunit,model.DriverFirstName,model.DriverLastName, model.DriverDOB, model.CoverStartDate, model.CoverLableId,model.Duration,model.NoClaimYearId,model.VoluntaryExcess };
                return ExecuteNonproc("usp_SetInsurenceRequest", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_getInsuranceQuoteList(ServiceprovideREGrModel model)
        {
            try
            {
                string[] paraname = { "@InsuranceId", "@UserId", "@ServiceProviderId" };
                string[] paravalue = { model.InsuranceId, model.UserId, model.ServiceProviderId };
                return Executeproc("usp_getInsuranceQuoteList", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_GetVehicleValueGroup(string id)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" };
                string[] paravalue = { id};
                return Executeproc("usp_GetVehicleValueGroup", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_GetVehicleMileageGroup(string id)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" };
                string[] paravalue = { id };
                return Executeproc("usp_GetVehicleMileageGroup", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public DataSet usp_GetVehicleParking(string id)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" };
                string[] paravalue = { id };
                return Executeproc("usp_GetVehicleParking", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public DataSet usp_GetVehicleCoverTypeFactor(string id)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" };
                string[] paravalue = { id };
                return Executeproc("usp_GetVehicleCoverTypeFactor", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public DataSet usp_GetVehicleTypeFactor(string id)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" };
                string[] paravalue = { id };
                return Executeproc("usp_GetVehicleTypeFactor", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_getLGAPrice(string id)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" };
                string[] paravalue = { id };
                return Executeproc("usp_getLGAPrice", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_GetNoClaimYearFactor(string id)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" };
                string[] paravalue = { id };
                return Executeproc("usp_GetNoClaimYearFactor", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_GetDriverAgeGroup(string id)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" };
                string[] paravalue = { id };
                return Executeproc("usp_GetDriverAgeGroup", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int usp_SetVehicleValueGroup(string id,string str)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId" , "@VehicleGroupId_Price" };
                string[] paravalue = { id,str};
                return ExecuteNonproc("usp_SetVehicleValueGroup", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_SetVehicleMileageGroup(string id, string str)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId", "@VehicleMileageId_Price" };
                string[] paravalue = { id, str };
                return ExecuteNonproc("usp_SetVehicleMileageGroup", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_SetVehicleParking(string id, string str)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId", "@ParkingId_Price" };
                string[] paravalue = { id, str };
                return ExecuteNonproc("usp_SetVehicleParking", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_SetVehicleCoverTypeFactor(string id, string str)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId", "@CoverTypeId_Price" };
                string[] paravalue = { id, str };
                return ExecuteNonproc("usp_SetVehicleCoverTypeFactor", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_SetVehicleTypeFactor(string id, string str)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId", "@VehicleTypeId_Price" };
                string[] paravalue = { id, str };
                return ExecuteNonproc("usp_SetVehicleTypeFactor", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_SetLGAPrice(string id, string str)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId", "@LGAId_Price" };
                string[] paravalue = { id, str };
                return ExecuteNonproc("usp_SetLGAPrice", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_SetNoClaimYearFactor(string id, string str)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId", "@NoClaimYearId_Price" };
                string[] paravalue = { id, str };
                return ExecuteNonproc("usp_SetNoClaimYearFactor", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_SetDriverAgeGroupFactor(string id, string str)
        {
            try
            {
                string[] paraname = { "@ServiceProviderId", "@DriverAgeGroupId_Price" };
                string[] paravalue = { id, str };
                return ExecuteNonproc("usp_SetDriverAgeGroupFactor", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet usp_getMake(InsuranceRequestModel model)
        {
            try
            {
                string[] paraname = { "@MakeId" };
                string[] paravalue = { model.Make };
                return Executeproc("usp_getMake", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public DataSet usp_getModel(InsuranceRequestModel model)
        {
            try
            {
                string[] paraname = { "@MakeId", "@ModelId" };
                string[] paravalue = { model.Make ,model.Models};
                return Executeproc("usp_getModel", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int usp_SetItemDeliveryRequest(Itemdelivery model)
        {
            try
            {
                string[] paraname = { "@UserId", "@ItemTypeId", "@ToName", "@ToAddress", "@ToLat", "@ToLong", "@ToCity", "@ToCountry", "@FromAddress", "@FromLat", "@FromLong", "@FromCity", "@FromCountry", "@NumberOfItem", "@Weight", "@Width", "@Hight", "@Length"};
                string[] paravalue = { model.userid,model.itemtypeid, model.Name,model.Address,model.Latitude,model.Longitude,model.City,model.Country,model.Addresspick,model.Latitudepick,model.Longitudepick,model.Citypick,model.Countrypick, model.noofitem,model.weight,model.width,model.height,model.length};
                return ExecuteNonproc("usp_SetItemDeliveryRequest", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}