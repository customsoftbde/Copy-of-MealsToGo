using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using MealsToGo.Models;

namespace MealsToGo.Service
{
    public interface IService<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(long Id);
        IEnumerable<T> FindByUser(long userid);
        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    }
}

