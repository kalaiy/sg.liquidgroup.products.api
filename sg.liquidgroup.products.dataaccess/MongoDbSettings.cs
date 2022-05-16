﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sg.LiquidGroup.Products.Dataaccess
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }

    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; } = "";
        public string ConnectionString { get; set; } = "";
    }

}
