using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace AMSS.Models
{
    public class Datalayer
    {
        public static byte[] pImage;

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


        public DataSet Inline_Process(String Query)
        {

            Property p = new Property();
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
                        string pathnew = HttpContext.Current.Server.MapPath(path);
                        f.SaveAs(pathnew);
                        Image imgOriginal = Image.FromFile(pathnew);
                        Image imgActual = ScaleBySize(imgOriginal);
                        imgOriginal.Dispose();
                        imgActual.Save(pathnew);
                        imgActual.Dispose();
                        if (File.Exists(pathnew))
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


        public static Image ScaleBySize(Image imgPhoto)
        {
            float sourcewidth = imgPhoto.Width;
            float sourceheight = imgPhoto.Height;
            float destwidth = 0;
            float destheight = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;


            destwidth = 1349;
            destheight = 667;


            Bitmap bmphoto = new Bitmap((int)destwidth, (int)destheight, PixelFormat.Format32bppPArgb);
            bmphoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            Graphics grphoto = Graphics.FromImage(bmphoto);
            grphoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grphoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destwidth, (int)destheight),
                new Rectangle(sourceX, sourceY, (int)sourcewidth, (int)sourceheight),
                GraphicsUnit.Pixel);

            grphoto.Dispose();
            return bmphoto;
        }
        //----------------------Data Access Layer Work---------------------------

        // EncryptDecrypt enc = new EncryptDecrypt();

        public DataSet AdmingetLogin(Property p)
        {
            try
            {
                string[] paraname = { "@UserId", "@Password" };
                string[] paravalue = { p.UserName, p.Password };
                return Ds_Process("spp_admin_Login", paraname, paravalue);
            }
            catch
            {
                throw;
            }
        }

        public DataSet changePassword_Admin(Property model)
        {
            try
            {
                string[] paraname = { "@UserId", "@Password", "@newPass", "@Confirm_Password" };
                string[] paravalue = { model.UserID, model.Password, model.NewPassword, model.ConfPassword };
                return Ds_Process("adminchangePassword", paraname, paravalue);
            }
            catch
            {

                throw;
            }
        }


        public int AdminEditProfile(Property model)
        {
            try
            {
                string[] paraname = { "@UserId", "@Password", "@UserType", "@EmailId", "@ContactNo", "@Photo", "@Name", "@Address", "@City" };
                string[] paravalue = { model.UserID, model.Password, model.UserType, model.EmailID, model.Contact, model.ImgURL, model.FirstName, model.Address, model.City };

                return Int_Process("SP_ADMIN_EDIT", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int usp_setAssignmentList(Property model)
        {
            try
            {
                string[] paraname = { "@UserId", "@AssignmentId" , "@AssignmentName" };
                string[] paravalue = { model.UserID, model.assignmentID,model.assignmentname };

                return Int_Process("usp_setAssignmentList", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int usp_setStudent(Property model)
        {
            try
            {
                string[] paraname = { "@StudentId", "@Name" };
                string[] paravalue = { model.studentid, model.FullName };

                return Int_Process("usp_setStudent", paraname, paravalue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public DataSet usp_getQuestion(Property p)
        {
            try
            {
                string[] paraname = { "@AssignmentId" };
                string[] paravalue = { p.assignmentID };
                return Ds_Process("usp_getQuestion", paraname, paravalue);
            }
            catch
            {
                throw;
            }
        }





        public DataSet usp_getAssignmentList(Property p)
        {
            try
            {
                string[] paraname = { "@UserId", "@AssignmentId" };
                string[] paravalue = { p.UserID, p.assignmentID };
                return Ds_Process("usp_getAssignmentList", paraname, paravalue);
            }
            catch
            {
                throw;
            }
        }

        public DataSet usp_getStudent(Property p)
        {
            try
            {
                string[] paraname = { "@StudentId" };
                string[] paravalue = { p.studentid };
                return Ds_Process("usp_getStudent", paraname, paravalue);
            }
            catch
            {
                throw;
            }
        }

        //[TRN].[]
    }
}