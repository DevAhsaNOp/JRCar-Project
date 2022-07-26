using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BOL.Validation_Classes
{
    public class ValidateUserAdsImage
    {
        public int ID { get; set; }

        [Display(Name ="Image")]
        public string Image { get; set; }

        public int tblUserAddID { get; set; }

        public virtual tblUserAdd tblUserAdd { get; set; }
    }
}
