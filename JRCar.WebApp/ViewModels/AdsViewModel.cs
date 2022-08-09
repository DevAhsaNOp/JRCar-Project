using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JRCar.BOL;
using JRCar.BLL.Repositories;
using JRCar.BOL.Validation_Classes;
using System.IO;

namespace JRCar.WebApp.ViewModels
{
    public class AdsViewModel
    {
        public int MaximumPrice { get; set; }

        public List<ValidationUserAds> ads { get; set; }

        public int? SortBy { get; set; }

        public int? CategoryID { get; set; }

        public string SearchTerm { get; set; }
    }
}