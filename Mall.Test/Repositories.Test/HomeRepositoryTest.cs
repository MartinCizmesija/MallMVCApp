using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Mall.Repositories;
using Mall.Models;
using System;

namespace Mall.Test
{
    [TestClass]
    public class HomeRepositoryTest
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
            var repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };

            //Act
            var result = repository.Add(mall);

            //Assert
            Assert.IsTrue(result.MallId == 1);
            Assert.IsTrue(result.MallName == "MallName");
            Assert.IsTrue(result.MallDescription == "MallDescription");

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new HomeRepository(dbContext);
            var mall = new MallCenter { MallId = 1, MallName = "MallName", MallDescription = "MallDescription" };
            var obj = repository.Add(mall);

            //Act
            var result = repository.Get(obj.MallId);

            //Assert
            Assert.IsTrue(result.MallId == 1);
            Assert.IsTrue(result.MallName == "MallName");
            Assert.IsTrue(result.MallDescription == "MallDescription");

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetFail()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new HomeRepository(dbContext);

            //Act
            var result = repository.Get(0);

            //Assert
            Assert.IsNull(result);

            //Clean up
            dbContext.Dispose();
        }
    }
}
