using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using System.Web.Mvc;

namespace MealsToGo.Models
{
    public class SearchParam {
          public const int DefaultPageSize = 10;

    

         public SearchParam() {
          
             Facets = new Dictionary<string, string>();
             //FacetDates = new Dictionary<string, string>();
             //FacetQueries = new Dictionary<string, string>();
         //   UserLoc = new GLatLong();
            PageSize = DefaultPageSize;
            PageIndex = 1;
            PickUpDateSearch = DateTime.Now.Date;
           // UserID = -1;
          
           
        }

        //  public int UserID { get; set; }
        //public GLatLong UserLoc { get; set; }
       // public string CurrLoc { get; set; }
        public string FreeSearch { get; set; }
        public string DistanceSearch { get; set; }
        public DateTime PickUpDateSearch { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IDictionary<string, string> Facets { get; set; }
        //public IDictionary<string, string> FacetDates { get; set; }
        //public IDictionary<string, string> FacetQueries { get; set; }
        public string Sort { get; set; }

        public int FirstItemIndex {
            get {
                return (PageIndex-1)*PageSize;
            }
        }

       

        public int LastItemIndex {
            get {
                return FirstItemIndex + PageSize;
            }
        }
    }

   
}