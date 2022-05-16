using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sg.LiquidGroup.Products.Domain
{
    public class Cart:Document
    {

        public List<CartProduct> Products { get; set; }

        public Payment Payment { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public bool IsSubmitted { get; set; }


    }


    public class CartProduct
    {
        public int Quantity { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = "";

    
    }


    public class Payment {

        public decimal TotalPrice { get; set; }

        public string Currency { get; set; } = "";
    }


}
