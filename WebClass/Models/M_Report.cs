//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebClass.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class M_Report
    {
        public int ReportID { get; set; }
        public string ReportContent { get; set; }
        public System.DateTime ReportDate { get; set; }
        public System.DateTime SolDate { get; set; }
        public int SolutionTypeID { get; set; }
        public Nullable<int> MessageBoardID { get; set; }
        public Nullable<int> ReMessageBoardID { get; set; }
        public int ReportUserID { get; set; }
    
        public virtual M_MessageBoard M_MessageBoard { get; set; }
        public virtual M_ReMessageBoard M_ReMessageBoard { get; set; }
        public virtual M_SolutionType M_SolutionType { get; set; }
        public virtual U_User U_User { get; set; }
    }
}