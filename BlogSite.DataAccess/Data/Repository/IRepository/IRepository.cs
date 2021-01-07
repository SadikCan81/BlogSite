using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BlogSite.DataAccess.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
                              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                              string includeProperties = null);

        void Add(T entity);

        void Remove(T entity);

        void Remove(int id);

        void RemoveRange(IEnumerable<T> entityList);
    }
}
