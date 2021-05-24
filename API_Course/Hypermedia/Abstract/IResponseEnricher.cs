using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace MVC.Hypermedia.Abstract
{
    public interface IResponseEnricher
    {
        bool CanEnrich(ResultExecutingContext resultExecutedContext);
        Task Enrich(ResultExecutingContext resultExecutedContext);
    }
}
