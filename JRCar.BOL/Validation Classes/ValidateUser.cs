using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BOL.Validation_Classes
{
    class ValidateUser
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="*")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int tblRoleID { get; set; }
        public string OTP { get; set; }
    }
}
