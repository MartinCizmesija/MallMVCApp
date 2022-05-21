using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Mall.Repositories;
using Mall.Models;
using System;

namespace Mall.Test
{
    [TestClass]
    public class ProductRepositoryTest
    {
        private MallDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<MallDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            var dbContext = new MallDbContext(options);
            return dbContext;
        }


        [TestMethod]
        public void TestAddSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var mall_repository = new HomeRepository(dbContext);
            var room_repository = new RoomRepository(dbContext);
            var store_repository = new StoreRepository(dbContext);
            var repository = new ProductRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            store_repository.Add(store);
            var product = new Product { StoreId = store.StoreId, Price = 10.0, ProductName = "ProductName", ProductDescription = "ProductDescription" };

            //Act
            var result = repository.Add(product);

            //Assert
            Assert.IsTrue(result.ProductId > 0);
            Assert.IsTrue(result.StoreId == store.StoreId);
            Assert.IsTrue(result.Price == 10.0);
            Assert.IsTrue(result.ProductName == "ProductName");
            Assert.IsTrue(result.ProductDescription == "ProductDescription");

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var mall_repository = new HomeRepository(dbContext);
            var room_repository = new RoomRepository(dbContext);
            var store_repository = new StoreRepository(dbContext);
            var repository = new ProductRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            store_repository.Add(store);
            var product = new Product { StoreId = store.StoreId, Price = 10.0, ProductName = "ProductName", ProductDescription = "ProductDescription" };
            var obj = repository.Add(product);

            //Act
            var result = repository.Get(obj.ProductId);

            //Assert
            Assert.IsTrue(result.ProductId > 0);
            Assert.IsTrue(result.StoreId == store.StoreId);
            Assert.IsTrue(result.Price == 10.0);
            Assert.IsTrue(result.ProductName == "ProductName");
            Assert.IsTrue(result.ProductDescription == "ProductDescription");

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetFail()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new ProductRepository(dbContext);

            //Act
            var result = repository.Get(0);

            //Assert
            Assert.IsNull(result);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestUpdateSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var mall_repository = new HomeRepository(dbContext);
            var room_repository = new RoomRepository(dbContext);
            var store_repository = new StoreRepository(dbContext);
            var repository = new ProductRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store1 = new Store { RoomId = room.RoomId, StoreName = "StoreName1", StoreDescription = "StoreDescription1", RentDebt = 500.0 };
            store_repository.Add(store1);
            var store2 = new Store { RoomId = room.RoomId, StoreName = "StoreName2", StoreDescription = "StoreDescription2", RentDebt = 600.0 };
            store_repository.Add(store2);
            var product = new Product { StoreId = store1.StoreId, Price = 10.0, ProductName = "ProductName", ProductDescription = "ProductDescription" };
            var obj = repository.Add(product);

            //Act
            obj.StoreId = store2.RoomId;
            obj.Price = 20.0;
            obj.ProductName = "NEWProductName";
            obj.ProductDescription = "NEWProductDescription";
            var result = repository.Update(obj);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(repository.Get(obj.ProductId).StoreId == store2.RoomId);
            Assert.IsTrue(repository.Get(obj.ProductId).Price == 20.0);
            Assert.IsTrue(repository.Get(obj.ProductId).ProductName == "NEWProductName");
            Assert.IsTrue(repository.Get(obj.ProductId).ProductName == "NEWProductName");
            Assert.IsTrue(repository.Get(obj.ProductId).ProductDescription == "NEWProductDescription");

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestDeleteSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var mall_repository = new HomeRepository(dbContext);
            var room_repository = new RoomRepository(dbContext);
            var store_repository = new StoreRepository(dbContext);
            var repository = new ProductRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            store_repository.Add(store);
            var product = new Product { StoreId = store.StoreId, Price = 10.0, ProductName = "ProductName", ProductDescription = "ProductDescription" };
            var obj = repository.Add(product);

            //Act
            repository.Delete(product);

            //Assert
            Assert.IsNull(repository.Get(obj.StoreId));

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetListSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var mall_repository = new HomeRepository(dbContext);
            var room_repository = new RoomRepository(dbContext);
            var store_repository = new StoreRepository(dbContext);
            var repository = new ProductRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            store_repository.Add(store);
            var product1 = new Product { StoreId = store.StoreId, Price = 10.0, ProductName = "ProductName1", ProductDescription = "ProductDescription1" };
            var product2 = new Product { StoreId = store.StoreId, Price = 15.0, ProductName = "ProductName2", ProductDescription = "ProductDescription2" };
            var product3 = new Product { StoreId = store.StoreId, Price = 20.0, ProductName = "ProductName3", ProductDescription = "ProductDescription3" };
            repository.Add(product1);
            repository.Add(product2);
            repository.Add(product3);

            //Act
            var result = repository.GetList();

            //Assert
            Assert.IsTrue(result.CountAsync().Result == 3);

            //Clean up
            dbContext.Dispose();
        }
    }
}
