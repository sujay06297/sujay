using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.ViewModel
{
    public class StudentVM
    {
        [DisplayName("帳號")]
        public string Account { get; set; }

        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("報名時間")]
        [DataType(DataType.Date)]
        public DateTime BuyTime { get; set; }

        [DisplayName("性別")]
        public string Gender { get; set; }

        [DisplayName("電子郵件")]
        public string Email { get; set; }

    }
}