using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDocumentSyncService.Models
{
    public class DocStore
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string DocGuid { get; set; }
        public byte[] DocFile { get; set; }
    }
}
