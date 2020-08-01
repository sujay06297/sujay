using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebClass.Models
{

    [MetadataType(typeof(classMetadata))]
    public partial class C_Class
    {
        public class classMetadata
        {
            [Key]
            public int ClassID { get; set; }

            [DisplayName("課程名稱")]
            [Required(ErrorMessage = "此欄位必填")]
            [StringLength(25, ErrorMessage = "字數不可超過25")]
            public string ClassName { get; set; }

            [DisplayName("價格")]
            [Required(ErrorMessage = "此欄位必填")]
            [Range(1, 99999, ErrorMessage = "數字不可為0或超過10萬")]
            [DisplayFormat(DataFormatString = "{0:c0}")]
            public int Price { get; set; }

            [DisplayName("封面圖片")]
            [DataType(DataType.Upload)]
            public byte[] MainPicture { get; set; }

            [DisplayName("課程敘述")]
            [AllowHtml]
            [DataType(DataType.MultilineText)]
            [Required(ErrorMessage = "此欄位必填")]
            public string ClassContent { get; set; }

            public Nullable<System.DateTime> UpLoadTime { get; set; }

            [DisplayName("事前預約時間")]
            [DataType(DataType.Date)]
            public System.DateTime PreRegisterTime { get; set; }

            [DisplayName("開課時間")]
            [DataType(DataType.Date)]
            [Required(ErrorMessage = "此欄位必填")]
            public System.DateTime RegisterTime { get; set; }

            [DisplayName("需求人數")]
            [Required(ErrorMessage = "此欄位必填")]
            public int NeedUser { get; set; }

            [DisplayName("開課者")]
            public int CreateUserID { get; set; }

            [DisplayName("開課狀態")]
            public int CreateTypeID { get; set; }

            [DisplayName("課程類別")]
            [Required(ErrorMessage = "此欄位必填")]
            public int ClassTypeID { get; set; }
        }

    }
}