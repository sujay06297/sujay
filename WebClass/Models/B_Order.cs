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
    
    public partial class B_Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public B_Order()
        {
            this.B_OrderDetail = new HashSet<B_OrderDetail>();
        }
    
        public int OrderID { get; set; }
        public System.DateTime BuyTime { get; set; }
        public int UserID { get; set; }
    
        public virtual U_User U_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<B_OrderDetail> B_OrderDetail { get; set; }
    }
}