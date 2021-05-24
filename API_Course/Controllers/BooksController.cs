using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Data.VO;
using MVC.Hypermedia.Filter;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksService _booksService;

        public BooksController(ILogger<BooksController> logger, IBooksService booksService)
        {
            _logger = logger;
            _booksService = booksService;
        }

        [HttpGet]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_booksService.FindAll());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var person = _booksService.FindByID(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpPost]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] BooksVO books)
        {
            if (books == null) return BadRequest();
            return Ok(_booksService.Create(books));
        }

        [HttpPut]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] BooksVO books)
        {
            if (books == null) return BadRequest();
            return Ok(_booksService.Update(books));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _booksService.Delete(id);
            return NoContent();
        }
    }
}
