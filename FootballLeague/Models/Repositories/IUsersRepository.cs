using System;
using System.Collections.Generic;

namespace FootballLeague.Models.Repositories
{
    public interface IUsersRepository
    {
        void DeleteUser(int id);
        IEnumerable<User> GetAllUsers();
        User GetUser(int id);
        User GetUser(string name);
        void InsertUser(User user);
    }
}
