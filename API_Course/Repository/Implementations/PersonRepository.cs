using MVC.Model;
using MVC.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Repository.Implementations
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(SqlServerContext context) : base(context)
        {

        }

        public Person Disable(long Id)
        {
            var user = FindByID(Id);

            if (user == null) return null;

            user.Enabled = false;

            try
            {
                Update(user);
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Person> FindByName(string firstName, string lastName)
        {
            return DbSet.Where(x => x.FirstName.Contains(firstName) || x.LastName.Contains(lastName)).ToList();
        }
    }
}
