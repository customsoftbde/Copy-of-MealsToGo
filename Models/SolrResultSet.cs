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
using SolrNet.Attributes;

namespace MealsToGo.Models
{
    public class SolrResultSet
    {

        [SolrUniqueKey("MealAdID")]
        public int MealAdID { get; set; }

        [SolrField("Item")]
        public string MealItemName { get; set; }

        [SolrField("Cuisine")]
        public string Cuisine { get; set; }

        [SolrField("Meal")]
        public string MealType { get; set; }

        [SolrField("Price")]
        public decimal Price { get; set; }

        [SolrField("PickUpTime")]
        public DateTime Timestamp { get; set; }

        [SolrField("Description")]
        public string Description { get; set; }

        [SolrField("Ingredients")]
        public string Ingredients { get; set; }

        [SolrField("AllergenicIngredients")]
        public string AllergenicIngredients { get; set; }

        [SolrField("PriceRange")]
        public string PriceRange { get; set; }

        [SolrField("Diet")]
        public string FoodType { get; set; }


        [SolrField("Country")]
        public string Country { get; set; }

        [SolrField("latlng")]
        public string latlng { get; set; }

        [SolrField("Distance")]
        public string Distance { get; set; }

        [SolrField("Provider")]
        public string ProviderType { get; set; }

        [SolrField("ProviderName")]
        public string ProviderName { get; set; }

        [SolrField("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [SolrField("FullAddress")]
        public string FullAddress { get; set; }



    }
}