using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using MealsToGo.Repository;
using System.Linq.Expressions;
using System.IO;


namespace MealsToGo.Service
{
    public class MealItemService : IMealItemService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
      
        public MealItemService()
        { 
         }

        public IEnumerable<MealItem> FindByUser(long userid)
        {
            return unitOfWork.mealitemrepository.FindByUser(userid);
        }

        public MealItem GetById(long Id)
        {
            return unitOfWork.mealitemrepository.GetById(Id);
          
        
        }

        public IEnumerable<MealItem> GetAll()
        {
            return unitOfWork.mealitemrepository.GetAll();


        }

        public IEnumerable<MealItem> Find(Expression<Func<MealItem, bool>> predicate)
      
        {
            return unitOfWork.mealitemrepository.Find(predicate);
                
        }

        public IEnumerable<LKUPServingUnit> GetServingUnitDDList()
        {
            return unitOfWork.ServUnitRepos.GetAll();


        }

        public IEnumerable<LKUPMealType> MealTypeDDList()
        {
            return unitOfWork.MealTypeRepository.GetAll();


        }

        public IEnumerable<LKUPDietType> DietTypeDDList()
        {
            return unitOfWork.DietTypeRepository.GetAll();


        }

        public IEnumerable<LKUPCuisineType> CuisineTypeDDList()
        {
            return unitOfWork.CuisineTypeRepository.GetAll();


        }

        public IEnumerable<LKUPAllergenicFood> AllergenicFoodsDDList()
        {
            return unitOfWork.AllergenicRepository.GetAll();


        }

        public void Add(MealItem mealitem)
        {

            mealitem.DateCreated = DateTime.Now;
            //mealitem.Status = 1;
            //mealitem.Price = 10.0M;

            unitOfWork.mealitemrepository.Add(mealitem);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }

       

        public int AddAndReturnID(MealItem mealitem)
        {
              mealitem.DateCreated = DateTime.Now;
              //mealitem.Status = 1;
              //mealitem.Price =10.0M;
            
              unitOfWork.mealitemrepository.Add(mealitem);
              unitOfWork.Save();

              int mealitemid = mealitem.MealItemId;
            
            

           //  unitOfWork.Save();

             foreach (var photo in mealitem.MealItems_Photos)
             {
                 photo.MealItemID = mealitemid;
                 unitOfWork.MealItemPhotoRepository.Add(photo);
             }
             foreach (var aller in mealitem.MealItems_AllergenicFoods)
             {

                 aller.MealItemID = mealitemid;
                 unitOfWork.MealAllergenRepository.Add(aller);
             }
             unitOfWork.Save();
             unitOfWork.Dispose();

            return mealitemid;

        }
   

        public void Delete (MealItem mt)
        {
            throw new NotImplementedException();
        }


        public void Update(MealItem mt)
        {
            unitOfWork.mealitemrepository.Update(mt);
            unitOfWork.Save();
            unitOfWork.Dispose();
            //unitOfWork.Save();
            //unitOfWork.Dispose();
            //throw new NotImplementedException();
        }

      
       
    }
}