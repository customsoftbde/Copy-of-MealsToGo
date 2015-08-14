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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;
using MealsToGo.Helpers;


namespace MealsToGo.Models.Binders {
    //public class SearchParametersBinder : IModelBinder {
    //    public const int DefaultPageSize = SearchParameters.DefaultPageSize;

    //    public IDictionary<string, string> NVToDict(NameValueCollection nv) {
    //        var d = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
    //        foreach (var k in nv.AllKeys)
    //            d[k] = nv[k];
    //        return d;
    //    }

    //    private static readonly Regex FacetRegex = new Regex("^f_", RegexOptions.Compiled | RegexOptions.IgnoreCase);


    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
    //        var qs = controllerContext.HttpContext.Request.QueryString;
    //        var qsDict = NVToDict(qs);
    //        var sp = new SearchParameters {
    //            FreeSearch = StringHelper.EmptyToNull(qs["q"]),
    //            PageIndex = StringHelper.TryParse(qs["page"], 1),
    //            PageSize = StringHelper.TryParse(qs["pageSize"], DefaultPageSize),
    //            Sort = StringHelper.EmptyToNull(qs["sort"]),
    //            Facets = qsDict.Where(k => FacetRegex.IsMatch(k.Key))
    //                .Select(k => k.WithKey(FacetRegex.Replace(k.Key, "")))
    //                .ToDictionary(),
              
    //        };
    //        return sp;
    //    }
    //}

    public class SearchParamBinder : IModelBinder
    {
        public const int DefaultPageSize = SearchParam.DefaultPageSize;


        public IDictionary<string, string> NVToDict(NameValueCollection nv)
        {
            var d = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var k in nv.AllKeys)
                d[k] = nv[k];
            return d;
        }


        public GLatLong GetLatLong(string latlongstring)
        {
            GLatLong latlong = new GLatLong();
            string[] nums = latlongstring.Split(",".ToCharArray()).ToArray();
            latlong.Latitude = Convert.ToDouble(nums[0]);
            latlong.Longitude = Convert.ToDouble(nums[1]);

            return latlong;
        }
        private static readonly Regex FacetRegex = new Regex("^f_", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //private static readonly Regex FacetQueryRegex = new Regex("^facet.query", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

               
            var qs = controllerContext.HttpContext.Request.QueryString;
           
         


            //int  UserID = Convert.ToInt32(StringHelper.EmptyToNull(qs["UserID"]));
          
            //string latlongstring = StringHelper.EmptyToNull(qs["UserLoc"]);
            //GLatLong latlong = new GLatLong();
            //latlong = GetLatLong(latlongstring);


            DateTime PickUpDateSearch = string.IsNullOrEmpty(qs["Search.PickUpDateSearch"]) ? Convert.ToDateTime(qs["PickUpDateSearch"]) : Convert.ToDateTime(qs["Search.PickUpDateSearch"]);

            var qsDict = NVToDict(qs);
            var sp = new SearchParam
            {
                //UserLoc.Latitude = Convert.ToDouble(StringHelper.EmptyToNull(qs["CurrLoc"])),
               // CurrLoc = StringHelper.EmptyToNull(qs["CurrLoc"]),
              //  UserID = WebSecurity.CurrentUserId,
                FreeSearch = StringHelper.EmptyToNull(qs["Search.FreeSearch"]),
                DistanceSearch = StringHelper.EmptyToNull(qs["DistanceDD.SelectedDistanceLimit"]),
                PickUpDateSearch = PickUpDateSearch,
                PageIndex = StringHelper.TryParse(qs["page"], 1),
                PageSize = StringHelper.TryParse(qs["pageSize"], DefaultPageSize),
                Sort = StringHelper.EmptyToNull(qs["sort"]),
                Facets = qsDict.Where(k => FacetRegex.IsMatch(k.Key))
                    .Select(k => k.WithKey(FacetRegex.Replace(k.Key, "")))
                    .ToDictionary(),
              //  UserLoc = latlong
                //FacetQueries = qsDict.Where(k => FacetQueryRegex.IsMatch(k.Key))
                //    .Select(k => k.WithKey(k.Key))
                //    .ToDictionary()
            };
            return sp;
        }
    }
}