using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClass.Models
{
    [MetadataType(typeof(userMetadata))]
    public partial class U_User
    {
        //[Required]
        //[DisplayName("確認密碼")]
        //[Compare("UserPassword", ErrorMessage = "確認密碼不正確!請重新輸入。")]
        //public string ConfirmPassword { get; set; }

        public class userMetadata
        {
            //[DataType(DataType.MultilineText)]
            [DisplayName("使用者ID")]
            public int UserID { get; set; }

            [Required(ErrorMessage = "此欄位必填")]
            [DisplayName("使用者帳戶")]
            [MinLength(8, ErrorMessage = "使用者帳戶長度不得少於8字!")]
            [MaxLength(20, ErrorMessage = "使用者帳戶長度不得多於20字!")]
            public string UserAccount { get; set; }

            [Required(ErrorMessage = "此欄位必填")]
            [DisplayName("輸入密碼")]
            [MinLength(8, ErrorMessage = "密碼長度不得少於8字!")]
            [MaxLength(20, ErrorMessage = "密碼長度不得多於20字!")]
            public string UserPassword { get; set; }

            [Required(ErrorMessage = "此欄位必填")]
            [DisplayName("信箱")]
            //[DataType(DataType.EmailAddress, ErrorMessage ="電子郵件地址不正確!")]
            [MaxLength(40, ErrorMessage = "電子郵件地址不得多於40字!")]
            [EmailAddress(ErrorMessage = "電子郵件地址不正確!")]
            public string Email { get; set; }

            [Required(ErrorMessage = "此欄位必填")]
            [DisplayName("姓名")]
            [MaxLength(10, ErrorMessage = "姓名不得多於10字!")]
            public string UserName { get; set; }

            [DataType(DataType.Upload)]
            [DisplayName("大頭照")]
            public byte[] UserPhoto { get; set; }

            [Required(ErrorMessage = "此欄位必填")]
            [DisplayName("生日")]
            public Nullable<System.DateTime> Birth { get; set; }

            [Required(ErrorMessage = "此欄位必填")]
            [DisplayName("性別")]
            public string Gender { get; set; }

            [DisplayName("行動")]
            //[DataType(DataType.PhoneNumber)]
            [MaxLength(10, ErrorMessage = "號碼不得多於10碼!")]
            public string Phone { get; set; }

            [DisplayName("地址")]
            [MaxLength(50, ErrorMessage = "地址不得多於50字!")]
            public string Address { get; set; }

            [DisplayName("建立日期")]
            public System.DateTime CreateTime { get; set; }

            [DisplayName("權限ID")]
            public int PermissionID { get; set; }
        }
    }
}