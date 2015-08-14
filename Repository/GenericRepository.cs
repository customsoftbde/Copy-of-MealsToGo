using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;


namespace MealsToGo.Repository
{
    /// <summary>
    /// Repository base class used with dbmeals
    /// </summary>
    /// <typeparam name="TContext">Type of DdContext that this repositiory operates on</typeparam>
    public class EFRepository<TContext> : IDisposable
        where TContext : DbContext, IObjectContextAdapter, new()
    {
        private TContext context;

        private EFRepository()
        {

        }
        /// <summary>
        /// Create new instance of repository
        /// </summary>
        /// <param name="connecstionStringName">Connection string name from .config file</param>
        public EFRepository(string connecstionStringName)
        {
            context = new TContext();
            context.Database.Connection.ConnectionString =
                ConfigurationManager.ConnectionStrings[connecstionStringName].ConnectionString;
        }

        public IEnumerable<TContext> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return context.Database.SqlQuery<TContext>(query, parameters);
        }


        /// <summary>
        /// Dipose repository
        /// </summary>
        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }


        /// <summary>
        /// Select data from database
        /// </summary>
        /// <typeparam name="TItem">Type of data to select</typeparam>
        /// <returns></returns>
        public IQueryable<TItem> Select<TItem>()
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));

            DbSet<TItem> set = property.GetValue(context, null) as DbSet<TItem>;

            return set;
        }

        /// <summary>
        /// Insert new item into database
        /// </summary>
        /// <typeparam name="TItem">Type of item to insert</typeparam>
        /// <param name="item">Item to insert</param>
        /// <returns>Inserted item</returns>
        public TItem Insert<TItem>(TItem item)
            where TItem : class, new()
        {
            DbSet<TItem> set = GetDbSet(typeof(TItem)).GetValue(context, null) as DbSet<TItem>;
            set.Add(item);
            context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Update na item
        /// </summary>
        /// <typeparam name="TItem">Type of item to update</typeparam>
        /// <param name="item">Item to update</param>
        /// <returns>Updated item</returns>
        public TItem Update<TItem>(TItem item)
            where TItem : class, new()
        {
            DbSet<TItem> set = GetDbSet(typeof(TItem)).GetValue(context, null) as DbSet<TItem>;
            set.Attach(item);
            context.Entry(item).State = System.Data.EntityState.Modified;
            context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Delete an item
        /// </summary>
        /// <typeparam name="TItem">Type of item to delete</typeparam>
        /// <param name="item">Item to delete</param>
        public void Delete<TItem>(TItem item)
           where TItem : class, new()
        {
            DbSet<TItem> set = GetDbSet(typeof(TItem)).GetValue(context, null) as DbSet<TItem>;
            var entry = context.Entry(item);
            if (entry != null)
            {
                entry.State = System.Data.EntityState.Deleted;
            }
            else
            {
                set.Attach(item);
            }
            context.Entry(item).State = System.Data.EntityState.Deleted;
            context.SaveChanges();
        }

        private PropertyInfo GetDbSet(Type itemType)
        {
            var properties = typeof(TContext).GetProperties().Where(item => item.PropertyType.Equals(typeof(DbSet<>).MakeGenericType(itemType)));

            return properties.First();
        }

    }
}