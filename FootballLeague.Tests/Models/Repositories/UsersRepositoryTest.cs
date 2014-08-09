using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeague.Tests.Models.Repositories
{
    [TestFixture]
    public class UsersRepositoryTest : RepositoryTestBase
    {
        FootballContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = MockRepository.GenerateMock<FootballContext>();
        }

        [Test]
        public void GetAllUsers_GetsAllUsers()
        {
            var users = new List<User> { new User(), new User() };
            _context.Users = MockContextData(_context, c => c.Users, users.AsQueryable());
            var repo = new UsersRepository(_context);

            Assert.That(repo.GetAllUsers(), Is.EqualTo(users));
        }

        [Test]
        public void GetUser_GetsUserById()
        {
            var users = new List<User> { new User { Id = 1 }, new User { Id = 2 } };
            _context.Users = MockContextData(_context, c => c.Users, users.AsQueryable());
            var repo = new UsersRepository(_context);

            Assert.That(repo.GetUser(2), Is.EqualTo(users[1]));
        }

        [Test]
        public void GetUser_WhenNotFoundId_ReturnsNull()
        {
            var users = new List<User> { new User { Id = 1 }, new User { Id = 2 } };
            _context.Users = MockContextData(_context, c => c.Users, users.AsQueryable());
            var repo = new UsersRepository(_context);

            Assert.IsNull(repo.GetUser(7));
        }

        [Test]
        public void GetUser_GetsUserByName()
        {
            var users = new List<User> { new User { Name = "Janko" }, new User { Name = "Marienka" } };
            _context.Users = MockContextData(_context, c => c.Users, users.AsQueryable());
            var repo = new UsersRepository(_context);

            Assert.That(repo.GetUser("Marienka"), Is.EqualTo(users[1]));
        }

        [Test]
        public void GetUser_WhenNotFoundName_ReturnsNull()
        {
            var users = new List<User> { new User { Name = "Janko" }, new User { Name = "Marienka" } };
            _context.Users = MockContextData(_context, c => c.Users, users.AsQueryable());
            var repo = new UsersRepository(_context);

            Assert.IsNull(repo.GetUser("Hrasko"));
        }

        [Test]
        public void InsertUser_AddsUser()
        {
            var user = new User();
            _context.Users = MockContextData(_context, c => c.Users, new List<User>().AsQueryable());
            _context.Users.Expect(u => u.Add(Arg<User>.Is.Same(user))).Return(null);
            _context.Expect(c => c.SaveChanges()).Return(0);
            var repo = new UsersRepository(_context);

            repo.InsertUser(user);

            _context.Users.VerifyAllExpectations();
            _context.VerifyAllExpectations();
        }

        [Test]
        public void DeleteUser_RemovesUser()
        {
            var user = new User { Id = 1 };
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { user }.AsQueryable());
            _context.Users.Expect(u => u.Remove(Arg<User>.Is.Same(user))).Return(null);
            _context.Expect(c => c.SaveChanges()).Return(0);
            var repo = new UsersRepository(_context);

            repo.DeleteUser(1);

            _context.Users.VerifyAllExpectations();
            _context.VerifyAllExpectations();
        }
    }
}
