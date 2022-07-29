using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidationUserAds
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Condition")]
        public string Condition { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Price")]
        public string Price { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Latitude")]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

        [Display(Name = "CreatedOn")]
        public System.DateTime CreatedOn { get; set; }

        [Display(Name = "ExpiryDate")]
        public System.DateTime ExpiryDate { get; set; }

        public int? AddressId { get; set; }

        public string UserRole { get; set; }

        [Display(Name = "Image")]
        public string UserImage { get; set; }

        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string Number { get; set; }

        public List<HttpPostedFileBase> Imagefiles { get; set; }

        [Display(Name = "Car Images")]
        public string CarImage { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Area")]
        public string Area { get; set; }

        [Display(Name = "Address")]
        public string CompleteAddress { get; set; }

        public virtual ICollection<tblUserAddImage> tblUserAddImages { get; set; }
        public virtual tblUser tblUser { get; set; }

        public virtual tblAddress tblAddress { get; set; }
    }
}
