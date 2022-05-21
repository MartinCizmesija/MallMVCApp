using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Mall.Repositories;
using Mall.Controllers;
using Mall.Models;
using System;
using Moq;

namespace Mall.Test
{
    [TestClass]
    public class HomeControllerTest
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
            var repository = new HomeRepository(dbContext);
            var controller = new HomeController(repository);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestPrivacy()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new HomeRepository(dbContext);
            var controller = new HomeController(repository);

            // Act
            var result = controller.Privacy() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            //Clean up
            dbContext.Dispose();
        }
    }
}
