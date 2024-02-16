using System.Text.Json.Serialization;

namespace AppShopping.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Addres { get; set; }

        [JsonIgnore]
        public ICollection<Shopping>? shoppings { get; set; }
    }
}
