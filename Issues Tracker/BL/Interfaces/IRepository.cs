using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Issues_Tracker.BL
{
    public interface IRepository<TEnitity> where TEnitity : class
    {
        TEnitity Get(int id);

        IEnumerable<TEnitity> GetAll();

        IEnumerable<TEnitity> Find(Expression<Func<TEnitity, bool>> predicate);

        IEnumerable<string> Select(Expression<Func<TEnitity, string>> selector);

        TEnitity FirstOrDefault(Expression<Func<TEnitity, bool>> predicate);

        void Create(TEnitity entity);

        void Remove(TEnitity entity);
    }
}
