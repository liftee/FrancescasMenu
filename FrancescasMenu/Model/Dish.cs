using Realms;
using MongoDB.Bson;

namespace FrancescasMenu.Model
{
    public class Dish : RealmObject
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

        [MapTo("course")]
        public string Course { get; set; }

        [MapTo("description")]
        public string Description { get; set; }

        [MapTo("price")]
        public decimal Price { get; set; }

        public enum CourseName
        {
            Antipasti,
            Insalate,
            Pizza,
            Features,
            Paste,
            Secondi,
            Contorni
        }
    }
}
