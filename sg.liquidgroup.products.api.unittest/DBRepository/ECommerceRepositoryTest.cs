using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Moq;
using sg.liquidgroup.products.api.unittest.Helper;
using Sg.LiquidGroup.Products.Dataaccess;
using Sg.LiquidGroup.Products.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sg.liquidgroup.products.api.unittest.DBRepository
{
    public class ECommerceRepositoryTest
    {
        List<Product> products = new List<Product>();
        private Mock<IMongoQueryable<Product>> productQuerableMock;

        public ECommerceRepositoryTest()
        {
            var queryableProduct = products.AsQueryable();
            productQuerableMock= new Mock<IMongoQueryable<Product>>();
            productQuerableMock.As<IQueryable<Product>>().Setup(x => x.Provider).Returns(queryableProduct.Provider);
            productQuerableMock.As<IQueryable<Product>>().Setup(x => x.Expression).Returns(queryableProduct.Expression);
            productQuerableMock.As<IQueryable<Product>>().Setup(x => x.ElementType).Returns(queryableProduct.ElementType);
            productQuerableMock.As<IQueryable<Product>>().Setup(x => x.GetEnumerator()).Returns(() => queryableProduct.GetEnumerator());
        }

        public IMongoQueryable<Product> Get()
        {
            return productQuerableMock.Object;
        }


        [Fact]
        public void ECommerceRepositoryTest_GetProducts()
        {

            var logger = new Mock<ILogger<ECommerceRepository>>();


            //var collectionMock1 = new MongoQueryable<Product>();
            //collectionMock1.MockData= new List<Product>() { new Product() { Id="sd" } };
            var collectionMock = new Mock<IMongoCollection<Product>>();
            var data = new List<Product>() { new Product() { Id="sd" } }.AsQueryable();

            var mockSet = new Mock<IMongoCollection<Product>>();
            mockSet.As<IMongoQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IMongoQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IMongoQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IMongoQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var dbMock = new Mock<IMongoDatabase>();
            dbMock.Setup(_ => _.GetCollection<Product>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(mockSet.Object);
          

            
            var repo = new ECommerceRepository(logger.Object, dbMock.Object);
           //Mock IMongoQueryable using a class


           //mockQueryable.MockData = expected;
           var result= repo.GetProducts("","","").GetAwaiter().GetResult();
            //return the mocked data
          
           // var result = repo.GetProducts("", "", "").GetAwaiter().GetResult();
            Assert.NotNull(result);
        }
    }
}
