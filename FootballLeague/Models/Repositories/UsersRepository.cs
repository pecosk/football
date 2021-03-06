﻿using System.Collections.Generic;
using System.Linq;

namespace FootballLeague.Models.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private FootballContext _context;

        public UsersRepository(FootballContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Where(u => u.Inactive == false).ToList();
        }

        public void InsertUser(User user)
        {
            var dbUser = GetUser(user.Name);
            if (dbUser != null && !dbUser.Inactive)
                return;

            if (dbUser == null)
                _context.Users.Add(user);
            else
                dbUser.Inactive = false;

            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
                return;

            user.Inactive = true;
            _context.SaveChanges();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name == name);
        }

        public IEnumerable<User> GetVerifiedUsers(IEnumerable<User> users)
        {
            var verifiedUsers = new List<User>();
            users.Select(u => u.Id).ToList().ForEach(id => {
                var verifiedUser = _context.Users.FirstOrDefault(u => u.Id == id);
                if (verifiedUser != null)
                    verifiedUsers.Add(verifiedUser);
            });
            return verifiedUsers;
        }
    }
}