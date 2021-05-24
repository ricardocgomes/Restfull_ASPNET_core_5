using MVC.Model;
using MVC.Model.Context;

namespace MVC.Repository.Implementations
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(SqlServerContext context) : base(context)
        {

        }
    }
}
