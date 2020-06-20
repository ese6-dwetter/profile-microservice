using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfileMicroservice.Entities
{
    public abstract class BaseEntity
    {
        [BsonId] public Guid Id { get; set; }
    }
}
