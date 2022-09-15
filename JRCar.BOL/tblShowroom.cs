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
    
    public partial class tblShowroom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblShowroom()
        {
            this.tblBlogs = new HashSet<tblBlog>();
            this.tblCars = new HashSet<tblCar>();
            this.tblNotifications = new HashSet<tblNotification>();
            this.tblAppointments = new HashSet<tblAppointment>();
        }
    
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CNIC { get; set; }
        public string Contact { get; set; }
        public string ShopNumber { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public string OTP { get; set; }
        public string ShowroomURL { get; set; }
        public int RoleId { get; set; }
        public Nullable<int> AddressId { get; set; }
        public Nullable<int> UnionId { get; set; }
        public bool Isactive { get; set; }
        public bool Isarchive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Description { get; set; }
    
        public virtual tblAddress tblAddress { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBlog> tblBlogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCar> tblCars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblNotification> tblNotifications { get; set; }
        public virtual tblRole tblRole { get; set; }
        public virtual tblUnion tblUnion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAppointment> tblAppointments { get; set; }
    }
}
