using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealsToGo.Models
{
    public class Contact
    {
        public string Name { get; set; }
        public string KitchenName { get; set; }
        public string CuisineType { get; set; }
        public string MostPopularDish { get; set; }
        public string FavoriteDish { get; set; }
        public string EmailContact { get; set; }
        //public string CuisineType { get; set; }
        public string GetNotified { get; set; }
       

    }
}