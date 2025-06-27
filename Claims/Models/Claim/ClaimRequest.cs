using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Models.Claims
{
    public class ClaimRequest
    {
        [BsonElement("coverId")]
        public string? CoverId { get; set; }

        [BsonElement("created")]
        public DateTime Created { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("claimType")]
        public ClaimType Type { get; set; }

        [BsonElement("damageCost")]
        public decimal DamageCost { get; set; }
    }
}
