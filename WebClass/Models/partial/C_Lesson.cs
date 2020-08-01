using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.Models
{
    [MetadataType(typeof(lessonMetadata))]
    public partial class C_Lesson
    {
        public class lessonMetadata
        {

            [Key]
            public int LessonID { get; set; }

            [DisplayName("單元名稱")]
            [Required(ErrorMessage = "此欄位必填")]
            [StringLength(25, ErrorMessage = "字數不可超過25")]
            public string LessonName { get; set; }

            [DisplayName("單元影片")]
            [Required(ErrorMessage = "此欄位必填")]
            public string LessonVideo { get; set; }

            [DisplayName("單元內容")]
            [Required(ErrorMessage = "此欄位必填")]
            [StringLength(100, ErrorMessage = "字數不可超過100")]
            public string LessonContent { get; set; }
        }
    }
}