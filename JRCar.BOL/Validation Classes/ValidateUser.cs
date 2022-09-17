using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidateUser
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email Address")]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", ErrorMessage = "Invalid Email Address")]
        [Remote("IsEmailExist", "Account", ErrorMessage = "Email is already registered!")]
        public string SignUpEmail { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{4}[-]?\d{7}$", ErrorMessage = "Invalid Phone Number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "<ul><li style=\"list-style:none !important;font-size:12px !important;line-height: 14px !important;margin-bottom:8px !important\">Password contain minimum 8 " +
            "characters in length and At least one uppercase, lowercase English letter and one digit and special character.</li></ul>")]
        public string Password { get; set; }

        public string ShowroomURL { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Showroom Description")]
        public string ShowroomDescription { get; set; }

        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int tblRoleID { get; set; }

        public virtual tblRole tblRole { get; set; }
        public string tblRoleName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "OTP")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid OTP")]
        public string OTP { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "CNIC")]
        [RegularExpression(@"^\d{5}[-]?\d{7}[-]?\d{1}$", ErrorMessage = "Invalid CNIC Number")]
        public string CNIC { get; set; }
    }
}
