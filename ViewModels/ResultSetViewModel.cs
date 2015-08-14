#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System.Collections.Generic;
using SolrNet;
using System.Web.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using MealsToGo.Models;
using JQGridMVCExamples.Models;


namespace MealsToGo.ViewModels
{
    public class ResultSetViewModel
    {
        public SearchParam Search { get; set; }
        

        //public TwoLevelHierarchyJqGridModel providerproductjqmodel { get; set; }
        public int TotalCount { get; set; }
        public IDictionary<string, ICollection<KeyValuePair<string, int>>> Facets { get; set; }
        //public IDictionary<string, int> PickUpTimeFacet { get; set; }
        // public IDictionary<string, int> DistanceFacet { get; set; }
        public DistanceViewModel DistanceDD { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PickUpDate { get; set; }
        public List<Provider> ProviderList { get; set; }

        public string DidYouMean { get; set; }
        public bool QueryError { get; set; }

        public ResultSetViewModel()
        {
            Search = new SearchParam();
            DistanceDD = new DistanceViewModel();
            //  TimeDD = new TimeViewModel();
            Facets = new Dictionary<string, ICollection<KeyValuePair<string, int>>>();
           // providerproductjqmodel = new TwoLevelHierarchyJqGridModel();
            //PickUpTimeFacet = new Dictionary<string, int>();
            //DistanceFacet = new Dictionary<string,int>();
            ProviderList = new List<Provider>();
           // ProductList = new List<Product>();
            PickUpDate = DateTime.Now.Date;
        }
    }

    public class DistanceViewModel
    {
        public string SelectedDistanceLimit { get; set; }
        public IEnumerable<SelectListItem> DistanceDDList { get; set; }

        public DistanceViewModel()
        {
            DistanceDDList = new[]
                {
                    new SelectListItem { Value = "1", Text = "1 mile" },
                    new SelectListItem { Value = "2", Text = "2 miles" },
                    new SelectListItem { Value = "5", Text = "5 miles" },
                    new SelectListItem { Value = "10", Text = "10 miles" },
                    new SelectListItem { Value = "20", Text = "20 miles" },
                    new SelectListItem { Value = "30", Text = "30 miles" },
                    new SelectListItem { Value = "40", Text = "40 miles" },
                    new SelectListItem { Value = "50", Text = "50 miles" }
                };
        }
    }
    // public class TimeViewModel
    //{
    //    public string SelectedTimeLimit { get; set; }
    //    public IEnumerable<SelectListItem> TimeDDList { get; set; }

    //    public TimeViewModel()
    //    {
    //        TimeDDList = new[]
    //        {
    //            new SelectListItem { Value = "1", Text = "within next 1 day " },
    //            new SelectListItem { Value = "2", Text = "within next 2 days " },
    //            new SelectListItem { Value = "5", Text = "within next 5 days " },
    //             new SelectListItem { Value = "7", Text = "within next 7 days " },
    //            new SelectListItem { Value = "14", Text = "within next 14 days " },
    //            new SelectListItem { Value = "21", Text = "within next 21 days " },
    //             new SelectListItem { Value = "30", Text = "within next 30 days " }
    //        };
    //    }


    //}
    // }
}

