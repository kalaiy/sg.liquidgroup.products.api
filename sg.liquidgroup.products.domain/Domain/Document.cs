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
    [Serializable]
    public abstract class Document 
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
      
        public string Id { get; set; }
    }

   
}
