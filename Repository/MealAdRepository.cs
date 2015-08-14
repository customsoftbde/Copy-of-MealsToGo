using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using System.Data.Entity;

namespace MealsToGo.Repository
{
    public class MealAdRepository : Repository<MealAd>, IMealAdRepository
    {

        private ThreeSixtyTwoEntities _context;// = new ThreeSixtyTwoEntities();
        private readonly DbSet<MealAd> _dbset;
        private readonly DbSet<LKUPDeliveryMethod> _DeliveryMethodset;

        public MealAdRepository(DbContext _context)
            : base(_context)
        {
            _context = new ThreeSixtyTwoEntities();
            _dbset = _context.Set<MealAd>();
        }

        public IQueryable<MealAd> FindByUser(long userid)
        {
            return _dbset.Where(x => x.MealItem.UserId == userid);
        }

        public IQueryable<LKUPDeliveryMethod> GetDeliveryMethod(long mealadid)
        {
            return _DeliveryMethodset.Where(x =>x.MealAds_DeliveryMethods.Any(m => m.MealAdID == mealadid));
        }

        public IEnumerable<MealAd> ExecWithStoreProcedure(string query, params object[] parameters)
        {
        return _context.Database.SqlQuery<MealAd>(query, parameters);
        }

       

        //protected override void Dispose(bool disposing)
        //{
        //    _context.Dispose();
        //    base.Dispose(disposing);
        //}


    }
}

