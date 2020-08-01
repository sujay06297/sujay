using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.ViewModel
{
    public class LearningVM
    {

        public int ClassID { get; set; }
        [DisplayName("課程名稱")]
        public string ClassName { get; set; }
        [DisplayName("購買價錢")]
        [DisplayFormat(DataFormatString = "{0:c0}")]
        public int Price { get; set; }
        [DisplayName("開課狀態")]
        public string CreateType { get; set; }
        [DisplayName("開課時間")]
        [DataType(DataType.Date)]
        public DateTime RegisterTime { get; set; }
        [DisplayName("購買時間")]
        [DataType(DataType.Date)]
        public DateTime BuyingTime { get; set; }
        [DisplayName("講師")]
        public string CreateUser { get; set; }
        [DisplayName("購買者")]
        public int Buyer { get; set; }
        [DisplayName("課程類別")]
        public string ClassType { get; set; }
    }
}