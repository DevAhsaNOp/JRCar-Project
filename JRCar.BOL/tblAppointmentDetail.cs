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
    
    public partial class tblAppointmentDetail
    {
        public int ID { get; set; }
        public Nullable<int> AppointmentID { get; set; }
        public Nullable<int> ShowroomID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Purpose { get; set; }
        public System.DateTime Date { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual tblAppointment tblAppointment { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblShowroom tblShowroom { get; set; }
    }
}
