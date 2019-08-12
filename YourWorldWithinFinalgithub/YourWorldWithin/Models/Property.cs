using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace YourWorldWithin.Models
{
    public class Property
    {
        //private string con = "data source=MANOJ-PC\\HARRY;user id=sa;password=sql@2012;initial catalog=alampforeverytrade";

        //private string con = "Data Source=64.16.214.16,1986;Initial Catalog=alampforeverytrade;User ID=sa;Password=";
        private string con = ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
        //private string con = "Data Source=Ravi-Pc;Initial Catalog=alampforeverytrade;User ID=sa;Password=sql@2012";
        public string Con
        {
            get
            {
                return con;
            }
        }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public string ConPassword { get; set; }
        public string OldPassword { get; set; }
        public string creationdate { get; set; }
        //=== VideoModel
        public string VideoId { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string CategoryId { get; set; }
        public string planid { get; set; }
        public string plan { get; set; }
        

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "VideoFile")]
        public string VideoFile { get; set; }

        [Display(Name = "ImageFile")]
        public string ImageFile { get; set; }
        public string Status { get; set; }

        //User
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Boolean EmailVerified { get; set; }
        public string ModifiedDate { get; set; }
                   
    }
}