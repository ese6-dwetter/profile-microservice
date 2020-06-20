using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProfileMicroservice.Entities;
using ProfileMicroservice.Repositories;
using ProfileMicroservice.Settings;

namespace ProfileMicroservice.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> Collection;

        protected BaseRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            Collection = database.GetCollection<T>(databaseSettings.CollectionName);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);

            return entity;
        }

        public async Task<IEnumerable<T>> ReadAsync()
        {
            return await Collection.Find(entity => true).ToListAsync();
        }

        public async Task<T> ReadByIdAsync(Guid id)
        {
            return await Collection.Find(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> UpdateAsync(Guid id, T entityIn)
        {
            await Collection.ReplaceOneAsync(entity => entity.Id == id, entityIn);

            return await Collection.Find(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await Collection.DeleteOneAsync(entity => entity.Id == id);
        }

        public async Task DeleteByUserAsync(T entityIn)
        {
            await Collection.DeleteOneAsync(entity => entity.Id == entityIn.Id);
        }
    }
}
