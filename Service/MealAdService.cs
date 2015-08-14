using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using MealsToGo.Repository;
using System.Linq.Expressions;
using System.IO;
using System.Data;
using System.Data.SqlClient;


namespace MealsToGo.Service
{
    public class MealAdService : IMealAdService
    {
        //  private readonly MealAdRepository MealAdrepos;
        private UnitOfWork unitOfWork = new UnitOfWork();

        public MealAdService()
        {
        }

        public IEnumerable<MealAd> FindByUser(long userid)
        {
            return unitOfWork.mealadrepository.FindByUser(userid);
        }

        public MealAd GetById(long Id)
        {
            return unitOfWork.mealadrepository.GetById(Id);


        }

        public IEnumerable<MealAd> GetAll()
        {
            return unitOfWork.mealadrepository.GetAll();


        }

        public IEnumerable<MealAd> Find(Expression<Func<MealAd, bool>> predicate)
        {
            return unitOfWork.mealadrepository.Find(predicate);

        }

        public IEnumerable<LKUPDeliveryMethod> GetDeliveryMethodDDList()
        {
            return unitOfWork.DeliveryMethodRepository.GetAll();


        }

        public IEnumerable<PaymentOption> PaymentMethodDDList()
        {
            return unitOfWork.PaymentMethodRepository.GetAll();

        }

    


        public void Add(MealAd MealAd)
        {

            var fileName = "";

          
            unitOfWork.mealadrepository.Add(MealAd);
            unitOfWork.Save();
            unitOfWork.Dispose();

        }


         public int AddAndReturnID(MealAd mealad)
        {
             // mealad.dc = DateTime.Now;
              //mealitem.Status = 1;
             // mealitem.Price =10.0M;
            
              unitOfWork.mealadrepository.Add(mealad);
              unitOfWork.Save();

              int mealadid = mealad.MealAdID;
            
            

          //   unitOfWork.Save();

            
             foreach (var payment in mealad.MealAds_PaymentOptions)
             {

                 payment.MealAdID = mealadid;
                 unitOfWork.MealPaymentRepository.Add(payment);
             }
             unitOfWork.Save();

              foreach (var deliverymethod in mealad.MealAds_DeliveryMethods)
             {

                 deliverymethod.MealAdID = mealadid;
                 unitOfWork.MealadDeliveryRepository.Add(deliverymethod);
             }

              foreach (var schedules in mealad.MealAd_Schedules)
              {

                  schedules.MealAdID = mealadid;
                  unitOfWork.MealScheduleRepository.Add(schedules);
              }
             unitOfWork.Save();
             try
             {
                 unitOfWork.mealadrepository.ExecWithStoreProcedureNoResults("PopulateActiveMealAd @MealAdID,@CRUD", new SqlParameter("MealAdID", SqlDbType.BigInt) { Value = mealad.MealAdID },
                     new SqlParameter("CRUD", SqlDbType.Char) { Value = "C" }
                     );

             }
             catch (Exception ex)
             {
                 string msg = ex.Message;

             }
             unitOfWork.Dispose();
            

            return mealadid;

        }
   

               
                

        public void Delete(MealAd mt)
        {
            throw new NotImplementedException();
        }


        public void Update(MealAd mt)
        {
            throw new NotImplementedException();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    unitOfWork.Dispose();
        //    base.Dispose(disposing);
        //}

    }
}