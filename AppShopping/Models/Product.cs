using System.Text.Json.Serialization;

namespace AppShopping.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Make { get; set; }
        public int? Stock {  get; set; }

        
        [JsonIgnore]
        public ICollection<Shopping>? shoppings { get; set; }
        
    }
}
