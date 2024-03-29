﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        [Required(ErrorMessage ="*")]
        [Display(Name = "Recievable")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Amount should be Greater than Zero!")]
        public decimal? Recievable { get; set; }
        
        [Required(ErrorMessage ="*")]
        [Display(Name = "Current Amount")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Amount should be Greater than Zero!")]
        public decimal? CAmount { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Recieved")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Amount should be Greater than Zero!")]
        public decimal? Recieved { get; set; }

        [Display(Name = "Discount")]
        public decimal? Discount { get; set; }

        [Display(Name = "Balance")]
        public decimal? Balance { get; set; }

        public decimal? PBalance { get; set; }

        public Nullable<System.DateTime> RecievableFromDate { get; set; }

        public Nullable<System.DateTime> RecievableToDate { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Recieved Month")]
        public List<string> RecievedDate { get; set; }

        public List<DatesD> RecievableDate { get; set; }

        public List<DatesD> RecievedDates { get; set; }

        public List<DatesD> datesDs { get; set; }

        public Nullable<System.DateTime> RecievedFromDate { get; set; }

        public Nullable<System.DateTime> RecievedToDate { get; set; }
        
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? Isactive { get; set; }
        public bool? Isarchive { get; set; }

        public virtual tblShowroom tblShowroom { get; set; }

        public int ID { get; set; }
    }

    public class DatesD
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public bool IsPaid { get; set; }
        public decimal? Recievable { get; set; }
        public decimal? Recieved { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Balance { get; set; }
    } 
}
