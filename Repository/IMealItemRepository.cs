using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using MealsToGo.Models;

namespace MealsToGo.Repository
{
    public interface IMealItemRepository :IRepository<MealItem>
    {

        IQueryable<MealItem> FindByUser(long userid);
         IEnumerable<LKUPAllergenicFood> GetAllergenicFoodItems(long mealitemid);
       
    }
}