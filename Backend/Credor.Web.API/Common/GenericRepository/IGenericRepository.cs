using System;
using System.Collections.Generic;
using System.Linq;

namespace Credor.Web.API.Common.GenericRepository
{
    public interface IGenericRepository<T> //where T :class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Get();
        T GetByID(object id);
        T GetWithNoTrackingByID(Func<T, bool> where);
        void Insert(T entity);
        void InsertList(ICollection<T> entity);
        void Delete(object id);
        void Delete(T entity);
        void Delete(Func<T, Boolean> where);
        void DeleteList(IEnumerable<T> entity);
        void Update(T entity);
        void UpdateList(ICollection<T> entity);
        void UpdateDelete(int Id);
        IEnumerable<T> GetMany(Func<T, bool> where);
        IQueryable<T> GetManyQueryable(Func<T, bool> where);
        T Get(Func<T, Boolean> where);

    }
}
