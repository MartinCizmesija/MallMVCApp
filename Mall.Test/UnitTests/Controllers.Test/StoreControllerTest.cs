using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Mall.Repositories;
using Mall.Controllers;
using Mall.Factories;
using Mall.Models;
using System;
using Moq;

namespace Mall.Test
{
    [TestClass]
    public class StoreControllerTest
    {
        private MallDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<MallDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            var dbContext = new MallDbContext(options);
            return dbContext;
        }

        public static IOptionsSnapshot<T> CreateIOptionSnapshotMock<T>(T value) where T : class, new()
        {
            var mock = new Mock<IOptionsSnapshot<T>>();
            mock.Setup(m => m.Value).Returns(value);
            return mock.Object;
        }

        [TestMethod]
        public void TestIndex()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var factory = new ViewModelFactory();
            var repository = new StoreRepository(dbContext);
            var appSettings = new AppSettings() { ConnectionString = "..." };
            var options = CreateIOptionSnapshotMock(appSettings);
            var mall_repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room_repository = new RoomRepository(dbContext);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var product_repository = new ProductRepository(dbContext);
            var controller = new StoresController(options, repository, product_repository, room_repository, factory);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            repository.Add(store);

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestCreate_RedirectsToIndexAction()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var factory = new ViewModelFactory();
            var repository = new StoreRepository(dbContext);
            var appSettings = new AppSettings() { ConnectionString = "..." };
            var options = CreateIOptionSnapshotMock(appSettings);
            var mall_repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room_repository = new RoomRepository(dbContext);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var product_repository = new ProductRepository(dbContext);
            var controller = new StoresController(options, repository, product_repository, room_repository, factory);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };

            // Act
            var result = controller.Create(store) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestEdit_Returns404()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var factory = new ViewModelFactory();
            var repository = new StoreRepository(dbContext);
            var appSettings = new AppSettings() { ConnectionString = "..." };
            var options = CreateIOptionSnapshotMock(appSettings);
            var mall_repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room_repository = new RoomRepository(dbContext);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var product_repository = new ProductRepository(dbContext);
            var controller = new StoresController(options, repository, product_repository, room_repository, factory);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            repository.Add(store);

            //Act
            Store null_store = null;
            var result = controller.Edit(null_store) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestDelete_RedirectsToIndexAction()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var factory = new ViewModelFactory();
            var repository = new StoreRepository(dbContext);
            var appSettings = new AppSettings() { ConnectionString = "..." };
            var options = CreateIOptionSnapshotMock(appSettings);
            var mall_repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room_repository = new RoomRepository(dbContext);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            room_repository.Add(room);
            var product_repository = new ProductRepository(dbContext);
            var controller = new StoresController(options, repository, product_repository, room_repository, factory);
            var store = new Store { RoomId = room.RoomId, StoreName = "StoreName", StoreDescription = "StoreDescription", RentDebt = 500.0 };
            repository.Add(store);

            // Act
            var result = controller.DeleteConfirmed(store.StoreId) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);

            //Clean up
            dbContext.Dispose();
        }
    }
}
