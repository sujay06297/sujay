using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.ViewModel
{
    public class TeachingVM
    {

        public int ClassID { get; set; }
        [DisplayName("課程名稱")]
        public string ClassName { get; set; }
        [DisplayName("價錢")]
        [DisplayFormat(DataFormatString = "{0:c0}")]
        public int Price { get; set; }
        [DisplayName("開課狀態")]
        public string CreateType { get; set; }
        [DisplayName("開課時間")]
        [DataType(DataType.Date)]
        public DateTime RegisterTime { get; set; }
        [DisplayName("課程人數")]
        public int ClassStudentNumber { get; set; }
        [DisplayName("開課者")]
        public int CreateUser { get; set; }
        [DisplayName("開課狀態ID")]
        public int CreateTypeID { get; set; }
        [DisplayName("課程類別")]
        public string ClassType { get; set; }
    }
}