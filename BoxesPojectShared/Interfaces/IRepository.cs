using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesPojectShared.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get();
        T GetById(int id);
        T Create(T entityToCreate);
        void Delete(int id);
        T Update(T entityToUpdate);
    }
}
