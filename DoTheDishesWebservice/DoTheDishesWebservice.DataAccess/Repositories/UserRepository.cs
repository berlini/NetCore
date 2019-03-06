using DoTheDishesWebservice.DataAccess.Configurations;
using DoTheDishesWebservice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DoTheDishesWebservice.DataAccess.Repositories
{
    public class UserRepository : IRepository<User>, IDisposable
    {
        private readonly DishesContext Context;

        public UserRepository(DishesContext context)
        {
            Context = context;
        }

        public void Delete(int id)
        {
            User user = Context.Users.Where(o => o.UserId == id).FirstOrDefault();
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        public User Get(int id)
        {
            return Context.Users.Where(o => o.UserId == id).FirstOrDefault();
        }

        public List<User> GetAll()
        {
            return Context.Users.ToList();
        }

        public IQueryable<User> Query(Expression<Func<User, bool>> filter)
        {
            return Context.Users.Where(filter);
        }

        public void Save(User model)
        {
            Context.Users.Add(model);
            Context.SaveChanges();
        }

        public void Update(User model)
        {
            User user = Context.Users.Where(o => o.UserId == model.UserId).FirstOrDefault();

            user.Login = model.Login;
            user.Password = model.Password;
            user.Nickname = model.Nickname;
            user.Home = model.Home;

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
