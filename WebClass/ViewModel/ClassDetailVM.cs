using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebClass.ViewModel
{
    public class ClassDetailVM
    {

        public int ClassID { get; set; }
        [DisplayName("課程名稱")]
        public string ClassName { get; set; }
        [DisplayName("價格")]
        public int Price { get; set; }
        [DisplayName("封面圖片")]
        public byte[] MainPicture { get; set; }
        [DisplayName("課程敘述")]
        public string ClassContent { get; set; }

        public Nullable<System.DateTime> UpLoadTime { get; set; }
        [DisplayName("事前預約時間")]
        public System.DateTime PreRegisterTime { get; set; }
        [DisplayName("正式上課時間")]
        public System.DateTime RegisterTime { get; set; }
        [DisplayName("需求人數")]
        public int NeedUser { get; set; }
        [DisplayName("開課者")]
        public int CreateUserID { get; set; }
        [DisplayName("開課狀態")]
        public int CreateTypeID { get; set; }
        [DisplayName("課程類別")]
        public string ClassTypeName { get; set; }
        [DisplayName("評分")]
        public double Score { get; set; }
        [DisplayName("購買者")]
        public bool Buyer { get; set; }

    }
}