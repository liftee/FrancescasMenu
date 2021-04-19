using Realms;
using MongoDB.Bson;
using System.Collections.Generic;

namespace FrancescasMenu.Model
{
    public class Menu : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("_partition")]
        [Required]
        public string Partition { get; set; }

        [MapTo("meal")]
        [Required]
        public string Meal { get; set; }

        public IList<Dish> Dishes { get; }

        public IList<Wine> Wines { get; }

        [MapTo("wroteOn")]
        public System.DateTimeOffset ServedOn { get; set; }
    }
}
