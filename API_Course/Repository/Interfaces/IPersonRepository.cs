using MVC.Model;
using System.Collections.Generic;

namespace MVC.Repository
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Person Disable(long Id);
        List<Person> FindByName(string firstName, string lastName);
    }
}
