using Mvc.Hypermedia.Utils;
using MVC.Data.Converter.Implementations;
using MVC.Data.VO;
using MVC.Repository.Implementations;
using MVC.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Services.Implementations
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly BooksConverter _booksConverter;

        public BooksService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
            _booksConverter = new BooksConverter();
        }

        public BooksVO Create(BooksVO Books)
        {
            var booksEntity = _booksConverter.Parse(Books);
            var bookCreated = _booksRepository.Create(booksEntity);
            return _booksConverter.Parse(bookCreated);
        }

        public void Delete(long id)
        {
            _booksRepository.Delete(id);
        }

        public void Dispose()
        {
            _booksRepository?.Dispose();
        }

        public List<BooksVO> FindAll()
        {
            return _booksConverter.Parse(_booksRepository.FindAll());
        }

        public BooksVO FindByID(long id)
        {
            return _booksConverter.Parse(_booksRepository.FindByID(id));
        }

        public PagedSearchVO<BooksVO> PagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc")) ? "asc" : "desc";
            int ExcludeRecords = (pageSize * page) - pageSize;

            var books = _booksRepository.FindAll();

            if (name != null)
            {
                books = books.Where(x => x.Title.Contains(name) || x.Author.Contains(name)).ToList();
            }
            else if (sort.Equals("desc"))
            {
                books = books.OrderByDescending(x => x.Id).ToList();
            }

            var totalResults = books.Count;
            books = books.Skip(ExcludeRecords).Take(pageSize).ToList();

            return new PagedSearchVO<BooksVO>
            {
                CurrentPage = page,
                List = _booksConverter.Parse(books),
                PageSize = pageSize,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public BooksVO Update(BooksVO Books)
        {
            var booksEntity = _booksConverter.Parse(Books);
            var bookUpdated = _booksRepository.Update(booksEntity);
            return _booksConverter.Parse(bookUpdated);
        }
    }
}
