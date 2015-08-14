using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;

namespace MealsToGo.Models
{
   public class StandardEmail : Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public string ConfirmationToken { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}