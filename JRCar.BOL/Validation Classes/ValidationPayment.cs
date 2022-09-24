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

        [Display(Name = "Recievable")]
        public decimal? Recievable { get; set; }

        [Display(Name = "Recieved")]
        public decimal? Recieved { get; set; }

        public System.DateTime RecievableFromDate { get; set; }

        public System.DateTime RecievableToDate { get; set; }

        public List<string> RecievableDate { get; set; }
        
        public List<string> RecievedDate { get; set; }

        public System.DateTime RecievedFromDate { get; set; }

        public System.DateTime RecievedToDate { get; set; }
        
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? Isactive { get; set; }
        public bool? Isarchive { get; set; }

        public virtual tblShowroom tblShowroom { get; set; }
    }
}
