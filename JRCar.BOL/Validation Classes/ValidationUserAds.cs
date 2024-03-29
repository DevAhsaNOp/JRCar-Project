﻿using System;
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
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Model Year")]
        public string Year { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Condition")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Condition")]
        public string Condition { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Range(5, 50, ErrorMessage = "Minimum 5 images you need to upload")]
        public int ImagesCount { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Title")]
        [StringLength(30,ErrorMessage ="Title can only contain 30 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Price")]
        public string Price { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        [StringLength(150, ErrorMessage = "Address can only contain 150 characters.")]
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

        [Display(Name = "Member Since")]
        public System.DateTime UserCreatedOn { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string Number { get; set; }

        public List<HttpPostedFileBase> Imagefiles { get; set; }

        [Display(Name = "Car Images")]
        public string CarImage { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a State")]
        public string State { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "City")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a City")]
        public string City { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Area")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Area")]
        public string Area { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a State")]
        public int StateID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "City")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a City")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Area")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Area")]
        public int AreaID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        [StringLength(150)]
        public string CompleteAddress { get; set; }
        
        [Display(Name = "AdURL")]
        public string AdURL { get; set; }

        [Display(Name = "Active")]
        public bool? Isactive { get; set; }

        public bool? IsSold { get; set; }
        
        [Display(Name = "Archive")]
        public bool? Isarchive { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Make")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Make")]
        public int? ManufacturerId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Make")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Make")]
        public string Manufacturer_Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Model")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Model")]
        public int? ManufacturerCarModelID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Model")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Model")]
        public string Manufacturer_CarModelName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Category")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Category")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Category")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Category")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "SubCategory")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a SubCategory")]
        public int? SubCategoryId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "SubCategory")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a SubCategory")]
        public string SubCategoryName { get; set; }

        public int MaximumPrice { get; set; }

        public int MinimumPrice { get; set; }

        public int AdID { get; set; }

        public string AdViewsCount { get; set; }

        public virtual ICollection<tblUserAddImage> tblUserAddImages { get; set; }

        public virtual tblUser tblUser { get; set; }

        public virtual tblAddress tblAddress { get; set; }

        public virtual tblCategory tblCategory { get; set; }

        public virtual tblManfacturerCarModel tblManfacturerCarModel { get; set; }

        public virtual tblManufacturer tblManufacturer { get; set; }

        public virtual tblSubCategory tblSubCategory { get; set; }
        public int TotalAdsCount { get; set; }
        public int TotalActiveAdsCount { get; set; }
        public int TotalInActiveAdsCount { get; set; }
        public bool UserActive { get; set; }
    }
}
