using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealsToGo.Models
{
    public class ListOfEmailaddressModel
    {

        public int UserId;
        public List<string> Emailaddresses { get; set; }
    }
}