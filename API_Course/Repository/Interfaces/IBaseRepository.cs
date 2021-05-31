using MVC.Model;
using System;
using System.Collections.Generic;

namespace MVC.Repository
{
    public interface IBaseRepository<T> : IDisposable where T : class
    {
        T Create(T T);
        T FindByID(long id);
        List<T> FindAll();
        T Update(T T);
        void Delete(long id);
    }
}
