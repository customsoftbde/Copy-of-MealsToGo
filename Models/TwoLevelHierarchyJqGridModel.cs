

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;

namespace JQGridMVCExamples.Models
{
    public class TwoLevelHierarchyJqGridModel
    {
        public JQGrid CustomersGrid { get; set; }
        public JQGrid OrdersGrid { get; set; }

        public TwoLevelHierarchyJqGridModel()
        {
            CustomersGrid = new JQGrid
            {
                Columns = new List<JQGridColumn>()
                                 {
                                     new JQGridColumn { DataField = "ProviderName",
                                                        HeaderText = "Provider Name",
                                                        PrimaryKey = true,
                                                        Width = 50 
                                                        },
                                    // new JQGridColumn { DataField = "ProviderName" },
                                     new JQGridColumn { DataField = "ProviderType" }//,
                                     //new JQGridColumn { DataField = "Distance" },
                                     //new JQGridColumn { DataField = "Country" },
                                     //new JQGridColumn { DataField = "latlng" }
                                 },
                 Width = Unit.Pixel(550),
                 Height = Unit.Pixel(300)
            };

            OrdersGrid = new JQGrid
            {
                Columns = new List<JQGridColumn>()
                                 {
                                     new JQGridColumn { DataField = "MealItemName", 
                                                        // always set PrimaryKey for Add,Edit,Delete operations
                                                        // if not set, the first column will be assumed as primary key
                                                        PrimaryKey = true,
                                                        Editable = false,
                                                        Width = 50 },
                                     //new JQGridColumn { DataField = "Timestamp", 
                                     //                   Editable = false,
                                     //                   Width = 100, 
                                     //                   DataFormatString = "{0:d}" },
                                     new JQGridColumn { DataField = "FoodType", 
                                                        Editable = false,
                                                        Width = 50 },
                                     new JQGridColumn { DataField = "CuisineType", 
                                                        Editable = false,
                                                        Width = 75 },
                                     new JQGridColumn { DataField = "MealType",
                                                        Editable =  false,
                                                        Width = 100
                                                      }                                     
                                 },
                Width = Unit.Pixel(450),
                Height = Unit.Percentage(100)
            };
  
        }
    }
}

