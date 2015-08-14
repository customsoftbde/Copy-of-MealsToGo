using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Data;
using System.Data.Objects.DataClasses;
using MealsToGo.Models;
using System.Data.Entity;



namespace MealsToGo.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbset;

        public Repository(DbContext context)
        {

            if (context == null)
                throw new ArgumentNullException("context");

            else
            {
                _context = context;
                _dbset = context.Set<T>();
            }
        }

        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(query, parameters);
        }

        public void ExecWithStoreProcedureNoResults(string query, params object[] parameters)
        {
            _context.Database.ExecuteSqlCommand(query, parameters);
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);

        }

        public virtual void Delete(T entity)
        {
            var entry = _context.Entry(entity);
            entry.State = System.Data.EntityState.Deleted;
        }

        public virtual void Update(T entity)
        {
            var entry = _context.Entry(entity);
            _dbset.Attach(entity);
            entry.State = System.Data.EntityState.Modified;
        }

        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

      


       

    }
}
