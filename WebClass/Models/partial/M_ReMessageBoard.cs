using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.Models
{
    [MetadataType(typeof(remessage))]
    public partial class M_ReMessageBoard
    {
        public class remessage
        {
            [DisplayName("回文ID")]
            public int ReMessageBoardID { get; set; }


            [DisplayName("回文內容")]
            public string ReMessageBoardContent { get; set; }


            [DisplayName("回文時間")]
            [DataType(DataType.Date)]
            public System.DateTime RePostTime { get; set; }


            [DisplayName("狀態(封鎖是否)")]
            public bool Status { get; set; }
        }

    }
}