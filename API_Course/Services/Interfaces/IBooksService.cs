using Mvc.Hypermedia.Utils;
using MVC.Data.VO;
using System;
using System.Collections.Generic;

namespace MVC.Services.Interfaces
{
    public interface IBooksService : IDisposable
    {
        BooksVO Create(BooksVO Books);
        BooksVO FindByID(long id);
        List<BooksVO> FindAll();
        BooksVO Update(BooksVO Books);
        void Delete(long id);
        PagedSearchVO<BooksVO> PagedSearch(string name, string sortDirection, int pageSize, int page);
    }
}
