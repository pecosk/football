using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using NUnit.Framework;
using FootballLeague;
using FootballLeague.Controllers;
using FootballLeague.Models;

namespace FootballLeague.Tests.Controllers
{
    [TestFixture]
    public class ValuesControllerTest
    {
        [Test]
        public void Get()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            IEnumerable<User> result = controller.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(2, Is.EqualTo(result.Count()));
            Assert.That("value1", Is.EqualTo(result.ElementAt(0)));
            Assert.That("value2", Is.EqualTo(result.ElementAt(1)));
        }

        [Test]
        public void GetById()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.That("value", Is.EqualTo(result));
        }

        [Test]
        public void Post()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            controller.Post();

            // Assert
        }

        [Test]
        public void Put()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [Test]
        public void Delete()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            controller.Delete(1);

            // Assert
        }
    }
}
