using DoTheDishesWebservice.DataAccess.Configurations;
using DoTheDishesWebservice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DoTheDishesWebservice.DataAccess.Repositories
{
    public class HomeRepository : IRepository<Home>, IDisposable
    {
        private readonly DishesContext Context;

        public HomeRepository(DishesContext context)
        {
            Context = context;
        }

        public void Delete(int id)
        {
            Home home = Context.Homes.Where(o => o.HomeId == id).FirstOrDefault();
            Context.Homes.Remove(home);
            Context.SaveChanges();
        }

        public Home Get(int id)
        {
            return Context.Homes.Where(o => o.HomeId == id).FirstOrDefault();
        }

        public List<Home> GetAll()
        {
            return Context.Homes.ToList();
        }

        public IQueryable<Home> Query(Expression<Func<Home, bool>> filter)
        {
            return Context.Homes.Where(filter);
        }

        public void Save(Home model)
        {
            Context.Homes.Add(model);
            Context.SaveChanges();
        }

        public void Update(Home model)
        {
            Home home = Context.Homes.Where(o => o.HomeId == model.HomeId).FirstOrDefault();

            home.Name = model.Name;
            home.Users = model.Users;

            Context.SaveChanges();
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}
