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
    }
}
