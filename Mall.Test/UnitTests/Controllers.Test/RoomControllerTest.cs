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
    public class RoomControllerTest
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
            var repository = new RoomRepository(dbContext);
            var appSettings = new AppSettings() { ConnectionString = "..." };
            var options = CreateIOptionSnapshotMock(appSettings);
            var controller = new RoomsController(options, repository, factory);
            var mall_repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            repository.Add(room);

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
            var repository = new RoomRepository(dbContext);
            var appSettings = new AppSettings() { ConnectionString = "..." };
            var options = CreateIOptionSnapshotMock(appSettings);
            var controller = new RoomsController(options, repository, factory);
            var mall_repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };

            // Act
            var result = controller.Create(room) as RedirectToActionResult;

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
            var repository = new RoomRepository(dbContext);
            var appSettings = new AppSettings() { ConnectionString = "..." };
            var options = CreateIOptionSnapshotMock(appSettings);
            var controller = new RoomsController(options, repository, factory);
            var mall_repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            repository.Add(room);

            //Act
            Room null_room = null;
            var result = controller.Edit(null_room) as NotFoundResult;

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
            var repository = new RoomRepository(dbContext);
            var appSettings = new AppSettings() { ConnectionString = "..." };
            var options = CreateIOptionSnapshotMock(appSettings);
            var controller = new RoomsController(options, repository, factory);
            var mall_repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            mall_repository.Add(mall);
            var room = new Room { MallId = mall.MallId, Rent = 100.0, IsAvailable = true };
            repository.Add(room);

            // Act
            var result = controller.DeleteConfirmed(room.RoomId) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);

            //Clean up
            dbContext.Dispose();
        }
    }
}
