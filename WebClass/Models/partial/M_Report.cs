using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebClass.Models.M_MessageBoard;

namespace WebClass.Models
{
    [MetadataType(typeof(messageMetadata))]
    public partial class M_Report
    {
        public class messageMetadata
        {
            [DisplayName("檢舉ID")]
            public int ReportID { get; set; }

            [DisplayName("檢舉原因")]
            [DataType(DataType.MultilineText)]
            [Required(ErrorMessage = "此欄位必填")]
            public string ReportContent { get; set; }

            [DisplayName("檢舉日期")]
            [DataType(DataType.Date)]
            public System.DateTime ReportDate { get; set; }

            [DisplayName("解決日期")]
            [DataType(DataType.Date)]
            public System.DateTime SolDate { get; set; }

            [DisplayName("處理類型ID")]
            public int SolutionTypeID { get; set; }

            [DisplayName("留言板ID")]
            public Nullable<int> MessageBoardID { get; set; }

            [DisplayName("回文ID")]
            public Nullable<int> ReMessageBoardID { get; set; }

            [DisplayName("檢舉人")]
            public int ReportUserID { get; set; }

            public virtual M_MessageBoard M_MessageBoard { get; set; }
            public virtual M_ReMessageBoard M_ReMessageBoard { get; set; }
            public virtual M_SolutionType M_SolutionType { get; set; }
            public virtual U_User U_User { get; set; }
        }
    }
}