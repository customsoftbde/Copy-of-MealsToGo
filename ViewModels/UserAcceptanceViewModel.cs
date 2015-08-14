
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using System.ComponentModel.DataAnnotations;

namespace MealsToGo.ViewModels
{
    public class UserAcceptanceViewModel
    {
        public int UserId { get; set; }
        public int AgreementID { get; set; }
        
        public string UserAgreement { get; set; }
        public DateTime DateofAgreement { get; set; }
     }
}   