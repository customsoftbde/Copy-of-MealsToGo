using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using System.ComponentModel.DataAnnotations;

namespace MealsToGo.ViewModels
{
    public class UserDetailViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string Notes { get; set; }
        public string Photo { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string AllInfo { get; set; }
        public AddressViewModel Address { get; set; }
        public Nullable<int> GoogleCheckoutID { get; set; }
        public Nullable<int> PayPalID { get; set; }
        public Nullable<int> AmazonPayID { get; set; }
        public bool CheckIfSeller { get; set; }
        public int IsSeller { get; set; }
        public int KitchenTypeID { get; set; }
        public String KitchenType { get; set; }
        public IEnumerable<SelectListItem> KitchenTypeDDList { get; set; }

        public string KitchenName { get; set; }
       // [Required]
        public string SkypeID { get; set; }
       // [Required]
        public string Mobile { get; set; }
        //[Required]
        public string IdentificationNumber { get; set; }
        //[Required]
        public string PassportNumber { get; set; }
        //[Required]
        public string CountryOfIssuance { get; set; }
        public IEnumerable<SelectListItem> CountryDDList { get; set; }

    }
}