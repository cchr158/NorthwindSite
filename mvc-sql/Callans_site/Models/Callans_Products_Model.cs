using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Callans_site.Models {
    public class Callans_Products_Model {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public string CategoryName { get; set; }
        //public string Description { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Country { get; set; }
    }
}