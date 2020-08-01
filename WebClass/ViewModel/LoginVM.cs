using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClass.ViewModel
{
    public class LoginVM
    {
        //新增類別來接收login裡面要傳遞的資料
        public string account { get; set; }
        public string password { get; set; }
        public string remember { get; set; }
    }
}