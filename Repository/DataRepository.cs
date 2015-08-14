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
}
   
//        public class DataRepository<TContext> : IDataRepository<TContext> where TContext : ObjectContext
//{
//    // Cached ObjectSets so changes persist
//    protected Dictionary<string, object> CachedObjects = new Dictionary<string, object>();
//    protected ObjectSet<TEntity> GetObjectSet<TEntity>() where TEntity : EntityObject
//    {
//        var fulltypename = typeof (TEntity).AssemblyQualifiedName;
//        if (fulltypename == null)
//            throw new ArgumentException(&quot;Invalid Type passed to GetObjectSet!&quot;);
//        if (!CachedObjects.ContainsKey(fulltypename))
//        {
//            var objectset = _context.CreateObjectSet<TEntity>();
//            CachedObjects.Add(fulltypename, objectset);
//        }
//        return CachedObjects[fulltypename] as ObjectSet<TEntity>;
//    }
 
//    protected TContext _context;
//    /// <summary>
//    /// Constructor that takes a context
//    /// </summary>
//    /// <param name=&quot;context&quot;>An established data context</param>
//    public DataRepository(TContext context)
//    {
//        _context = context;
//    }
 
//    /// <summary>
//    /// Constructor that takes a connection string and an EDMX name
//    /// </summary>
//    /// <param name=&quot;connectionString&quot;>The connection string</param>
//    /// <param name=&quot;edmxName&quot;>The name of the EDMX so we can build an Entity Connection string</param>
//    public DataRepository(string connectionString, string edmxName)
//    {
//        var entityConnection =
//            String.Format(
//                &quot;metadata=res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl;provider=System.Data.SqlClient;provider connection string=&quot;,
//                edmxName);
 
//        // append the database connection string and save
//        entityConnection = entityConnection + &quot;\&quot;&quot; + connectionString + &quot;\&quot;&quot;;
//        var targetType = typeof (TContext);
//        var ctx = Activator.CreateInstance(targetType, entityConnection);
//        _context = (TContext) ctx;
//    }
 
//    public IQueryable<TEntity> Fetch<TEntity>() where TEntity : EntityObject
//    {
//        return GetObjectSet<TEntity>();
//    }
 
//    public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : EntityObject
//    {
 
//        return GetObjectSet<TEntity>().AsEnumerable();
//    }
 
//    public IEnumerable<TEntity> Find<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
//    {
//        return GetObjectSet<TEntity>().Where(predicate);
//    }
 
//    public TEntity GetSingle<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
//    {
//        return GetObjectSet<TEntity>().Single(predicate);
//    }
 
//    public TEntity GetFirst<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
//    {
//        return GetObjectSet<TEntity>().First(predicate);
//    }
 
//    public IEnumerable<TEntity> GetLookup<TEntity>() where TEntity : EntityObject
//    {
//        return GetObjectSet<TEntity>().ToList();
//    }
 
//    public void Add<TEntity>(TEntity entity) where TEntity : EntityObject
//    {
//        if (entity == null)
//            throw new ArgumentException(&quot;Cannot add a null entity&quot;);
 
//        GetObjectSet<TEntity>().AddObject(entity);
//    }
 
//    public void Delete<TEntity>(TEntity entity) where TEntity : EntityObject
//    {
//        if (entity == null)
//            throw new ArgumentException(&quot;Cannot delete a null entity&quot;);
 
//        GetObjectSet<TEntity>().DeleteObject(entity);
//    }
 
//    public void Attach<TEntity>(TEntity entity) where TEntity : EntityObject
//    {
//        if (entity == null)
//            throw new ArgumentException(&quot;Cannot attach a null entity&quot;);
 
//        GetObjectSet<TEntity>().Attach(entity);
//    }
 
//    public void SaveChanges()
//    {
//        SaveChanges(SaveOptions.None);
//    }
 
//    public void SaveChanges(SaveOptions options)
//    {
//        _context.SaveChanges(options);
//    }
 
//    public void Refresh<TEntity>(TEntity entity) where TEntity : EntityObject
//    {
//        _context.Refresh(RefreshMode.StoreWins, entity);
//    }
 
//    public void Refresh<TEntity>(IEnumerable<TEntity> entities) where TEntity : EntityObject
//    {
//        _context.Refresh(RefreshMode.StoreWins, entities);
//    }
 
//    #region IDisposable implementation
//    private bool disposedValue;
//    protected void Dispose(bool disposing)
//    {
//        if (!this.disposedValue)
//        {
//            if (disposing)
//            {
//                // dispose managed state here if required
//            }
//            // dispose unmanaged objects and set large fields to null
//        }
//        this.disposedValue = true;
//    }
 
//    public void Dispose()
//    {
//        Dispose(true);
//        GC.SuppressFinalize(this);
//    }
//    #endregion
//}
//    }
