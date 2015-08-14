using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Linq.Expressions;
using System.Data.Objects;

namespace MealsToGo.Repository
{
    public interface IDataRepository<T> where T : class
    {
        //--Search Operations
        IQueryable<T> GetAll();
        IEnumerable<T> GetAllList();
        IEnumerable<T> Get(Expression<Func<T, bool>> filter);
        T GetIt(Expression<Func<T, bool>> filter);
        T GetById(object id);


        //--CRUD Operations
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        void SaveChanges();
        void SaveChanges(SaveOptions options);
        
    }
}