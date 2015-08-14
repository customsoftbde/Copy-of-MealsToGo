using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using System.Data.Entity;

namespace MealsToGo.Repository
{
    public class MealItemRepository : Repository<MealItem>, IMealItemRepository
    {

        private ThreeSixtyTwoEntities _context;// = new ThreeSixtyTwoEntities();
        private readonly DbSet<MealItem> _dbset;
        private readonly DbSet<LKUPAllergenicFood> _allergenicset;
        private readonly DbSet<MealItems_AllergenicFoods> _mealallergenset;
        private readonly DbSet<MealItems_Photos> _mealphoto;


        public MealItemRepository(DbContext _context)
            : base(_context)
           
        {
            _context = new ThreeSixtyTwoEntities();
            _dbset = _context.Set<MealItem>();
            _mealallergenset = _context.Set<MealItems_AllergenicFoods>();
            _mealphoto = _context.Set<MealItems_Photos>();
      
        }

        public IQueryable<MealItem> FindByUser(long userid)
        {
            return _dbset.Where(x => x.UserId == userid);
        }

       

        public IEnumerable<LKUPAllergenicFood> GetAllergenicFoodItems(long mealitemid)
        {

            return _mealallergenset.Where(x => x.MealItemID == mealitemid)        // source
            .Join(_allergenicset,         // target
             c => c.AllergenicFoodID,          // FK
             cm => cm.AllergenicFoodID,   // PK
            (c, cm) => new { LKUPAllergenicFood = cm }).Select(x => x.LKUPAllergenicFood);


        }

      
    }
}

