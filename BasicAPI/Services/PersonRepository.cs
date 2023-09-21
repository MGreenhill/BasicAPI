using BasicAPI.DbContexts;
using BasicAPI.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BasicAPI.Services
{
    public class PersonRepository<Person> : IRepository<Person> where Person : class, IEntity
    {
        private readonly DbContext _context;
        public PersonRepository(PeopleContext context)
        {
            //Establish database upon creation
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Person entity)
        {
            //Add Person entity to database
            await _context.Set<Person>().AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            //Find entity in database and then remove it
            Person entityToRemove = await _context.Set<Person>().FindAsync(id);
            if (entityToRemove != null) { 
                _context.Set<Person>().Remove(entityToRemove);
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            //Returns IEnumerable of Person entities in database
            return await _context.Set<Person>().ToListAsync();
        }

        public async Task<Person> GetById(int id)
        {
            //Returns a Person entity based on its id.
            return await _context.Set<Person>().FindAsync(id);
        }

        public async Task Save()
        {
            //Save database changes
            await _context.SaveChangesAsync();
        }

        public async Task Update(Person entity)
        {
            //Find and replace all values of existing entity to incoming entity's then save.
            Person existingPerson = _context.Set<Person>().FirstOrDefault(p => p.Id == entity.Id);
            _context.Entry(existingPerson).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

        public async Task PartialUpdate(int id, JsonPatchDocument personDocument)
        {
            //Find existing Person entity and apply changed values from patch document then save.
            Person existingPerson = _context.Set<Person>().FirstOrDefault(p => p.Id == id);
            if (existingPerson != null)
            {
                personDocument.ApplyTo(existingPerson);
                await _context.SaveChangesAsync();
            }
        }
    }
}
