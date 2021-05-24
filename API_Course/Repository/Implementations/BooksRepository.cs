using MVC.Model;
using MVC.Model.Context;

namespace MVC.Repository.Implementations
{
    public class BooksRepository : BaseRepository<Books>, IBooksRepository
    {
        public BooksRepository(SqlServerContext serverContext) : base(serverContext)
        {
        }
    }
}
