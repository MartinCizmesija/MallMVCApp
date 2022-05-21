using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Mall.Repositories;
using Mall.Models;
using System;

namespace Mall.Test
{
    [TestClass]
    public class CategoryRepositoryTest
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
            var repository = new CategoryRepository(dbContext);
            var category = new Category { CategoryName = "CategoryName", CategoryDescription = "CategoryDescription" };

            //Act
            var result = repository.Add(category);

            //Assert
            Assert.IsTrue(result.CategoryId > 0);
            Assert.IsTrue(result.CategoryName == "CategoryName");
            Assert.IsTrue(result.CategoryDescription == "CategoryDescription");

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var category = new Category { CategoryName = "CategoryName", CategoryDescription = "CategoryDescription" };
            var obj = repository.Add(category);

            //Act
            var result = repository.Get(obj.CategoryId);

            //Assert
            Assert.IsTrue(result.CategoryId > 0);
            Assert.IsTrue(result.CategoryName == "CategoryName");
            Assert.IsTrue(result.CategoryDescription == "CategoryDescription");

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetFail()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new CategoryRepository(dbContext);

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
            var repository = new CategoryRepository(dbContext);
            var category = new Category { CategoryName = "CategoryName", CategoryDescription = "CategoryDescription" };
            var obj = repository.Add(category);

            //Act
            obj.CategoryName = "NEWCategoryName";
            obj.CategoryDescription = "NEWCategoryDescription";
            var result = repository.Update(obj);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(repository.Get(obj.CategoryId).CategoryName == "NEWCategoryName");
            Assert.IsTrue(repository.Get(obj.CategoryId).CategoryDescription == "NEWCategoryDescription");

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestDeleteSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var category = new Category { CategoryName = "CategoryName", CategoryDescription = "CategoryDescription" };
            var obj = repository.Add(category);

            //Act
            repository.Delete(category);

            //Assert
            Assert.IsNull(repository.Get(obj.CategoryId));

            //Clean up
            dbContext.Dispose();
        }

        [TestMethod]
        public void TestGetListSuccess()
        {
            //Arrange
            var dbContext = CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var category1 = new Category { CategoryName = "CategoryName1", CategoryDescription = "CategoryDescription1" };
            var category2 = new Category { CategoryName = "CategoryName2", CategoryDescription = "CategoryDescription2" };
            var category3 = new Category { CategoryName = "CategoryName3", CategoryDescription = "CategoryDescription3" };
            repository.Add(category1);
            repository.Add(category2);
            repository.Add(category3);

            //Act
            var result = repository.GetList();

            //Assert
            Assert.IsTrue(result.CountAsync().Result == 3);

            //Clean up
            dbContext.Dispose();
        }
    }
}
