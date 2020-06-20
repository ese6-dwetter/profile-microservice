using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProfileMicroservice.Entities;

namespace ProfileMicroservice.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        /// <summary>
        ///     Creates a new entity.
        /// </summary>
        /// <param name="entity">Entity to be saved</param>
        /// <returns>Created entity</returns>
        Task<T> CreateAsync(T entity);

        /// <summary>
        ///     Gets a list of all the entities.
        /// </summary>
        /// <returns>List of all the entities</returns>
        Task<IEnumerable<T>> ReadAsync();

        /// <summary>
        ///     Gets a single entity by their Guid.
        /// </summary>
        /// <param name="id">The guid to search for</param>
        /// <returns>Entity with the corresponding guid</returns>
        Task<T> ReadByIdAsync(Guid id);

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <param name="entity">Entity with updated fields</param>
        /// <returns>Updated user</returns>
        Task<T> UpdateAsync(Guid id, T entity);

        /// <summary>
        ///     Removes an entity by their Guid.
        /// </summary>
        /// <param name="id">Guid of the entity to remove</param>
        /// <returns>Async task to await</returns>
        Task DeleteByIdAsync(Guid id);

        /// <summary>
        ///     Removes an entity.
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        /// <returns>Async task to await</returns>
        Task DeleteByUserAsync(T entity);
    }
}
