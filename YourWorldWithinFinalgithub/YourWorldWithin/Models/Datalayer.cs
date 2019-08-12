using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using YourWorldWithin.Models;

namespace YourWorldWithin.Models
{
    public class Datalayer
    {
        Property p = new Property();
        public static byte[] pImage;
        protected DataSet Executeproc(string storedproc, string[] paraname, string[] paravalue)
        {
            try
            {
                SqlDataAdapter da;
                DataSet ds;
                SqlConnection cn = new SqlConnection(p.Con);
                SqlCommand cmd = new SqlCommand(storedproc, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paraname.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paraname[i], paravalue[i]);
                }
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                cn.Dispose();
                cn.Close();
                return ds;
            }
            catch
            {
                throw;
            }
        }
        protected int ExecuteNonproc(string storedproc, string[] paraname, string[] paravalue)
        {
            try
            {
                int result = 0;
                SqlConnection cn = new SqlConnection(p.Con);
                SqlCommand cmd = new SqlCommand(storedproc, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paraname.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paraname[i], paravalue[i]);
                }
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cn.Open();
                result = cmd.ExecuteNonQuery();
                cn.Dispose();
                cn.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet Inline_Process(String Query)
        {


            SqlConnection con = new SqlConnection(p.Con);
            SqlCommand cmd = new SqlCommand(Query, con);


            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            da.Dispose();
            con.Dispose();
            return ds;

        }
        public int Inline_ExecuteNonQry(String Query)
        {
            SqlConnection con = new SqlConnection(p.Con);
            SqlCommand cmd = new SqlCommand(Query, con);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Dispose();
            con.Close();
            return result;
        }
        public string SaveSingleImages(string directory, HttpPostedFileBase f)
        {
            string path = "", retpath = "";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
                }

                if (f != null)
                {
                    if (f.ContentLength > 0)
                    {
                        int fCount = 0;
                        fCount = Directory.GetFiles(HttpContext.Current.Server.MapPath(directory), "*", SearchOption.AllDirectories).Length;
                        fCount++;
                        path = directory + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + fCount.ToString() + Path.GetExtension(f.FileName);

                        f.SaveAs(HttpContext.Current.Server.MapPath(path));
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = path;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return retpath;
        }
        public bool CheckImageExtention(string ext)
        {
            bool flag = false;
            string[] extensionlist = { "jpg", "jpeg", "bmp", "png" };
            for (int i = 0; i < extensionlist.Length; i++)
            {
                if (ext.ToString().ToLower() == extensionlist[i])
                {
                    flag = true;
                }
            }
            return flag;
        }
        public bool CheckResumeExtention(string ext)
        {
            bool resume = false;
            string[] extensionlist = { "txt", "pdf", "doc", "docx" };
            for (int i = 0; i < extensionlist.Length; i++)
            {
                if (ext.ToString().ToLower() == extensionlist[i])
                {
                    resume = true;
                }
            }
            return resume;
        }
        //Page wise Inline query-------------
        public DataSet Inline_Process(String Query, string OrderBy, string AscDesc, long Page, long PageSize)
        {
            string[] paraname = { "@Qry", "@OrderBy", "@ASCDESC", "@Page", "@rowsPerPage" };
            string[] paravalue = { Query, OrderBy, AscDesc, Page.ToString(), PageSize.ToString() };
            DataSet ds = Executeproc("ExecuteQueryPageWise", paraname, paravalue);
            return ds;
        }
        public int Int_Process(String Storp, string[] parametername, string[] parametervalue)
        {
            int a = 0;
            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            SqlCommand cmd = new SqlCommand(Storp, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < parametername.Length; i++)
            {
                if (parametername[i] == "@img")
                {
                    cmd.Parameters.AddWithValue(parametername[i], pImage);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parametername[i], parametervalue[i]);
                }
            }
            con.Open();

            a = cmd.ExecuteNonQuery();
            con.Dispose();
            return a;
        }
        public DataSet Ds_Process(String Storp, string[] parametername, string[] parametervalue)
        {
            try
            {
                Property p = new Property();
                SqlConnection con = new SqlConnection(p.Con);
                SqlCommand cmd = new SqlCommand(Storp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < parametername.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parametername[i], parametervalue[i]);
                }
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                con.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                DataSet ds = null;
                return ds;
            }

        }
        public DataSet MyDs_Process(String Storp)
        {

            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            SqlCommand cmd = new SqlCommand(Storp, con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            da.Dispose();
            con.Dispose();
            return ds;

        }

        //----------------------Data Access Layer Work---------------------------

        // EncryptDecrypt enc = new EncryptDecrypt();

        //public DataSet FETCH_LOGIN_DETAILS(Property p)
        //{
        //    try
        //    {
        //        string[] paname = { "@EmailID", "@Password" };
        //        string[] pvalue = { p.EmailID, p.Password };
        //        return Ds_Process("FETCH_LOGIN_DETAILS", paname, pvalue);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public DataSet usp_getElectronicsDonation(Property p)
        //{
        //    try
        //    {
        //        string[] paname = { "@ElectronicsId", "@DonorId", "@ElectronicsTypeId" };
        //        string[] pvalue = { p.ElectronicsId, p.DonorId, p.ElectronicsTypeId };
        //        return Ds_Process("usp_getElectronicsDonation", paname, pvalue);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        //public DataSet usp_getDonorDonation(Property p)
        //{
        //    try
        //    {
        //        string[] paname = { "@VehicleId", "@DonorId", "@PropertyId", "@BoatId", "@ElectronicsId" };
        //        string[] pvalue = { p.VehicleId, p.DonorId, p.PropertyId, p.BoatId, p.ElectronicsId };
        //        return Ds_Process("usp_getDonorDonation", paname, pvalue);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public DataSet usp_getVideo(Property p)
        {
            try
            {
                string[] paname = { "@VideoId", "@Title"};
                string[] pvalue = { p.VideoId, p.Title};
                return Ds_Process("usp_getVideo", paname, pvalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataSet usp_getUser(Property p)
        {
            try
            {
                string[] paname = { "@UserId" };
                string[] pvalue = { p.UserId};
                return Ds_Process("usp_getUser", paname, pvalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public DataSet usp_getSubscription(Property p)
        {
            try
            {
                string[] paname = { "@SubscriptionId" };
                string[] pvalue = { p.planid };
                return Ds_Process("usp_getSubscription", paname, pvalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_setVideo(Property p)
        {
            try
            {
                string[] paname = { "@VideoId", "@Title"
                , "@Description", "@Tags","@VideoFile", "@ImageFile", "@CategoryId"};
                string[] pvalue = { p.VideoId, p.Title, p.Description, p.Tags, p.VideoFile, p.ImageFile, p.CategoryId };
                return Int_Process("usp_setVideo", paname, pvalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int usp_SetUser(Property p)
        {
            try
            {
                string[] paname = { "@UserId", "@Name"
                , "@EmailId", "@Password","@EmailVerified", "@Phone"};
                string[] pvalue = { p.UserId, p.Name, p.Email, p.Password, Convert.ToBoolean(p.EmailVerified).ToString(), p.Phone };
                return Int_Process("usp_SetUser", paname, pvalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string NewSaveSingleImages(string directory, HttpPostedFileBase f, string oldfile)
        {
            string path = "", retpath = "";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                {

                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
                }
                if (f != null)
                {
                    try
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath(directory + oldfile)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(directory + oldfile));
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (f.ContentLength > 0)
                    {
                        int fCount = 0;
                        fCount =
                            Directory.GetFiles(HttpContext.Current.Server.MapPath(directory), "*",
                                SearchOption.AllDirectories).Length;
                        fCount++;
                        oldfile = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + fCount.ToString() +
                                  Path.GetExtension(f.FileName);
                        path = directory + oldfile;

                        f.SaveAs(HttpContext.Current.Server.MapPath(path));
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = oldfile;
                        }
                    }
                }
                else
                {
                    if (oldfile != "")
                    {
                        path = directory + oldfile;
                        if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                        {
                            retpath = oldfile;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return retpath;
        }

        ////Admin=====


    }
}