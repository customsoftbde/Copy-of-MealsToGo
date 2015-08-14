using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MealsToGo.ViewModels
{
    public class AddressViewModel
    {
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string Zip { get; set; }
        //[Required]
        [RegularExpression(@"[0-9]*", ErrorMessage = "{0} must be a Number.")]
        public string Telephone { get; set; }
        [Required]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string Country { get; set; }
        public string KitchenType { get; set; }
        
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }


    }
}


