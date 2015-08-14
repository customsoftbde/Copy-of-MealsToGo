using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using System.Data.Entity;

namespace MealsToGo.Repository
{
    public class UserSettingRepository : Repository<UserSetting>, IUserSettingRepository
    {

        private ThreeSixtyTwoEntities _context;// = new ThreeSixtyTwoEntities();
        private readonly DbSet<UserSetting> _dbset;
        //private readonly DbSet<LKUPDeliveryMethod> _DeliveryMethodset;

        public UserSettingRepository(ThreeSixtyTwoEntities _context)
            : base(_context)
        {
            _context = new ThreeSixtyTwoEntities();
            _dbset = _context.Set<UserSetting>();
        }

        public IQueryable<UserSetting> FindByUser(long userid)
        {
            return _dbset.Where(x => x.UserID==userid);
        }

     

        //protected override void Dispose(bool disposing)
        //{
        //    _context.Dispose();
        //    base.Dispose(disposing);
        //}


    }
}

