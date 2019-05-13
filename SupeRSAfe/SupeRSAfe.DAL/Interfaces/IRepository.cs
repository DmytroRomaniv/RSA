using System;
using System.Collections.Generic;
using System.Text;

namespace SupeRSAfe.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> All { get; }
        IEnumerable<T> Find(Func<T, bool> condition);
        void Create(T item);

        void Update(T item);

        void Delete(T item);
        
    }
}
