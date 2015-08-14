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

namespace MealsToGo.Models {
    public class Product {

       // [SolrUniqueKey("MealAdID")]
        public int MealAdID { get; set; }

       // [SolrField("MealItemName")]
        public string MealItemName { get; set; }

        [SolrField("Cuisine")]
        public string Cuisine { get; set; }

        [SolrField("Meal")]
        public string MealType { get; set; }
               
        [SolrField("Price")]
        public decimal Price { get; set; }

      //  [SolrField("PickUpTime")]
        public DateTime Timestamp { get; set; }

      //  [SolrField("Description")]
        public string Description { get; set; }

       // [SolrField("Ingredients")]
        public string Ingredients { get; set; }

        [SolrField("Allergen")]
        public string AllergenicIngredients { get; set; }

        [SolrField("PriceRange")]
        public string PriceRange { get; set; }

        [SolrField("Diet")]
        public string FoodType { get; set; }

        public string ProviderName { get; set; }

        
    }
}