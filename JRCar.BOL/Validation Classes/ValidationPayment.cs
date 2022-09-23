using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidationPayment
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Showroom")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Showroom")]
        public int? ShowroomID { get; set; }

        public string ShowroomName { get; set; }

        public string ShowroomNumber { get; set; }

        public string ShowroomAddress { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Recievable")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Must Enter a Recievable")]
        public decimal? Recievable { get; set; }

        public decimal? Recieved { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "From Date")]
        public System.DateTime FromDate { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "To Date")]
        public System.DateTime ToDate { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? Isactive { get; set; }
        public bool? Isarchive { get; set; }

        public virtual tblShowroom tblShowroom { get; set; }
    }
}
