using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using System.Data.Entity.Validation;

namespace MealsToGo.Repository
{
    public class UnitOfWork : IDisposable
    {
       
        private ThreeSixtyTwoEntities context = new ThreeSixtyTwoEntities();
        private MealItemRepository mealitemrepos;
        private MealAdRepository mealadrepos;
      
        private Repository<LKUPServingUnit> servunitrepos;
        private Repository<LKUPCuisineType> cuisinetyperepos;
        private Repository<LKUPMealType> mealtyperepos;
        private Repository<LKUPAllergenicFood> allergenicrepos;
        private Repository<LKUPDeliveryMethod> deliverymethodrepos;
        private Repository<ActiveMealAd> activemealadrepos;
        private UserSettingRepository usersettingrepos;
        private Repository<LKUPDietType> diettyperepos;
        private Repository<MealItems_Photos> mealitemphotorepos;
        private Repository<MealItems_AllergenicFoods> mealallergenrepos;
        private Repository<MealAds_DeliveryMethods> mealaddeliveryrepos;
         private Repository<PaymentOption> paymentmethodrepos;
         private Repository<MealAds_PaymentOptions> mealpaymentrepos;
         private Repository<MealAd_Schedules> mealschedulesrepos;

         public Repository<MealAd_Schedules> MealScheduleRepository
         {
             get
             {

                 if (this.mealschedulesrepos == null)
                 {
                     this.mealschedulesrepos = new Repository<MealAd_Schedules>(context);
                 }
                 return mealschedulesrepos;
             }
         }



         public Repository<PaymentOption> PaymentMethodRepository
         {
             get
             {

                 if (this.paymentmethodrepos == null)
                 {
                     this.paymentmethodrepos = new Repository<PaymentOption>(context);
                 }
                 return paymentmethodrepos;
             }
         }


         public Repository<MealAds_PaymentOptions> MealPaymentRepository
         {
             get
             {

                 if (this.mealpaymentrepos == null)
                 {
                     this.mealpaymentrepos = new Repository<MealAds_PaymentOptions>(context);
                 }
                 return mealpaymentrepos;
             }
         }
        public Repository<MealAds_DeliveryMethods> MealadDeliveryRepository
        {
            get
            {

                if (this.mealaddeliveryrepos == null)
                {
                    this.mealaddeliveryrepos = new Repository<MealAds_DeliveryMethods>(context);
                }
                return mealaddeliveryrepos;
            }
        }


      


          public Repository<MealItems_AllergenicFoods> MealAllergenRepository
          {
              get
              {

                  if (this.mealallergenrepos == null)
                  {
                      this.mealallergenrepos = new Repository<MealItems_AllergenicFoods>(context);
                  }
                  return mealallergenrepos;
              }
          }

        public Repository<ActiveMealAd> ActiveMealAdRepository
        {
            get
            {

                if (this.activemealadrepos == null)
                {
                    this.activemealadrepos = new Repository<ActiveMealAd>(context);
                }
                return activemealadrepos;
            }
        }

        public Repository<LKUPDeliveryMethod> DeliveryMethodRepository
        {
            get
            {

                if (this.deliverymethodrepos == null)
                {
                    this.deliverymethodrepos = new Repository<LKUPDeliveryMethod>(context);
                }
                return deliverymethodrepos;
            }
        }
        
        public Repository<LKUPMealType> MealTypeRepository
        {
            get
            {

                if (this.mealtyperepos == null)
                {
                    this.mealtyperepos = new Repository<LKUPMealType>(context);
                }
                return mealtyperepos;
            }
        }
        
        public Repository<LKUPAllergenicFood> AllergenicRepository
        {
            get
            {

                if (this.allergenicrepos == null)
                {
                    this.allergenicrepos = new Repository<LKUPAllergenicFood>(context);
                }
                return allergenicrepos;
            }
        }

        public Repository<LKUPCuisineType> CuisineTypeRepository
        {
            get
            {

                if (this.cuisinetyperepos == null)
                {
                    this.cuisinetyperepos = new Repository<LKUPCuisineType>(context);
                }
                return cuisinetyperepos;
            }
        }


        public Repository<LKUPDietType> DietTypeRepository
        {
            get
            {

                if (this.diettyperepos == null)
                {
                    this.diettyperepos = new Repository<LKUPDietType>(context);
                }
                return diettyperepos;
            }
        }

        public Repository<MealItems_Photos> MealItemPhotoRepository
        {
            get
            {

                if (this.mealitemphotorepos == null)
                {
                    this.mealitemphotorepos = new Repository<MealItems_Photos>(context);
                }
                return mealitemphotorepos;
            }

             set
            {

                this.mealitemphotorepos = value;
            }
           
        }

        

        public MealItemRepository mealitemrepository
        {
            get
            {

                if (this.mealitemrepos == null)
                {
                    this.mealitemrepos = new MealItemRepository(context);
                }
                return mealitemrepos;
            }
        }

        public MealAdRepository mealadrepository
        {
            get
            {

                if (this.mealadrepos == null)
                {
                    this.mealadrepos = new MealAdRepository(context);
                }
                return mealadrepos;
            }
        }

        public UserSettingRepository usersettingrepository
        {
            get
            {

                if (this.usersettingrepos == null)
                {
                    this.usersettingrepos = new UserSettingRepository(context);
                }
                return usersettingrepos;
            }
        }

        public Repository<LKUPServingUnit> ServUnitRepos
        {
            get
            {

                if (this.servunitrepos == null)
                {
                    this.servunitrepos = new Repository<LKUPServingUnit>(context);
                }
                return servunitrepos;
            }
        }

        public Repository<ActiveMealAd> ActiveMealAdRepos
        {
            get
            {

                if (this.activemealadrepos == null)
                {
                    this.activemealadrepos = new Repository<ActiveMealAd>(context);
                }
                return activemealadrepos;
            }
        }

        public void Save()
        {


            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string msg = ex.Message;//email support of this error
            }
            catch (SystemException ex)
            {
                string msg = ex.Message;//email support of this error
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
    
