using MVC.Model;
using MVC.Model.Context;
using System;

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
    }
}
