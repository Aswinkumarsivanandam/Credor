
#region Using Namespaces...

using Credor.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#endregion

namespace Credor.Web.API.Common.GenericRepository
{
    /// <summary>
    /// Generic Repository class for Entity Operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        #region Private member variables...
        internal ApplicationDbContext Context;
        internal DbSet<TEntity> DbSet;
        #endregion

        #region Public Constructor...
        /// <summary>
        /// Public Constructor,initializes privately declared local variables.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(ApplicationDbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }


        #endregion

        #region Public member methods...

        /// <summary>
        /// generic Get method for Entities
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = DbSet;
            return query.ToList();
        }

        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetByID(object id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetWithNoTrackingByID(Func<TEntity, bool> where)
        {

            return DbSet.AsNoTracking().Where(where).FirstOrDefault();
        }

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            if (entity.GetType().GetProperty("CreatedOn") != null)
            {
                entity.GetType().GetProperty("CreatedOn").SetValue(entity, DateTime.Now);
            }
            //set modified date to null
            //if (entity.GetType().GetProperty("ModifiedOn") != null)
            //{
            //    entity.GetType().GetProperty("ModifiedOn").SetValue(entity, null);
            //}
            DbSet.Add(entity);
        }

        public virtual void InsertList(ICollection<TEntity> entity)
        {
            if (entity.GetType().GetProperty("CreatedOn") != null)
            {
                entity.GetType().GetProperty("CreatedOn").SetValue(entity, DateTime.Now);
            }
            //set modified date to null
            if (entity.GetType().GetProperty("ModifiedOn") != null)
            {
                entity.GetType().GetProperty("ModifiedOn").SetValue(entity, null);
            }
            DbSet.AddRange(entity);
        }

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }


        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            //var claims = from c in ClaimsPrincipal.Current.Claims select new { c.Type, c.Value };
            //var UserId = claims.Where(t => t.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            if (entityToUpdate.GetType().GetProperty("ModifiedOn") != null)
            {
                entityToUpdate.GetType().GetProperty("ModifiedOn").SetValue(entityToUpdate, DateTime.Now);
            }

            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdatePrivateDoc(TEntity entityToUpdate)
        {
            //var claims = from c in ClaimsPrincipal.Current.Claims select new { c.Type, c.Value };
            //var UserId = claims.Where(t => t.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            //if (entityToUpdate.GetType().GetProperty("ModifiedOn") != null)
            //{
            //    entityToUpdate.GetType().GetProperty("ModifiedOn").SetValue(entityToUpdate, DateTime.Now);
            //}

            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Generic update list 
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void UpdateList(ICollection<TEntity> entityToUpdate)
        {
            if (entityToUpdate.GetType().GetProperty("ModifiedOn") != null)
            {
                entityToUpdate.GetType().GetProperty("ModifiedOn").SetValue(entityToUpdate, DateTime.Now);
            }

            DbSet.AttachRange(entityToUpdate);
            foreach (var et in entityToUpdate)
            {
                Context.Entry(et).State = EntityState.Modified;
            }

        }

        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="Id"></param>
        public virtual void UpdateDelete(int Id)
        {
            SetStatus(Id, 0);
        }

        public virtual void UpdateDeleteArray(int[] Id)
        {
            foreach (var Data in Id)
            {
                SetStatus(Data, 0);
            }
        }

        public virtual bool IsActive(int Id, object status)
        {
            return SetStatus(Id, status);
        }

        private bool SetStatus(int Id, object status)
        {
            TEntity entityToDelete = DbSet.Find(Id);

            if (entityToDelete.GetType().GetProperty("Status") != null)
            {
                //get the property information based on the type
                System.Reflection.PropertyInfo propertyInfo = entityToDelete.GetType().GetProperty("Status");

                //find the property type
                Type propertyType = propertyInfo.PropertyType;

                //Convert.ChangeType does not handle conversion to nullable types
                //if the property type is nullable, we need to get the underlying type of the property
                var targetType = IsNullableType(propertyType) ? Nullable.GetUnderlyingType(propertyType) : propertyType;

                entityToDelete.GetType().GetProperty("Status").SetValue(entityToDelete, Convert.ChangeType(status, targetType));
            }

            if (entityToDelete.GetType().GetProperty("ModifiedOn") != null)
            {
                entityToDelete.GetType().GetProperty("ModifiedOn").SetValue(entityToDelete, DateTime.Now);
            }
            try
            {
                DbSet.Attach(entityToDelete);
                Context.Entry(entityToDelete).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        /// <summary>
        /// check the object is nullable type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsNullableType(Type type)
        {
            return (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return DbSet.Where(where).ToList();
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return DbSet.Where(where).AsQueryable();
        }

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TEntity Get(Func<TEntity, Boolean> where)
        {
            return DbSet.Where(where).FirstOrDefault<TEntity>();
        }

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public void Delete(Func<TEntity, Boolean> where)
        {
            IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();
            foreach (TEntity obj in objects)
                DbSet.Remove(obj);
        }

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public void DeleteList(IEnumerable<TEntity> entity)
        {
            DbSet.RemoveRange(entity);
        }

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetWithInclude(
            System.Linq.Expressions.Expression<Func<TEntity,
                    bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        /// <summary>
        /// get the data without tracking
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetNoTrackWithInclude(
            System.Linq.Expressions.Expression<Func<TEntity,
            bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.AsNoTracking().Where(predicate);
        }

        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public bool Exists(object primaryKey)
        {
            return DbSet.Find(primaryKey) != null;
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public TEntity GetSingle(Func<TEntity, bool> predicate)
        {
            return DbSet.Single<TEntity>(predicate);
        }

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public TEntity GetFirst(Func<TEntity, bool> predicate)
        {
            return DbSet.First<TEntity>(predicate);
        }


        /// <summary>
        /// The Last record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the last record matching the specified criteria</returns>
        public TEntity GetLast(Func<TEntity, bool> predicate)
        {
            return DbSet.Last<TEntity>(predicate);
        }


        /// <summary>
        /// update a single property
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        /// <param name="collection">String Collection of modified property</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public virtual void UpdateProperty(TEntity entityToUpdate, List<string> collection)
        {
            try
            {

                DbSet.Attach(entityToUpdate);
                if (entityToUpdate.GetType().GetProperty("ModifiedOn") != null)
                {
                    entityToUpdate.GetType().GetProperty("ModifiedOn").SetValue(entityToUpdate, DateTime.Now);
                    Context.Entry(entityToUpdate).Property("ModifiedOn").IsModified = true;
                }
                // Context.Entry(entityToUpdate).Property("ModifiedBy").IsModified = true;

                foreach (var item in collection)
                {
                    var temp = entityToUpdate.GetType().GetProperty(item).Name;
                    Context.Entry(entityToUpdate).Property(temp).IsModified = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public virtual void UpdatePropertyWithoutModified(TEntity entityToUpdate, List<string> collection)
        {
            try
            {

                DbSet.Attach(entityToUpdate);
                foreach (var item in collection)
                {
                    var temp = entityToUpdate.GetType().GetProperty(item).Name;
                    Context.Entry(entityToUpdate).Property(temp).IsModified = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
