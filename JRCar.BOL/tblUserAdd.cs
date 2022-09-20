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
    
    public partial class tblUserAdd
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblUserAdd()
        {
            this.tblUserAddImages = new HashSet<tblUserAddImage>();
            this.tblAppointments = new HashSet<tblAppointment>();
        }
    
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Condition { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<int> ManufacturerId { get; set; }
        public Nullable<int> ManufacturerCarModelID { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public Nullable<int> AddressId { get; set; }
        public Nullable<bool> Isactive { get; set; }
        public Nullable<bool> Isarchive { get; set; }
        public string UserAdsURL { get; set; }
        public Nullable<bool> Issold { get; set; }
    
        public virtual tblAddress tblAddress { get; set; }
        public virtual tblCategory tblCategory { get; set; }
        public virtual tblManfacturerCarModel tblManfacturerCarModel { get; set; }
        public virtual tblManufacturer tblManufacturer { get; set; }
        public virtual tblSubCategory tblSubCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUserAddImage> tblUserAddImages { get; set; }
        public virtual tblUser tblUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAppointment> tblAppointments { get; set; }
    }
}
