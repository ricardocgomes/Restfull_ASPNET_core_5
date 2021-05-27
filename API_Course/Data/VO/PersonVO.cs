using MVC.Hypermedia;
using MVC.Hypermedia.Abstract;
using System.Collections.Generic;

namespace MVC.Data.VO
{
    public class PersonVO : ISuportsHyperMedia
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool Enabled { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
