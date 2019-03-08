using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DoTheDishesWebservice.DataAccess.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        T Get(int id);
        IQueryable<T> Query(Expression<Func<T, bool>> filter);
        List<T> GetAll();
        void Save(T model);
        void Delete(int id);
        void Update(T model);
        bool CheckIfExists(int id);
    }
}
