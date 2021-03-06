﻿using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using FootballLeague.Tests.Data;
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
        public void GetAllUsers_GetsAllActiveUsers()
        {
            var users = new List<User> { new User(), new User(), new User { Inactive = true } };
            _context.Users = MockContextData(_context, c => c.Users, users.AsQueryable());
            var repo = new UsersRepository(_context);

            var activeUsers = repo.GetAllUsers();

            Assert.That(activeUsers.Count(), Is.EqualTo(2));
            Assert.That(activeUsers.First(), Is.EqualTo(users[0]));
            Assert.That(activeUsers.Last(), Is.EqualTo(users[1]));
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
        public void InsertUser_WhenUserDoesNotExist_AddsUser()
        {
            var user = new User { Name = "Janko" };
            _context.Users = MockContextData(_context, c => c.Users, new List<User>().AsQueryable());
            _context.Users.Expect(u => u.Add(Arg<User>.Is.Same(user))).Return(null);
            _context.Expect(c => c.SaveChanges()).Return(0);
            var repo = new UsersRepository(_context);

            repo.InsertUser(user);

            _context.Users.VerifyAllExpectations();
            _context.VerifyAllExpectations();
        }

        [Test]
        public void InsertUser_WhenUserExists_DoesNothing()
        {
            var user = new User { Name = "Janko" };
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { user }.AsQueryable());
            _context.Users.Expect(u => u.Add(Arg<User>.Is.Anything)).Repeat.Never();
            _context.Expect(c => c.SaveChanges()).Repeat.Never();
            var repo = new UsersRepository(_context);

            repo.InsertUser(user);

            _context.Users.VerifyAllExpectations();
            _context.VerifyAllExpectations();
        }

        [Test]
        public void InsertUser_WhenUserExistsButInactive_SetsHimToActive()
        {
            var user = new User { Name = "Janko", Inactive = true };
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { user }.AsQueryable());
            _context.Users.Expect(u => u.Add(Arg<User>.Is.Anything)).Repeat.Never();
            _context.Expect(c => c.SaveChanges()).Return(0);
            var repo = new UsersRepository(_context);

            repo.InsertUser(user);

            _context.Users.VerifyAllExpectations();
            _context.VerifyAllExpectations();
            Assert.IsFalse(user.Inactive);
        }

        [Test]
        public void DeleteUser_MakesHimInactive()
        {
            var user = new User { Id = 1 };
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { user }.AsQueryable());
            _context.Users.Expect(u => u.Remove(Arg<User>.Is.Same(user))).Repeat.Never();
            _context.Expect(c => c.SaveChanges()).Return(0);
            var repo = new UsersRepository(_context);

            repo.DeleteUser(1);

            _context.Users.VerifyAllExpectations();
            _context.VerifyAllExpectations();
            Assert.IsTrue(user.Inactive);
        }

        [Test]
        public void UsersExist_WithOneExistingUser_ConfirmsExistance()
        {
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { Users.Dano, Users.Ferko }.AsQueryable());
            var repo = new UsersRepository(_context);

            var verifiedUsers = repo.GetVerifiedUsers(new List<User> { Users.Dano });

            Assert.That(verifiedUsers.Count(), Is.EqualTo(1));
        }

        [Test]
        public void UsersExist_WithNonExistingUser_DeniesExistance()
        {
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { Users.Dano, Users.Ferko }.AsQueryable());
            var repo = new UsersRepository(_context);

            var verifiedUsers = repo.GetVerifiedUsers(new List<User> { Users.Jurko });

            Assert.That(verifiedUsers.Count(), Is.EqualTo(0));
        }

        [Test]
        public void UsersExist_WithAllUsersExisting_ConfirmsExistance()
        {
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { Users.Dano, Users.Ferko }.AsQueryable());
            var repo = new UsersRepository(_context);

            var verifiedUsers = repo.GetVerifiedUsers(new List<User> { Users.Dano, Users.Ferko });

            Assert.That(verifiedUsers.Count(), Is.EqualTo(2));
        }

        [Test]
        public void UsersExist_WithOnlyFirstUserExisting_DeniesExistance()
        {
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { Users.Dano, Users.Ferko }.AsQueryable());
            var repo = new UsersRepository(_context);

            var verifiedUsers = repo.GetVerifiedUsers(new List<User> { Users.Dano, Users.Jurko });

            Assert.That(verifiedUsers.Count(), Is.EqualTo(1));
            Assert.That(verifiedUsers.ToList()[0], Is.EqualTo(Users.Dano));
        }

        [Test]
        public void UsersExist_WithOnlySecondUserExisting_DeniesExistance()
        {
            _context.Users = MockContextData(_context, c => c.Users, new List<User> { Users.Dano, Users.Ferko }.AsQueryable());
            var repo = new UsersRepository(_context);

            var verifiedUsers = repo.GetVerifiedUsers(new List<User> { Users.Jurko, Users.Ferko });

            Assert.That(verifiedUsers.Count(), Is.EqualTo(1));
            Assert.That(verifiedUsers.ToList()[0], Is.EqualTo(Users.Ferko));
        }
    }
}
