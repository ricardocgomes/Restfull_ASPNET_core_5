using MVC.Data.Converter.Implementations;
using MVC.Data.VO;
using MVC.Model;
using MVC.Repository;
using System.Collections.Generic;

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
            var booksEntity = _personConverter.Parse(person);
            var bookCreated = _personRepository.Create(booksEntity);
            return _personConverter.Parse(bookCreated);
        }

        // Method responsible for updating one person
        public PersonVO Update(PersonVO person)
        {
            var booksEntity = _personConverter.Parse(person);
            var bookUpdated = _personRepository.Update(booksEntity);
            return _personConverter.Parse(bookUpdated);
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
    }
}
