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
    
    public partial class tblCar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCar()
        {
            this.tblAppointments = new HashSet<tblAppointment>();
            this.tblCarImages = new HashSet<tblCarImage>();
            this.tblFavAdds = new HashSet<tblFavAdd>();
        }
    
        public int ID { get; set; }
        public int CarModelID { get; set; }
        public int tblShowroomID { get; set; }
        public string RegNo { get; set; }
        public string RegLocation { get; set; }
        public string Condition { get; set; }
        public string MaxSpeed { get; set; }
        public string Color { get; set; }
        public string Price { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public bool Isactive { get; set; }
        public bool Isarchive { get; set; }
        public string GearType { get; set; }
        public string CurrentLocation { get; set; }
        public string Mileage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Transmission { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAppointment> tblAppointments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCarImage> tblCarImages { get; set; }
        public virtual tblCarModel tblCarModel { get; set; }
        public virtual tblShowroom tblShowroom { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFavAdd> tblFavAdds { get; set; }
    }
}
