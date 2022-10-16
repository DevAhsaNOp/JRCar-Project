using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidateShowroom
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Showroom Name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only Alphabets are allow")]
        [MaxLength(50, ErrorMessage = "Name Max Length should be under 50")]
        [MinLength(5, ErrorMessage = "Name Min Length should be 5")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid Email Address")]
        [Remote("IsEmailExist", "Account", ErrorMessage = "Email is already registered!")]
        public string SignUpEmail { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "<ul><li style=\"list-style:none !important;font-size:12px !important;line-height: 14px !important;margin-bottom:8px !important\">Password contain minimum 8 " +
            "characters in length and At least one uppercase, lowercase English letter and one digit and special character.</li></ul>")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "CNIC")]
        [RegularExpression(@"^\d{5}[-]?\d{7}[-]?\d{1}$", ErrorMessage = "Invalid CNIC Number")]
        public string CNIC { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"03[0-9]{2}(?!1234567)(?!1111111)(?!7654321)[0-9]{7}", ErrorMessage = "Invalid Phone Number")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Shop Number")]
        [MaxLength(150, ErrorMessage = "Address Length should be under 150")]
        [MinLength(5, ErrorMessage = "Address Min Length should be 5")]
        public string ShopNumber { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "OTP")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid OTP")]
        public string OTP { get; set; }

        public string ShowroomURL { get; set; }

        public int RoleId { get; set; }
        
        public int? AddressId { get; set; }

        public int? UnionId { get; set; }

        public bool Isactive { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "User Status")]
        [Range(0, 1, ErrorMessage = "Must select a Status")]
        public string Active { get; set; }

        public bool Isarchive { get; set; }

        public int CreatedBy { get; set; }

        public System.DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public System.DateTime? UpdatedOn { get; set; }

        public virtual tblRole tblRole { get; set; }
        public virtual tblAddress tblAddress { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBlog> tblBlogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCar> tblCars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblNotification> tblNotifications { get; set; }
        public virtual tblUnion tblUnion { get; set; }
    }
}
