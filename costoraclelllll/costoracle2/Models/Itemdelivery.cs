using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace costoracle2.Models
{
    public class Itemdelivery
    {
        public string Id { get; set; }
        public string itemtypeid { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Addresspick { get; set; }
        public string Latitude { get; set; }
        public string Latitudepick { get; set; }
        public string Longitude { get; set; }
        public string Longitudepick { get; set; }
        public string City { get; set; }
        public string Citypick { get; set; }
        public string Country { get; set; }
        public string Countrypick { get; set; }
        public string noofitem { get; set; }
        public string weight { get; set; }
        public string height { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        
    }
}