using Microsoft.EntityFrameworkCore;
using MVC.Model;
using MVC.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Repository.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : baseEntity
    {
        protected readonly SqlServerContext _context;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(SqlServerContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        // Method responsible for returning all,
        public List<T> FindAll()
        {
            return DbSet.AsNoTracking().ToList();
        }

        // Method responsible for returning one Model by ID
        public T FindByID(long id)
        {
            return DbSet.Find(id);
        }

        // Method responsible to crete one new Model
        public T Create(T model)
        {
            try
            {
                _context.Add(model);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        // Method responsible for updating one Model
        public T Update(T model)
        {
            var result = FindByID(model.Id);
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(model);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return model;
        }

        // Method responsible for deleting a Model from an ID
        public void Delete(long id)
        {
            var result = FindByID(id);
            if (result != null)
            {
                try
                {
                    DbSet.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
       
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
