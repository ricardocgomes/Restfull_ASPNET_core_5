using MVC.Hypermedia;
using MVC.Hypermedia.Abstract;
using System;
using System.Collections.Generic;

namespace MVC.Data.VO
{
    public class BooksVO : ISuportsHyperMedia
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public DateTime LaunchDate { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
