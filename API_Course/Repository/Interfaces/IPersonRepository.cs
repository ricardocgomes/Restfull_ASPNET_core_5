using MVC.Model;

namespace MVC.Repository
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Person Disable(long Id);
    }
}
