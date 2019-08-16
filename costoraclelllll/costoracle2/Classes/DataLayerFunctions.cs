using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Classes
{
    public class DataLayerFunctions
    {
        private Connection cnn = new Connection();
        protected DataSet Executeproc(string storedproc, string[] paraname, string[] paravalue)
        {
            try
            {
                SqlDataAdapter da;
                DataSet ds;
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
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
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
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
            catch(Exception ex)
            {
                throw;
            }
        }
        public DataSet Inline_Process(String Query)
        {


            SqlConnection con = new SqlConnection(cnn.DbConnectionString);
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
            SqlConnection con = new SqlConnection(cnn.DbConnectionString);
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
        public string SaveSingleImages(string directory,HttpPostedFileBase f)
        {
            string path = "",retpath="";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
                }

            if(f!=null)
            {
                if(f.ContentLength>0)
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
            string[] extensionlist = {"jpg","jpeg","bmp","png" };
            for (int i = 0; i < extensionlist.Length;i++ )
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
        public DataSet Inline_Process(String Query,string OrderBy,string AscDesc,long Page,long PageSize)
        {
            string[] paraname = { "@Qry", "@OrderBy", "@ASCDESC", "@Page", "@rowsPerPage" };
            string[] paravalue = { Query, OrderBy, AscDesc, Page.ToString(), PageSize.ToString() };
            DataSet ds = Executeproc("ExecuteQueryPageWise", paraname, paravalue);
            return ds;
        }
        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0, dist_km = 0;
            try
            {
                string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + lat1 + "," + long1 + "&destinations=" + lat2 + "," + long2 + "";
                WebClient client = new WebClient();
                // result = Newtonsoft.Json.JsonConvert.DeserializeObject<VehicleTypeClass.DistanceModelApi>(client.DownloadString(url));
                dynamic stuff = JsonConvert.DeserializeObject(client.DownloadString(url));
                string daddress = stuff.destination_addresses[0];
                string saddress = stuff.origin_addresses[0];
                string status = stuff.status;
                if (status == "OK")
                {
                    if (stuff.rows.Count > 0)
                    {
                        //distance = stuff.rows[0].elements[0].distance.value;
                        string _distance = "";
                        try
                        {
                            if (stuff.rows[0].elements[0].status == "OK")
                            {
                                _distance = stuff.rows[0].elements[0].distance.value;
                            }
                        }
                        catch (Exception)
                        {
                        }


                        double.TryParse(_distance, out distance);
                    }
                }
            }
            catch (Exception)
            {
            }

            dist_km = distance / 1000;
            return dist_km;

        }
        public string[] GetDistance_Duration_BetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0, dist_km = 0;
            long dinMin = 0;

            string[] data = new string[2];
            string _distance = "", _duration = "";
            try
            {
                string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + lat1 + "," + long1 + "&destinations=" + lat2 + "," + long2 + "";
                WebClient client = new WebClient();
                // result = Newtonsoft.Json.JsonConvert.DeserializeObject<VehicleTypeClass.DistanceModelApi>(client.DownloadString(url));
                dynamic stuff = JsonConvert.DeserializeObject(client.DownloadString(url));
                string daddress = stuff.destination_addresses[0];
                string saddress = stuff.origin_addresses[0];
                string status = stuff.status;
                if (status == "OK")
                {
                    if (stuff.rows.Count > 0)
                    {
                        //distance = stuff.rows[0].elements[0].distance.value;

                        try
                        {
                            if (stuff.rows[0].elements[0].status == "OK")
                            {
                                _distance = stuff.rows[0].elements[0].distance.value;
                                _duration = stuff.rows[0].elements[0].duration.value;
                            }
                        }
                        catch (Exception)
                        {
                        }
                        double.TryParse(_distance, out distance);
                    }
                }
            }
            catch (Exception)
            {
            }

            long.TryParse(_duration, out dinMin);
            dinMin = dinMin / 60;
            dist_km = distance / 1000;

            data[0] = dist_km.ToString();
            data[1] = dinMin.ToString();


            return data;

        }
        public string GeenrateRandomnumber(string prefix)
        {
            string randomnumber = "";
            //Random a = new Random(Guid.NewGuid().GetHashCode());
            Random a = new Random(DateTime.Now.GetHashCode());
            randomnumber = prefix + a.Next(0, 999999999).ToString("000000000");
            return randomnumber;
        }

        public DataSet Inline_Process(String Query, string OrderBy, long Page, long PageSize)
        {
            string[] paraname = { "@Qry", "@OrderBy", "@Page", "@rowsPerPage" };
            string[] paravalue = { Query, OrderBy, Page.ToString(), PageSize.ToString() };
            DataSet ds = Executeproc("ExecuteQueryPageWise_v3", paraname, paravalue);
            return ds;
        }

        // Inline Process with full text Search
        public DataSet Inline_Process(String Query, string OrderBy, long Page, long PageSize, string Searchtxt, string SearchOnColumns, string UniqueColId, string Schema, string TableName)
        {
            string[] paraname = { "@Qry", "@OrderBy", "@Page", "@rowsPerPage", "@Search", "@SearchOnColumns", "@UniqueColId", "@Schema", "@TableName" };
            string[] paravalue = { Query, OrderBy, Page.ToString(), PageSize.ToString() };
            DataSet ds = Executeproc("PageWiseQuery_FullTxtSearch", paraname, paravalue);
            return ds;
        }

        //public bool checkExists(string Table, string Column, string Value)
        //{
        //    DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "'");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public bool checkExistsforEdit(string Table, string Column, string Value, string uniquecolumn, string uniqueid)
        //{
        //    DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + uniquecolumn + "<>'" + uniqueid + "' ");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        //public bool checkExists(string Table, string Column, string Value)
        //{
        //    DataSet ds = dl.Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "'");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public double GetDistanceBetweenPoints1(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                       + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));


            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                        + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calaculate distance in  killo metres.
            distance = radius * c;
            return distance / 1000;
        }


        public Tuple<int, int, int, int, int, int> Pager(int totalItems, int? page, int pageSize)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 10;
            var endPage = currentPage + 9;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 20)
                {
                    startPage = endPage - 19;
                }
            }
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            return new Tuple<int, int, int, int, int, int>(TotalItems, CurrentPage, PageSize, TotalPages, StartPage, EndPage);

        }
        public string CheckYoutubeURL(string YURL)
        {
            try
            {
                string url = YURL;  //"https://www.youtube.com/watch?v=eRsGyueVLvQ";
                Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
                Match youtubeMatch = YoutubeVideoRegex.Match(url);

                if (youtubeMatch.Success)
                { YURL = youtubeMatch.Groups[4].Value; }
                else { YURL = ""; }
            }
            catch { YURL = ""; }
            return YURL;
        }
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }


        public DataTable FindCoordinates(string postcode)
        {
            ////https://maps.googleapis.com/maps/api/js?key=AIzaSyDrhVUPfDziSuRXQ5GiwL_nlsOuFpB6Pzk&sensor=false
            string url = "http://maps.google.com/maps/api/geocode/xml?address=" + postcode + "&sensor=false";
            DataTable dtCoordinates = new DataTable();

            dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                new DataColumn("Address", typeof(string)),
                new DataColumn("Latitude",typeof(string)),
                new DataColumn("Longitude",typeof(string)) });

            try
            {
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        DataSet dsResult = new DataSet();
                        dsResult.ReadXml(reader);

                        foreach (DataRow row in dsResult.Tables["result"].Rows)
                        {
                            string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                            DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                            dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                        }
                        return dtCoordinates;
                    }
                }
            }
            catch (Exception)
            {
                return dtCoordinates;
            }

        }

        public DataTable CSV_To_DT(string csvfilePath)
        {
            string filepath = csvfilePath;
            StreamReader sr = new StreamReader(filepath);
            string line = sr.ReadLine();
            string[] value = line.Split(',');
            DataTable dt = new DataTable();
            DataRow row;
            foreach (string dc in value)
            {
                dt.Columns.Add(new DataColumn(dc.Trim('"')));
            }

            while (!sr.EndOfStream)
            {
                value = sr.ReadLine().Split(',');
                for (int i = 0; i < value.Length; i++)
                {
                    value[i] = value[i].Trim('"');
                }
                if (value.Length == dt.Columns.Count)
                {
                    row = dt.NewRow();
                    row.ItemArray = value;
                    dt.Rows.Add(row);
                }
            }
            sr.Dispose();
            return dt;
        }

        public string GetCountryTime(string country)
        {
            DateTime gmt = default(DateTime);
            System.DateTime value = default(System.DateTime);
            gmt = DateTime.Now.AddMinutes(-330);

            switch (country)
            {
                case "India":
                case "Sri Lanka":
                    return DateTime.Now.ToString();
                case "United Kingdom":
                case "Portugal":
                case "Sierra Leone":
                case "Senegal":
                case "Morocco":
                case "Mali":
                    return gmt.ToString();
                case "France":
                case "Spain":
                case "Slovenia":
                case "Slovakia":
                case "Poland":
                case "Nigeria":
                case "Niger":
                case "Hungary":
                case "Denmark":
                case "Czech Republic":
                    return gmt.AddMinutes(60).ToString();
                case "Botswana":
                case "Moldova":
                case "South Africa":
                case "Malawi":
                case "Lithuania":
                case "Libya":
                case "Turkey":
                case "Finland":
                case "Egypt":
                    return gmt.AddMinutes(120).ToString();
                    ;
                case "Bahrain":
                case "Somalia":
                case "Saudi Arabia":
                case "Russia":
                case "Qatar":
                case "Sudan":
                case "Madagascar":
                case "Iraq":
                    return gmt.AddMinutes(180).ToString();
                case "Iran":
                    return gmt.AddMinutes(220).ToString();
                case "Armenia":
                case "Seychelles":
                case "Reunion":
                case "Oman":
                case "Mauritius":
                case "United Arab Emirates":
                case "Georgia":
                case "Azerbaijan":
                    return gmt.AddMinutes(240).ToString();
                case "Afghanistan":
                    return gmt.AddMinutes(270).ToString();
                case "Pakistan":
                case "Maldives":
                case "Kyrgyzstan":
                    return gmt.AddMinutes(300).ToString();
                case "Nepal":
                    return gmt.AddMinutes(345).ToString();
                case "Bangladesh":
                case "Kazakhstan":
                    return gmt.AddMinutes(360).ToString();
                case "Myanmar":
                    return gmt.AddMinutes(390).ToString();
                case "Cambodia":
                case "Laos":
                    return gmt.AddMinutes(420).ToString();
                case "Philippines":
                case "Malaysia":
                case "Hong Kong":
                case "China":
                    return gmt.AddMinutes(480).ToString();
                case "Japan":
                case "Korea":
                    return gmt.AddMinutes(540).ToString();
                case "Micronesia":
                    return gmt.AddMinutes(720).ToString();
                case "Papua New Guinea":
                case "Australia":
                    return gmt.AddMinutes(600).ToString();
                case "New Caledonia":
                    return gmt.AddMinutes(660).ToString();
                case "New Zealand":
                case "Fiji":
                    return gmt.AddMinutes(720).ToString();
                case "Argentina":
                case "Brazil":
                    return gmt.AddMinutes(-180).ToString();
                case "Cuba":
                    return gmt.AddMinutes(-300).ToString();
                case "Aruba":
                case "Paraguay":
                case "Netherlands Antilles":
                case "Barbados":
                case "Chile":
                case "Dominican Republic":
                case "Guyana":
                    return gmt.AddMinutes(-240).ToString();
                case "Bahamas":
                    return gmt.AddMinutes(-240).ToString();
                case "Peru":
                case "Panama":
                case "Jamaica":
                case "Haiti":
                case "Colombia":
                case "Canary Islands":
                    return gmt.AddMinutes(-300).ToString();
                case "Bhutan":
                    return gmt.AddMinutes(360).ToString();
                case "Belize":
                case "Mexico":
                case "Honduras":
                case "Canada":
                    return gmt.AddMinutes(-360).ToString();
                case "Nicaragua":
                    return gmt.AddMinutes(-300).ToString();

                case "United States Of America":
                    return gmt.AddMinutes(-480).ToString();
                case "French Polynesia":
                    return gmt.AddMinutes(720).ToString();
                case "Samoa":
                    return gmt.AddMinutes(-660).ToString();
                case "Singapore":
                    return gmt.AddMinutes(480).ToString();
                case "Slovak Republic":
                    return gmt.AddMinutes(60).ToString();
                case "Solomon Islands":
                    return gmt.AddMinutes(660).ToString();
                case "St Helena":
                    return gmt.AddMinutes(0).ToString();
                case "St Kitts & Nevia":
                    return gmt.AddMinutes(-240).ToString();
                case "St Lucia":
                    return gmt.AddMinutes(-240).ToString();
                case "Surinam":
                    return gmt.AddMinutes(-180).ToString();
                case "Swaziland":
                    return gmt.AddMinutes(120).ToString();
                case "Sweden":
                    return gmt.AddMinutes(60).ToString();
                case "Switzerland":
                    return gmt.AddMinutes(60).ToString();
                case "Syria":
                    return gmt.AddMinutes(120).ToString();
                case "Taiwan":
                    return gmt.AddMinutes(480).ToString();
                case "Tajikistan":
                    return gmt.AddMinutes(300).ToString();
                case "Tanzania":
                    return gmt.AddMinutes(180).ToString();
                case "Thailand":
                    return gmt.AddMinutes(420).ToString();
                case "Tonga":
                    return gmt.AddMinutes(0).ToString();
                case "Trinidad & Tobago":
                    return gmt.AddMinutes(-240).ToString();
                case "Tunisia":
                    return gmt.AddMinutes(60).ToString();
                case "Turkmenistan":
                    return gmt.AddMinutes(300).ToString();
                case "Turks & Caicos Islands":
                    return gmt.AddMinutes(-240).ToString();
                case "Tuvalu":
                    return gmt.AddMinutes(720).ToString();
                case "Uganda":
                    return gmt.AddMinutes(180).ToString();
                case "Ukraine":
                    return gmt.AddMinutes(120).ToString();
                case "Uruguay":
                    return gmt.AddMinutes(-180).ToString();
                case "USA":
                    return gmt.AddMinutes(-480).ToString();
                case "Uzbekistan":
                    return gmt.AddMinutes(300).ToString();
                case "Vanuatu":
                    return gmt.AddMinutes(660).ToString();
                case "Venezuela":
                    return gmt.AddMinutes(-240).ToString();
                case "Vietnam":
                    return gmt.AddMinutes(420).ToString();
                case "Wallis & Futuna Islands":
                    return gmt.AddMinutes(720).ToString();
                case "Yemen":
                    return gmt.AddMinutes(180).ToString();
                case "Zambia":
                    return gmt.AddMinutes(120).ToString();
                case "Zimbabwe":
                    return gmt.AddMinutes(120).ToString();
                default:
                    return "";
            }
        }

        public double ToTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public List<string> getTimeZone(double lat1, double long1)
        {
            //https://maps.googleapis.com/maps/api/timezone/json?location=51.52213,-0.04988&timestamp=1374868635&sensor=false         
            List<string> TimeZone = new List<string>();
            try
            {

                double datetimespan = ToTimestamp(DateTime.UtcNow);
                string url = "https://maps.googleapis.com/maps/api/timezone/json?location=" + lat1 + "," + long1 + "&timestamp=" + datetimespan + "&sensor=false";
                WebClient client = new WebClient();
                dynamic stuff = JsonConvert.DeserializeObject(client.DownloadString(url));
                string status = stuff.status;
                if (status == "OK")
                {
                    string dstOffset = stuff.dstOffset;
                    string rawOffset = stuff.rawOffset;
                    string timeZoneId = stuff.timeZoneId;
                    string timeZoneName = stuff.timeZoneName;
                    TimeZone.Add(timeZoneName);
                    TimeZone.Add(timeZoneId);
                }
            }
            catch (Exception)
            {

            }
            return TimeZone;



        }

        public string Geenrate4Randomnumber(string prefix)
        {
            string randomnumber = "";


            Random a = new Random(DateTime.Now.GetHashCode());
            randomnumber = prefix + a.Next(0, 9999).ToString("0000");

            return randomnumber;

        }


        public DataRow getDataRow(string Table, string Column, string Value)
        {
            DataSet ds = Inline_Process("select * from " + Table + " where " + Column + "='" + Value + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

        public int DeleteRow(string Table, string Column, string Value)
        {
            return InlineNonproc("delete from " + Table + " where " + Column + "='" + Value + "'");
        }

        public int InlineNonproc(string Query)
        {
            try
            {
                int result = 0;
                SqlConnection cn = new SqlConnection(cnn.DbConnectionString);
                SqlCommand cmd = new SqlCommand(Query, cn);
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
            catch
            {
                throw;
            }
        }

        public bool checkExistsforEdit(string Table, string Column, string Value, string uniquecolumn, string uniqueid)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + uniquecolumn + "<>'" + uniqueid + "' ");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkExistsquery(string qry)
        {
            DataSet ds = Inline_Process(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
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
        
        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

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

        public bool checkExistsCategoryWise(string Table, string Column, string Value, string IdColumn, string IdValue)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + IdColumn + "='" + IdValue + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkExistsCategoryWiseEdit(string Table, string Column, string Value, string IdColumn, string IdValue, string UniqColumn, string UniqColumnValue)
        {
            DataSet ds = Inline_Process("select " + Column + " from " + Table + " where " + Column + "='" + Value + "' and " + IdColumn + "='" + IdValue + "' and " + UniqColumn + "<>'" + UniqColumnValue + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}