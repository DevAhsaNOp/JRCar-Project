using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidateShowroomAds
    {
        public int tblCarID { get; set; }

        public int CarModelID { get; set; }

        public int tblShowroomID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Registration Number")]
        public string RegNo { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(5, 50,ErrorMessage = "Minimum 5 images you need to upload")]
        public int ImagesCount { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Registered Location")]
        public string RegLocation { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Condition")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Condition")]
        public string Condition { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Max Speed")]
        public string MaxSpeed { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Price")]
        [Range(100000, 100000000, ErrorMessage = "Please enter correct Price")]
        public string Price { get; set; }

        [Display(Name = "Latitude")]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

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

        [Display(Name = "Address")]
        public string CompleteAddress { get; set; }

        public string ShowroomRole { get; set; }

        public int tblCarCreatedBy { get; set; }

        public string tblCarCreatedOn { get; set; }

        public string tblCarUpdatedOn { get; set; }

        public string CarIsActive { get; set; }

        public int? tblCarUpdatedBy { get; set; }

        public bool tblCarIsactive { get; set; }
        
        public bool IsSold { get; set; }

        public bool tblCarIsarchive { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Gear Type")]
        public string GearType { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Showroom Location")]
        public string CurrentLocation { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Mileage")]
        [Range(1, 1000000, ErrorMessage = "Please enter correct Mileage")]
        public string Mileage { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "TRansmission")]
        public string Transmission { get; set; }

        public int? AddressId { get; set; }

        public string CarsURL { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Make")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Make")]
        public int? ManufacturerId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Model")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Model")]
        public int? ManufacturerCarModelID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Category")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Category")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sub Category")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a SubCategory")]
        public int? SubCategoryId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Make")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Make")]
        public string Manufacturer_Name { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Display(Name = "Model")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Model")]
        public string Manufacturer_CarModelName { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Display(Name = "Category")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Category")]
        public string CategoryName { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Display(Name = "Sub Category")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a SubCategory")]
        public string SubCategoryName { get; set; }

        public int tblCarModelID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Year")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Must select a Model Year")]
        public string Year { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Body Type")]
        public string BodyType { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Seater")]
        public string Seater { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Assembly")]
        public string Assembly { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Engine Capacity")]
        public string EngineCapacity { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Engine Type")]
        public string EngineType { get; set; }

        public int CarFeatureID { get; set; }

        public int CarModelCreatedBy { get; set; }

        public int CarModelUpdatedBy { get; set; }

        public string CarModelCreatedOn { get; set; }

        public string CarModelUpdatedOn { get; set; }

        public bool CarModelIsactive { get; set; }

        public bool CarModelIsarchive { get; set; }

        public int tblCarFeatureID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }

        [Display(Name = "Air Conditioned")]
        public bool AirConditioned { get; set; }

        [Display(Name = "ABS")]
        public bool ABS { get; set; }

        [Display(Name = "Air Bag")]
        public bool AirBag { get; set; }

        [Display(Name = "Power Windows")]
        public bool PowerWindows { get; set; }

        [Display(Name = "Power Mirrors")]
        public bool PowerMirrors { get; set; }

        [Display(Name = "Power Locks")]
        public bool PowerLocks { get; set; }

        [Display(Name = "Power Steering")]
        public bool PowerSteering { get; set; }

        [Display(Name = "Immbilizer Key")]
        public bool ImmobilizerKey { get; set; }

        [Display(Name = "Radio")]
        public bool Radio { get; set; }

        [Display(Name = "Key Less Entry")]
        public bool KeyLessEntry { get; set; }

        [Display(Name = "Alloy Rims")]
        public bool AlloyRims { get; set; }

        [Display(Name = "Cool Box")]
        public bool CoolBox { get; set; }

        [Display(Name = "Cruise Control")]
        public bool CruiseControl { get; set; }

        [Display(Name = "Sun Roof")]
        public bool SunRoof { get; set; }

        [Display(Name = "Navigation System")]
        public bool NavigationSystem { get; set; }

        [Display(Name = "Rear ACVents")]
        public bool RearAcVents { get; set; }

        [Display(Name = "Front Camera")]
        public bool FrontCam { get; set; }

        [Display(Name = "Cassest Player")]
        public bool CassetPlayer { get; set; }

        [Display(Name = "Dvd Player")]
        public bool DvdPlayer { get; set; }

        [Display(Name = "Steering Switch")]
        public bool SteeringSwitch { get; set; }

        [Display(Name = "CD Player")]
        public bool CdPlayer { get; set; }

        [Display(Name = "Climate Control")]
        public bool ClimateControl { get; set; }

        [Display(Name = "Front Speaker")]
        public bool FrontSpeaker { get; set; }

        [Display(Name = "Heated Seat")]
        public bool HeatedSeat { get; set; }

        [Display(Name = "Rear Camera")]
        public bool RearCamera { get; set; }

        [Display(Name = "Rear Seat Entertain")]
        public bool RearSeatEntertain { get; set; }

        [Display(Name = "Rear Speaker")]
        public bool RearSpeaker { get; set; }

        [Display(Name = "Box Usb Aux")]
        public bool BoxUsbAux { get; set; }

        public int CarFeatureCreatedBy { get; set; }

        public int CarFeatureUpdatedBy { get; set; }

        public string CarFeatureCreatedOn { get; set; }

        public string CarFeatureUpdatedOn { get; set; }

        public bool CarFeatureIsactive { get; set; }

        public string ShowroomImage { get; set; }

        public string ShowroomName { get; set; }

        public string ShowroomEmail { get; set; }

        public string ShowroomNumber { get; set; }

        public System.DateTime ShowroomCreatedOn { get; set; }

        public string ShowroomURL { get; set; }

        public bool CarFeatureIsarchive { get; set; }

        public int tblCarImagesID { get; set; }

        public string CarImage { get; set; }

        public int tblCarImagesCarID { get; set; }

        public virtual tblCarFeature tblCarFeature { get; set; }

        public virtual ICollection<tblCar> tblCars { get; set; }

        public virtual ICollection<tblCarModel> tblCarModels { get; set; }

        public virtual tblAddress tblAddress { get; set; }

        public virtual ICollection<tblAppointment> tblAppointments { get; set; }

        public virtual ICollection<tblCarImage> tblCarImages { get; set; }

        public virtual tblShowroom tblShowroom { get; set; }

        public virtual ICollection<tblFavAdd> tblFavAdds { get; set; }

        public virtual tblCategory tblCategory { get; set; }

        public virtual tblManfacturerCarModel tblManfacturerCarModel { get; set; }

        public virtual tblManufacturer tblManufacturer { get; set; }

        public virtual tblSubCategory tblSubCategory { get; set; }

        public virtual tblCarModel tblCarModel { get; set; }
        
        public bool? ShowroomActive { get; set; }
    }

}
