using MVC.Hypermedia.Abstract;
using System.Collections.Generic;

namespace MVC.Hypermedia.Filter
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
