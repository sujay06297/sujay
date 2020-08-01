using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.Models
{
    [MetadataType(typeof(posttype))]
    public partial class M_PostType
    {
        public class posttype
        {
            [DisplayName("發文類型ID")]
            public int PostTypeID { get; set; }

            [DisplayName("發文類型")]
            public string PostTypeName { get; set; }
        }
    }
}