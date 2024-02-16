using System.Text.Json.Serialization;

namespace AppShopping.Models
{
    public class Shopping
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int ProductId { get; set; }
        public required string Document {  get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public DateTime Created { get; set; }


        //relations
        [JsonIgnore]
        public Provider? Provider { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
