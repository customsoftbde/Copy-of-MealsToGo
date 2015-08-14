//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MealsToGo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddressList
    {
        public AddressList()
        {
            this.UserDetails = new HashSet<UserDetail>();
            this.UserDetails1 = new HashSet<UserDetail>();
        }
    
        public int AddressID { get; set; }
        public int UserId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Zip { get; set; }
        public string Telephone { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int IsBillingAddress { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public string latlng { get; set; }
        public int IsCurrent { get; set; }
    
        public virtual LKUPCountry LKUPCountry { get; set; }
        public virtual ICollection<UserDetail> UserDetails { get; set; }
        public virtual ICollection<UserDetail> UserDetails1 { get; set; }
    }
}
