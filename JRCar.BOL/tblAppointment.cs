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
    
    public partial class tblAppointment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAppointment()
        {
            this.tblAppointmentDetails = new HashSet<tblAppointmentDetail>();
        }
    
        public int ID { get; set; }
        public Nullable<int> UserInterestedID { get; set; }
        public Nullable<int> ShowroomInterestedID { get; set; }
        public Nullable<int> UserCarID { get; set; }
        public Nullable<int> ShowroomCarID { get; set; }
        public bool Isactive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<bool> IsAccepted { get; set; }
    
        public virtual tblCar tblCar { get; set; }
        public virtual tblShowroom tblShowroom { get; set; }
        public virtual tblUserAdd tblUserAdd { get; set; }
        public virtual tblUser tblUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAppointmentDetail> tblAppointmentDetails { get; set; }
    }
}
