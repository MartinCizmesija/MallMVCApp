using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Mall.Repositories;
using Mall.Models;
using System;

namespace Mall.Test
{
    [TestClass]
    public class RoomRepositoryTest
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
            var repository = new RoomRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };

            //Act
            var result = repository.Add(room);

            //Assert
            Assert.IsTrue(result.RoomId > 0);
            Assert.IsTrue(result.MallId == mall.MallId);
            Assert.IsTrue(result.Rent == 100.0);
            Assert.IsTrue(result.IsAvailable == true);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var mall_repository = new HomeRepository(dbContext);
            var repository = new RoomRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            var obj = repository.Add(room);

            //Act
            var result = repository.Get(obj.RoomId);

            //Assert
            Assert.IsTrue(result.RoomId > 0);
            Assert.IsTrue(result.MallId == mall.MallId);
            Assert.IsTrue(result.Rent == 100.0);
            Assert.IsTrue(result.IsAvailable == true);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetFail()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new RoomRepository(dbContext);

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
            var repository = new RoomRepository(dbContext);
            var mall1 = new MallCenter { MallId = 1, MallName = "MallName1", MallDescription = "MallDescription1" };
            mall_repository.Add(mall1);
            var mall2 = new MallCenter { MallId = 2, MallName = "MallName2", MallDescription = "MallDescription2" };
            mall_repository.Add(mall2);
            var room = new Room { MallId = mall1.MallId, Rent = 100.0, IsAvailable = true };
            var obj = repository.Add(room);

            //Act
            obj.MallId = mall2.MallId;
            obj.Rent = 200.0;
            obj.IsAvailable = false;
            var result = repository.Update(obj);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(repository.Get(obj.RoomId).MallId == mall2.MallId);
            Assert.IsTrue(repository.Get(obj.RoomId).Rent == 200.0);
            Assert.IsTrue(repository.Get(obj.RoomId).IsAvailable == false);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestDeleteSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var mall_repository = new HomeRepository(dbContext);
            var repository = new RoomRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            var obj = repository.Add(room);

            //Act
            repository.Delete(room);

            //Assert
            Assert.IsNull(repository.Get(obj.RoomId));

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetListSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var mall_repository = new HomeRepository(dbContext);
            var repository = new RoomRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room1 = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            var room2 = new Room { MallId = mall.MallId, Rent = 150.0, IsAvailable = true };
            var room3 = new Room { MallId = mall.MallId, Rent = 200.0, IsAvailable = true };
            repository.Add(room1);
            repository.Add(room2);
            repository.Add(room3);

            //Act
            var result = repository.GetList();

            //Assert
            Assert.IsTrue(result.CountAsync().Result == 3);

            //Clean up
            dbContext.Dispose();
        }
    }
}
