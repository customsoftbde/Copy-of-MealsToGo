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
    
    public partial class SendText
    {
        public int SendTextID { get; set; }
        public string SenderPhone { get; set; }
        public string RecipientPhone { get; set; }
        public string Message { get; set; }
        public System.DateTime DeliveryTime { get; set; }
        public string DeliveryMethod { get; set; }
        public string Status { get; set; }
        public System.DateTime SendDate { get; set; }
    }
}