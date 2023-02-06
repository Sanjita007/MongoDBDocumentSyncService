using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessLogicLayer.Models
{
    public class SignatureHist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public int Id { get; set; }
        public string ImageId { get; set; }
        public byte[] DocFile { get; set; }
    }

}
