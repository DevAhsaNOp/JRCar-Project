using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidateAddress
    {
        [Required(ErrorMessage ="*")]
        [Display(Name ="State")]
        public string State { get; set; }

        [Required(ErrorMessage ="*")]
        [Display(Name ="City")]
        public string City { get; set; }

        [Display(Name ="Area")]
        public string Area { get; set; }
 
        [Display(Name ="Address")]
        public string CompleteAddress { get; set; }

        public virtual ICollection<tblCar> tblCars { get; set; }
        public virtual ICollection<tblShowroom> tblShowrooms { get; set; }
        public virtual ICollection<tblUserAdd> tblUserAdds { get; set; }
    }
}
