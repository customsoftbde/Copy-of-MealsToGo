using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using MealsToGo.Models;

namespace MealsToGo.Repository
{
    public interface IUserSettingRepository : IRepository<UserSetting>
    {

        IQueryable<UserSetting> FindByUser(long userid);
      
    }
}