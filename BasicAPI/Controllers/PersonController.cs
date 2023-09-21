using AutoMapper;
using BasicAPI.Entities;
using BasicAPI.Models;
using BasicAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IRepository<Entities.Person> _repository;
        private readonly IMapper _mapper;

        public PersonController(IRepository<Entities.Person> repository, IMapper mapper)
        {
            //Immediately instantiate and null check the repository and automapper
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns a list of all People Entities filtered through PersonDtos
        /// </summary>
        /// <returns>An ActionResult IEnumerable of PersonDto</returns>
        /// <response code= "200">Returns a list of PeopleDtos</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPeople()
        {   
            //Requests list of People from repository and parses People entities as PersonDtos
            var peopleEntities = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<PersonDto>>(peopleEntities));
        }

        /// <summary>
        /// Returns a Person by id
        /// </summary>
        /// <param name="id">The id of the Person to get</param>
        /// <returns>An ActionResult of PersonDto</returns>
        /// <response code="200">Returns the requested Person</response>
        /// <response code="404">Person id does not exist</response>
        /// <response code="400">Person request was not valid</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDto>> GetById(int id)
        {
            //Requests a existing person based its id
            var person = await _repository.GetById(id);
            return person == null ? NotFound() : Ok(_mapper.Map<PersonDto>(person));

        }

        /// <summary>
        /// Posts a new Person to the database
        /// </summary>
        /// <param name="person">New person entity to parse</param>
        /// <returns>An ActionResult of PersonDto</returns>
        /// <response code="200">Returns posted new Person</response>
        /// <response code="400">User request was not valid</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDto>> AddPerson([FromBody] PersonCreateDto person)
        {
            //Attempt to map incoming Dto to an entity and add it to the database.
            //Returns created person or bad request
            try
            {
                var addedPerson = _mapper.Map<Entities.Person>(person);
                await _repository.Add(addedPerson);
                await _repository.Save();

                var createdPerson = _mapper.Map<Models.PersonDto>(addedPerson);

                return CreatedAtRoute("GetById", new { id = createdPerson.Id }, createdPerson);
            }
            catch (Exception Ex) {

                return BadRequest(Ex);
            }
        }

        /// <summary>
        /// Deletes a Person from the database
        /// </summary>
        /// <param name="id">The id of the Person to delete from database</param>
        /// <returns>An ActionResult</returns>
        /// <response code="204">Person has been deleted from database</response>
        /// <response code="404">User request was not valid</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePerson(int id)
        {
            //Check if entity exists
            var personToDelete = await _repository.GetById(id);
            if (personToDelete == null) {
                return NotFound();
            }

            //Request repository to delete entity and then save
            await _repository.Delete(id);
            await _repository.Save();

            return NoContent();

        }

        /// <summary>
        /// Completely updates a Person in the database
        /// </summary>
        /// <param name="id">The id of the Person to update</param>
        /// <param name="person">New Person to update current Person</param>
        /// <returns>An ActionResult</returns>
        /// <response code="204">Person has been updated</response>
        /// <response code="400">User request was not valid</response>
        /// <response code="404">Person to replace was not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDto person)
        {
            //Check if entity to update exists
            var personEntity = await _repository.GetById(id);
            if (personEntity == null)
            {
                return NotFound();
            }

            //Attempt to map Dto as entity then have respository replace current entity.
            try
            {
                var updatedPerson = _mapper.Map<Entities.Person>(person);
                await _repository.Update(updatedPerson);

                return NoContent();
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex);
            }
        }

        /// <summary>
        /// Partially updates a Person in the database
        /// </summary>
        /// <param name="id">The id of the Person to update</param>
        /// <param name="patchDocument">A set of properties values to update the Person</param>
        /// <returns>An ActionResult</returns>
        /// <response code="204">Person has been patched</reponse>
        /// <response code="400">User input was invalid</response>
        /// <response code="404">Person does not exist</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PartialUpdatePerson(int id, [FromBody] JsonPatchDocument<PersonUpdateDto> patchDocument)
        {
            //Check if entity exists
            var personEntity = await _repository.GetById(id);
            if (personEntity == null)
            {
                return NotFound();
            }

            //Attempt to map JsonPatchDocument<PersonUpdateDto> to JsonPatchDocument
            //Then have respository attempt to apply patch.
            try
            {
                var convertedPatch = _mapper.Map<JsonPatchDocument>(patchDocument);
                await _repository.PartialUpdate(id, convertedPatch);

                return NoContent();
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex);
            }
        }
    }
}
