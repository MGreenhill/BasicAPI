using BasicAPI.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BasicAPI.Services
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task PartialUpdate(int id, JsonPatchDocument patchDocument);
        Task Delete(int id);
        Task Save();

    }
}
