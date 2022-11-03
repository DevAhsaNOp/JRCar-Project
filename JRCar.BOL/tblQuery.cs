//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JRCar.BOL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblQuery
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblQuery()
        {
            this.tblQueryDetails = new HashSet<tblQueryDetail>();
        }
    
        public int ID { get; set; }
        public string QueryNo { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> ShowroomID { get; set; }
        public Nullable<int> UnionID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<bool> Isactive { get; set; }
        public Nullable<bool> Isarchive { get; set; }
        public Nullable<bool> IsUserRead { get; set; }
        public Nullable<bool> IsShowroomRead { get; set; }
        public Nullable<bool> IsUnionRead { get; set; }
    
        public virtual tblUser tblUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblQueryDetail> tblQueryDetails { get; set; }
        public virtual tblShowroom tblShowroom { get; set; }
        public virtual tblUnion tblUnion { get; set; }
    }
}
