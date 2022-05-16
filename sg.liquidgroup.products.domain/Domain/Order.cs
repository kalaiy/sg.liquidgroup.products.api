using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sg.LiquidGroup.Products.Domain
{
    public class Order: Document
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string CartId { get; set; }
        //public decimal TotalPrice { get; set; } // duplicated for the given user story
        public OrderStatus Status { get; set;}

    }

    public enum OrderStatus:int
    {
        OPEN,
        COMPLETED
    }

}
