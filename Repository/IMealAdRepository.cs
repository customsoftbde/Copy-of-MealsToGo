using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using MealsToGo.Models;

namespace MealsToGo.Repository
{
    public interface IMealAdRepository : IRepository<MealAd>
    {

        IQueryable<MealAd> FindByUser(long userid);
        IQueryable<LKUPDeliveryMethod> GetDeliveryMethod(long mealadid);
        //IEnumerable<MealAd> ExecWithStoreProcedure(string procedurename, params object[] parameters);
       

    }
}