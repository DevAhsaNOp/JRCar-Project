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
    using System.ComponentModel.DataAnnotations;

    public partial class tblAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAddress()
        {
            this.tblCars = new HashSet<tblCar>();
            this.tblShowrooms = new HashSet<tblShowroom>();
            this.tblUserAdds = new HashSet<tblUserAdd>();
        }
    
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a State")]
        public int State { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "City")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a City")]
        public int City { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Area")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Area")]
        public Nullable<int> Area { get; set; }
        public string CompleteAddress { get; set; }
    
        public virtual tblCity tblCity { get; set; }
        public virtual tblState tblState { get; set; }
        public virtual tblZone tblZone { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCar> tblCars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblShowroom> tblShowrooms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUserAdd> tblUserAdds { get; set; }
    }
}
