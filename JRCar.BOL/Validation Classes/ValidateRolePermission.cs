using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidateRolePermission
    {
        public int ID { get; set; }

        public int RoleID { get; set; }

        [Display(Name ="Role Name")]
        [Required(ErrorMessage ="*")]
        public string Role { get; set; }

        [Display(Name ="Make Annoucment")]
        public bool MakeAnnoucment { get; set; }
        
        [Display(Name ="Show Annoucment")]
        public bool ShowAnnoucment { get; set; }
        
        [Display(Name ="Add User")]
        public bool AddUser { get; set; }
        
        [Display(Name ="Edit User")]
        public bool EditUser { get; set; }
        
        [Display(Name ="Delete User")]
        public bool DeleteUser { get; set; }
        
        [Display(Name ="Show User")]
        public bool ShowUsers { get; set; }
        
        [Display(Name ="Add Showroom")]
        public bool AddShowroom { get; set; }
        
        [Display(Name ="Edit Showroom")]
        public bool EditShowroom { get; set; }
        
        [Display(Name ="Delete Showroom")]
        public bool DeleteShowroom { get; set; }
        
        [Display(Name ="Show Showroom")]
        public bool ShowShowroom { get; set; }
        
        [Display(Name ="Add Union Member")]
        public bool AddUnionMember { get; set; }
        
        [Display(Name ="Edit Union Member")]
        public bool EditUnionMember { get; set; }
        
        [Display(Name ="Delete Union Member")]
        public bool DeleteUnionMember { get; set; }
        
        [Display(Name ="Show Union Member")]
        public bool ShowUnionMember { get; set; }
        
        [Display(Name ="Manage User Ads")]
        public bool ManagUserAds { get; set; }
        
        [Display(Name ="Manage Showroom Ads")]
        public bool ManagShowroomAds { get; set; }
        
        [Display(Name ="Make Payments")]
        public bool MakePayments { get; set; }
        
        [Display(Name ="Show Payments")]
        public bool ShowPayments { get; set; }
        
        [Display(Name ="Edit Profile")]
        public bool EditProfile { get; set; }

        public virtual tblRole tblRole { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
