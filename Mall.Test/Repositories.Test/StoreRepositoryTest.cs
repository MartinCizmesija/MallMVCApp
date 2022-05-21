using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Mall.Repositories;
using Mall.Models;
using System;

namespace Mall.Test
{
    [TestClass]
    public class StoreRepositoryTest
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
            var repository = new StoreRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };

            //Act
            var result = repository.Add(store);

            //Assert
            Assert.IsTrue(result.RoomId > 0);
            Assert.IsTrue(result.RoomId == room.RoomId);
            Assert.IsTrue(result.StoreName == "StoreName");
            Assert.IsTrue(result.StoreDescription == "StoreDescription");
            Assert.IsTrue(result.RentDebt == 500.0);

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
            var repository = new StoreRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            var obj = repository.Add(store);

            //Act
            var result = repository.Get(obj.StoreId);

            //Assert
            Assert.IsTrue(result.RoomId > 0);
            Assert.IsTrue(result.RoomId == room.RoomId);
            Assert.IsTrue(result.StoreName == "StoreName");
            Assert.IsTrue(result.StoreDescription == "StoreDescription");
            Assert.IsTrue(result.RentDebt == 500.0);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetFail()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new StoreRepository(dbContext);

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
            var repository = new StoreRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room1 = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room1);
            var room2 = new Room { MallId = mall.MallId, Rent = 200, IsAvailable = true };
            room_repository.Add(room2);
            var store = new Store { RoomId = room1.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            var obj = repository.Add(store);

            //Act
            obj.RoomId = room2.RoomId;
            obj.StoreName = "NEWStoreName";
            obj.StoreDescription = "NEWStoreDescription";
            obj.RentDebt = 1000.0;
            var result = repository.Update(obj);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(repository.Get(obj.StoreId).RoomId == room2.RoomId);
            Assert.IsTrue(repository.Get(obj.StoreId).StoreName == "NEWStoreName");
            Assert.IsTrue(repository.Get(obj.StoreId).StoreDescription == "NEWStoreDescription");
            Assert.IsTrue(repository.Get(obj.StoreId).RentDebt == 1000.0);

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
            var repository = new StoreRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            var obj = repository.Add(store);

            //Act
            repository.Delete(store);

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
            var repository = new StoreRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var store1 = new Store { RoomId = room.RoomId, StoreName = "StoreName1", StoreDescription = "StoreDescription1", RentDebt = 500.0 };
            var store2 = new Store { RoomId = room.RoomId, StoreName = "StoreName2", StoreDescription = "StoreDescription2", RentDebt = 600.0 };
            var store3 = new Store { RoomId = room.RoomId, StoreName = "StoreName3", StoreDescription = "StoreDescription3", RentDebt = 900.0 };
            repository.Add(store1);
            repository.Add(store2);
            repository.Add(store3);

            //Act
            var result = repository.GetList();

            //Assert
            Assert.IsTrue(result.CountAsync().Result == 3);

            //Clean up
            dbContext.Dispose();
        }
    }
}
