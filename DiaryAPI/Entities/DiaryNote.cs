using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DiaryAPI.Entities
{
    public class DiaryNote
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("PersonName")]
        public string PersonName { get; set; }

        [BsonElement("Note")]
        public string Note { get; set; }

    }
}
