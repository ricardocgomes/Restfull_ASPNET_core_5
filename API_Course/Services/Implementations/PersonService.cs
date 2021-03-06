using Mvc.Hypermedia.Utils;
using MVC.Data.Converter.Implementations;
using MVC.Data.VO;
using MVC.Model;
using MVC.Repository;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Business.Implementations
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly PersonConverter _personConverter;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
            _personConverter = new PersonConverter();
        }

        // Method responsible for returning all people,
        public List<PersonVO> FindAll()
        {
            return _personConverter.Parse(_personRepository.FindAll());
        }

        // Method responsible for returning one person by ID
        public PersonVO FindByID(long id)
        {
            return _personConverter.Parse(_personRepository.FindByID(id));
        }

        // Method responsible to crete one new person
        public PersonVO Create(PersonVO person)
        {
            var personEntity = _personConverter.Parse(person);
            var personCreated = _personRepository.Create(personEntity);
            return _personConverter.Parse(personCreated);
        }

        // Method responsible for updating one person
        public PersonVO Update(PersonVO person)
        {
            var personEntity = _personConverter.Parse(person);
            var personUpdated = _personRepository.Update(personEntity);
            return _personConverter.Parse(personUpdated);
        }

        // Method responsible for deleting a person from an ID
        public void Delete(long id)
        {
            _personRepository.Delete(id);
        }

        public void Dispose()
        {
            _personRepository?.Dispose();
        }

        public PersonVO Disable(long Id)
        {
            var personEntity = _personRepository.Disable(Id);
            return _personConverter.Parse(personEntity);
        }

        public List<PersonVO> FindByName(string firstName, string lastName)
        {
            return _personConverter.Parse(_personRepository.FindByName(firstName, lastName));
        }

        public PagedSearchVO<PersonVO> PagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc")) ? "asc" : "desc";
            int ExcludeRecords = (pageSize * page) - pageSize;

            var persons = _personRepository.FindAll();

            if (name != null)
            {
                persons = persons.Where(x => x.FirstName.Contains(name) || x.LastName.Contains(name)).ToList();
            }
            else if (sort.Equals("desc"))
            {
                persons = persons.OrderByDescending(x => x.Id).ToList();
            }

            var totalResults = persons.Count;
            persons = persons.Skip(ExcludeRecords).Take(pageSize).ToList();

            return new PagedSearchVO<PersonVO>
            {
                CurrentPage = page,
                List = _personConverter.Parse(persons),
                PageSize = pageSize,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }
    }
}
