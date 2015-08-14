using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using MealsToGo.Models;

namespace MealsToGo.Service
{
    public interface IMealItemService:IService<MealItem> 
    {
        int AddAndReturnID(MealItem entity);
        IEnumerable<LKUPServingUnit> GetServingUnitDDList();
        IEnumerable<LKUPMealType> MealTypeDDList();
        IEnumerable<LKUPDietType> DietTypeDDList();
        IEnumerable<LKUPCuisineType> CuisineTypeDDList();
        IEnumerable<LKUPAllergenicFood> AllergenicFoodsDDList();
       


    }
}

