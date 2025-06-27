using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Models.Cover
{
    public class CoverRequest
    {
        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("claimType")]
        public CoverType Type { get; set; }
    }
}
