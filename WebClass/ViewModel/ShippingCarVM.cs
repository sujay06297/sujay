using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebClass.Models;

namespace WebClass.ViewModel
{
    public class ShippingCarVM
    {

        public string ClassName { get; set; }
        public string CreateTypeName { get; set; }
        public int Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:c0}")]
        public int Amount { get; set; }
        public int ShoppingCarID { get; set; }
        public int ClassID { get; set; }
    }
}