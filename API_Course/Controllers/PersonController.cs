﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Business;
using MVC.Data.VO;
using MVC.Hypermedia.Filter;
using System.Collections.Generic;

namespace MVC.Controllers
{

    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class PersonController : ControllerBase
    {

        private readonly ILogger<PersonController> _logger;

        // Declaration of the service used
        private IPersonService _personBusiness;

        // Injection of an instance of IPersonService
        // when creating an instance of PersonController
        public PersonController(ILogger<PersonController> logger, IPersonService personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        // Maps GET requests to /api/person
        // Get no parameters for FindAll -> Search All
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(List<PersonVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        // Maps GET requests to /api/person/{id}
        // receiving an ID as in the Request Path
        // Get with parameters for FindById -> Search by ID
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var person = _personBusiness.FindByID(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        // Maps POST requests to /api/person/
        // [FromBody] consumes the JSON object sent in the request body
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] PersonVO person)
        {
            if (person == null) return BadRequest();
            return Ok(_personBusiness.Create(person));
        }

        // Maps PUT requests to /api/person/
        // [FromBody] consumes the JSON object sent in the request body
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] PersonVO person)
        {
            if (person == null) return BadRequest();
            return Ok(_personBusiness.Update(person));
        }

        /// <summary>
        ///  Maps PUT requests to /api/person/
        ///   consumes the JSON object sent in the request body
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Patch(long id)
        {
            if (id == null) return BadRequest();
            return Ok(_personBusiness.Disable(id));
        }

        // Maps DELETE requests to /api/person/{id}
        // receiving an ID as in the Request Path
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }
    }
}
