using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using System.Linq.Expressions;

namespace MealsToGo.Service
{
    public interface IMealAdService:IService<MealAd> 
  
    {
        //void Add(MealAd entity);
        //void Delete(MealAd entity);
        //void Update(MealAd entity);
        //MealAd GetById(long Id);
        //IEnumerable<MealAd> FindByUser(long userid);
        //IEnumerable<MealAd> GetAll();
        //IEnumerable<MealAd> Find(Expression<Func<MealAd, bool>> predicate);
        int AddAndReturnID(MealAd entity);
        IEnumerable<LKUPDeliveryMethod> GetDeliveryMethodDDList();
        IEnumerable<PaymentOption> PaymentMethodDDList();
      
       


    }
}

