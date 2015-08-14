using System;
using System.Collections.Generic;
using MealsToGo.Models;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;


namespace MealsToGo.Repository
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<UserDetail> GetUsers();
        UserDetail GetUserByID(int userId);
        void InsertUser(UserDetail user);
        void DeleteUser(int userId);
        void UpdateUser(UserDetail user);
        void Save();
    }


    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(long Id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> ExecWithStoreProcedure(string procedurename, params object[] parameters);
        void ExecWithStoreProcedureNoResults(string procedurename, params object[] parameters);
        
      
    }
    }



