using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.Models
{
    [MetadataType(typeof(messageMetadata))]
    public partial class M_MessageBoard
    {
        public class messageMetadata
        {
            [DisplayName("留言板ID")]
            public int MessageBoardID { get; set; }

            [DisplayName("留言內容")]
            [DataType(DataType.MultilineText)]
            [Required(ErrorMessage = "此欄位必填")]
            public string MessageBoardContent { get; set; }

            [DisplayName("留言日期")]
            [DataType(DataType.Date)]
            public DateTime PostTime { get; set; }

            public bool Status { get; set; }

            [DisplayName("使用者ID")]
            public int UserID { get; set; }

            [DisplayName("課程ID")]
            public Nullable<int> ClassID { get; set; }

            [DisplayName("單元ID")]
            public Nullable<int> LessonID { get; set; }


            [DisplayName("發文類型ID")]
            public int PostTypeID { get; set; }

            public virtual C_Class C_Class { get; set; }
            public virtual C_Lesson C_Lesson { get; set; }
            public virtual M_PostType M_PostType { get; set; }
            public virtual U_User U_User { get; set; }
        }
    }
}