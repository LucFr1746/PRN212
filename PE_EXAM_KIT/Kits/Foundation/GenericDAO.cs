using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PRN212.ExamKit.Foundation
{
    /// <summary>
    /// Generic Data Access Object providing basic CRUD operations.
    /// Requires Nuget Package: Microsoft.EntityFrameworkCore and Microsoft.EntityFrameworkCore.SqlServer
    /// </summary>
    /// <typeparam name="TEntity">The database entity type.</typeparam>
    /// <typeparam name="TContext">The DbContext type.</typeparam>
    public class GenericDAO<TEntity, TContext> 
        where TEntity : class 
        where TContext : DbContext, new()
    {
        /// <summary>
        /// Creates a new instance of the DbContext.
        /// This ensures context operations are isolated and short-lived, preventing memory leaks and stale cache.
        /// </summary>
        protected virtual TContext CreateContext()
        {
            return new TContext();
        }

        /// <summary>
        /// Retrieves all entities from the table.
        /// </summary>
        public virtual List<TEntity> GetAll()
        {
            using (var context = CreateContext())
            {
                return context.Set<TEntity>().ToList();
            }
        }

        /// <summary>
        /// Retrieves a single entity by its primary key.
        /// </summary>
        public virtual TEntity GetById(object id)
        {
            using (var context = CreateContext())
            {
                return context.Set<TEntity>().Find(id);
            }
        }

        /// <summary>
        /// Inserts a new entity into the table.
        /// </summary>
        public virtual void Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            using (var context = CreateContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates an existing entity in the table.
        /// </summary>
        public virtual void Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var context = CreateContext())
            {
                context.Set<TEntity>().Update(entity);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes an entity from the table.
        /// </summary>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var context = CreateContext())
            {
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes an entity by its primary key if it exists.
        /// </summary>
        public virtual void DeleteById(object id)
        {
            using (var context = CreateContext())
            {
                var dbSet = context.Set<TEntity>();
                var entity = dbSet.Find(id);
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    context.SaveChanges();
                }
            }
        }
    }
}
