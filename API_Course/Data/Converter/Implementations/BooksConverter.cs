using MVC.Data.Converter.Contract;
using MVC.Data.VO;
using MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Data.Converter.Implementations
{
    public class BooksConverter : IParser<BooksVO, Books>, IParser<Books, BooksVO>
    {
        //BooksVO to Books

        public Books Parse(BooksVO origin)
        {
            if (origin != null)
            {
                return new Books
                {
                    Id = origin.Id,
                    Author = origin.Author,
                    LaunchDate = origin.LaunchDate,
                    Price = origin.Price,
                    Title = origin.Title
                };
            }

            return null;
        }

        public List<Books> Parse(List<BooksVO> orgins)
        {
            if (orgins.Count == 0) return null;
            return orgins.Select(item => Parse(item)).ToList();
        }


        //Books to BooksVO
        public BooksVO Parse(Books origin)
        {
            if (origin != null)
            {
                return new BooksVO
                {
                    Id = origin.Id,
                    Author = origin.Author,
                    LaunchDate = origin.LaunchDate,
                    Price = origin.Price,
                    Title = origin.Title
                };
            }

            return null;
        }

        public List<BooksVO> Parse(List<Books> orgins)
        {
            if (orgins.Count == 0) return null;
            return orgins.Select(item => Parse(item)).ToList();
        }
    }
}
