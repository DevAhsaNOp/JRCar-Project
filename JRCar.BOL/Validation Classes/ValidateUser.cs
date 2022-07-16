using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidateUser
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{4}[-]?\d{7}$", ErrorMessage = "Invalid Phone Number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "<ul style=\"font-size:11px !important;\"><li>Password contain minimum 8 " +
            "characters in length.</li><li> At least one uppercase and lowercase English letter.</li>"+
            "<li> At least one digit and special character. </li></ul>")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Image")]
        public string Image { get; set; }

        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int tblRoleID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "OTP")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid OTP")]
        public string OTP { get; set; }
    }
}
