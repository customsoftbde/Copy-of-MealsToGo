using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MealsToGo.ViewModels
{
    public class MealAdViewModel
    {


        public int MealAdID { get; set; }
        
        public string MealItemName { get; set; }
        [Required]
        public int PlacedOrder { get; set; }
        [Required] 
        public int MaxOrders { get; set; }
        [Required]
        
      
       
        
        public int Status { get; set; }
        [Required]


        public List<MealAdSchedule> MealAdSchedules { get; set; }
       
        public List<DeliveryMethodViewModel> DeliveryMethods { get; set; }
        public List<PaymentMethodViewModel> PaymentMethods { get; set; }
        public AvailabilityTypeViewModel AvailabilityTypeDD { get; set; }
        public MealItemsViewModel MealItemsDD { get; set; }


        public MealAdViewModel()
        {
            MealItemsDD = new MealItemsViewModel();
            AvailabilityTypeDD = new AvailabilityTypeViewModel();
            List<MealAdSchedule> MealAdSchedules = new List<MealAdSchedule>();
           
            
           
        }
    }
    public class AvailabilityTypeViewModel
    {
        public string SelectedAvailabilityType { get; set; }
        public IEnumerable<SelectListItem> AvailabilityTypeDDList { get; set; }
    }
    public class MealItemsViewModel
    {
        public string SelectedMealItem { get; set; }
        public IEnumerable<SelectListItem> MealItemsDDList { get; set; }
    }

    public class PaymentMethodViewModel
    {
        public string PaymentMethod { get; set; }
        public int PaymentMethodID { get; set; }
        public bool Selected { get; set; }
    }

    public class DeliveryMethodViewModel
    {
        public string DeliveryMethod { get; set; }
        public int DeliveryMethodID { get; set; }
        public bool Selected { get; set; }
    }

    public class MealAdSchedule
    { 
        public System.DateTime PickUpStartDateTime { get; set; }
        
        public System.DateTime PickUpEndDateTime { get; set; }
       
        public System.DateTime LastOrderDateTime { get; set; }

       
     public MealAdSchedule()
        {
            PickUpStartDateTime = DateTime.Now.AddDays(4);
            PickUpEndDateTime = DateTime.Now.AddDays(4);
            LastOrderDateTime = DateTime.Now.AddDays(4);
           
            
           
        }
       
    }


}


    
