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
    
    public partial class tblCarModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCarModel()
        {
            this.tblCars = new HashSet<tblCar>();
        }
    
        public int ID { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Manufacturer { get; set; }
        public string BodyType { get; set; }
        public string Seater { get; set; }
        public string Assembly { get; set; }
        public string EngineCapacity { get; set; }
        public int CarFeatureID { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public bool Isactive { get; set; }
        public bool Isarchive { get; set; }
        public string EngineType { get; set; }
    
        public virtual tblCarFeature tblCarFeature { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCar> tblCars { get; set; }
    }
}
