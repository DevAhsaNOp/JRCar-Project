using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidateAppointment
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public Nullable<int> UserInterestedID { get; set; }
        public Nullable<int> ShowroomInterestedID { get; set; }
        public Nullable<int> UserCarID { get; set; }
        public Nullable<int> ShowroomCarID { get; set; }

        [Required(ErrorMessage ="*")]
        public string Number { get; set; }

        [Required(ErrorMessage ="*")]
        public string Purpose { get; set; }

        [Required(ErrorMessage ="*")]
        public System.DateTime Datetime { get; set; }

        public bool Isactive { get; set; }
        public bool IsAccepted { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public virtual tblCar tblCar { get; set; }
        public virtual tblUserAdd tblUserAdd { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblShowroom tblShowroom { get; set; }
        public int ShowroomID { get; set; }
        public int? UserID { get; set; }
        public string ShowroomContact { get; set; }
        public string ShowroomEmail { get; set; }
        public string UserContact { get; set; }
        public string UserEmail { get; set; }
    }
}
