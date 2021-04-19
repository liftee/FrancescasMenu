using Realms;
using MongoDB.Bson;

namespace FrancescasMenu.Model
{
    public class Wine : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        [MapTo("name")]
        [Required]
        public string Name { get; set; }

        [MapTo("year")]
        public System.DateTimeOffset Year { get; set; }

        [MapTo("price")]
        public decimal Price { get; set; }
    }
}
