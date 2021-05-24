using System.Collections.Generic;

namespace MVC.Hypermedia.Abstract
{
    public interface ISuportsHyperMedia
    {
        List<HyperMediaLink> Links { get; set; }
    }
}
