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
    
    public partial class tblCarFeature
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCarFeature()
        {
            this.tblCarModels = new HashSet<tblCarModel>();
        }
    
        public int ID { get; set; }
        public string FuelType { get; set; }
        public bool AirConditioned { get; set; }
        public bool ABS { get; set; }
        public bool AirBag { get; set; }
        public bool PowerWindows { get; set; }
        public bool PowerMirrors { get; set; }
        public bool PowerLocks { get; set; }
        public bool PowerSteering { get; set; }
        public bool ImmobilizerKey { get; set; }
        public bool Radio { get; set; }
        public bool KeyLessEntry { get; set; }
        public bool AlloyRims { get; set; }
        public bool CoolBox { get; set; }
        public bool CruiseControl { get; set; }
        public bool SunRoof { get; set; }
        public bool NavigationSystem { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public bool Isactive { get; set; }
        public bool Isarchive { get; set; }
        public bool RearAcVents { get; set; }
        public bool FrontCam { get; set; }
        public bool CassetPlayer { get; set; }
        public bool DvdPlayer { get; set; }
        public bool SteeringSwitch { get; set; }
        public bool CdPlayer { get; set; }
        public bool ClimateControl { get; set; }
        public bool FrontSpeaker { get; set; }
        public bool HeatedSeat { get; set; }
        public bool RearCamera { get; set; }
        public bool RearSeatEntertain { get; set; }
        public bool RearSpeaker { get; set; }
        public bool BoxUsbAux { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCarModel> tblCarModels { get; set; }
    }
}
