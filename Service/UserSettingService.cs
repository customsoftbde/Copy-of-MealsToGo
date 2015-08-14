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
    public class UserSettingService : IUserSettingService
    {
        //  private readonly UserSettingRepository UserSettingrepos;
        private UnitOfWork unitOfWork = new UnitOfWork();

        public UserSettingService()
        {
        }

        public IEnumerable<UserSetting> FindByUser(long userid)
        {
            return unitOfWork.usersettingrepository.FindByUser(userid);
        }

        public UserSetting GetById(long Id)
        {
            return unitOfWork.usersettingrepository.GetById(Id);


        }

        public IEnumerable<UserSetting> GetAll()
        {
            return unitOfWork.usersettingrepository.GetAll();


        }

        public IEnumerable<UserSetting> Find(Expression<Func<UserSetting, bool>> predicate)
        {
            return unitOfWork.usersettingrepository.Find(predicate);




        }

        public IEnumerable<LKUPDeliveryMethod> GetDeliveryMethodDDList()
        {
            return unitOfWork.DeliveryMethodRepository.GetAll();


        }

    


        public void Add(UserSetting UserSetting)
        {

            var fileName = "";

              unitOfWork.usersettingrepository.Add(UserSetting);
            unitOfWork.Save();
            unitOfWork.Dispose();

        }







        public void Delete(UserSetting mt)
        {
            throw new NotImplementedException();
        }


        public void Update(UserSetting mt)
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