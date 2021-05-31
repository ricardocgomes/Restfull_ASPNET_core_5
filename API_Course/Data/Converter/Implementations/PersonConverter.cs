using MVC.Data.Converter.Contract;
using MVC.Data.VO;
using MVC.Model;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Data.Converter.Implementations
{
    public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
    {
        //PersonVO to Person

        public Person Parse(PersonVO origin)
        {
            if(origin != null)
            {
                return new Person
                {
                    Id = origin.Id,
                    FirstName = origin.FirstName,
                    LastName = origin.LastName,
                    Address = origin.Address,
                    Gender = origin.Gender,
                    Enabled = origin.Enabled
                };
            }

            return null;
        }

        public List<Person> Parse(List<PersonVO> orgins)
        {
            if (orgins.Count == 0) return null;
            return orgins.Select(item => Parse(item)).ToList();
        }

        //Person to PersonVO

        public PersonVO Parse(Person origin)
        {
            if (origin != null)
            {
                return new PersonVO
                {
                    Id = origin.Id,
                    FirstName = origin.FirstName,
                    LastName = origin.LastName,
                    Address = origin.Address,
                    Gender = origin.Gender,
                    Enabled = origin.Enabled
                };
            }

            return null;
        }

        public List<PersonVO> Parse(List<Person> orgins)
        {
            if (orgins.Count == 0) return null;
            return orgins.Select(item => Parse(item)).ToList();
        }
    }
}