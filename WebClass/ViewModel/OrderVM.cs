using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.ViewModel
{
    public class OrderVM
    {

        public int OrderDetailID { get; set; }
        public int Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:c0}")]
        public int Amount { get; set; }
        public int ClassID { get; set; }
        public int OrderID { get; set; }

        public string ClassName { get; set; }
        public string CreateTypeName { get; set; }
        public int ShoppingCarID { get; set; }
    }
}