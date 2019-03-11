using DoTheDishesWebservice.DataAccess.Configurations;
using DoTheDishesWebservice.DataAccess.Models;
using DoTheDishesWebservice.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DoTheDishesWebservice.DataAccess.Repositories
{
    public class HomeRepoitory : IHomeRepository
    {
        private readonly DishesContext Context;

        public HomeRepoitory(DishesContext context)
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

        public IEnumerable<Home> GetAll()
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

        public bool CheckIfExists(int id)
        {
            return Context.Homes.Any(o => o.HomeId == id);
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
