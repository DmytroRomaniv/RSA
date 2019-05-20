using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SupeRSAfe.DAL.Context;
using SupeRSAfe.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SupeRSAfe.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MailDbContext _dbContext;

        public Repository( MailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<T> All
        {
            get
            {
                var collection = _dbContext.Set<T>();
                return collection;
            }
        }

        public async void Create(T item)
        {
            All.Append(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public IEnumerable<T> Find(Func<T, bool> condition)
        {
            var items = _dbContext.Set<T>().Where(condition);
            return items;
        }

        public void Update(T item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}
