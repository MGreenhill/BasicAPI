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
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Person entity)
        {
            await _context.Set<Person>().AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            Person entityToRemove = await _context.Set<Person>().FindAsync(id);
            if (entityToRemove != null) { 
                _context.Set<Person>().Remove(entityToRemove);
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Set<Person>().ToListAsync();
        }

        public async Task<Person> GetById(int id)
        {
            return await _context.Set<Person>().FindAsync(id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Person entity)
        {
            Person existingPerson = _context.Set<Person>().FirstOrDefault(p => p.Id == entity.Id);
            _context.Entry(existingPerson).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

        public async Task PartialUpdate(int id, JsonPatchDocument personDocument)
        {
            Person existingPerson = _context.Set<Person>().FirstOrDefault(p => p.Id == id);
            if (existingPerson != null)
            {
                personDocument.ApplyTo(existingPerson);
                await _context.SaveChangesAsync();
            }
        }
    }
}
