using System.Collections.Generic;
using System.Linq;

namespace FootballLeague.Models.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private FootballContext _context;

        public UsersRepository(FootballContext context = null)
        {
            _context = context ?? new FootballContext();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void InsertUser(User user)
        {
            if (GetUser(user.Name) != null)
                return;

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
                return;

            _context.Users.Remove(user);
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
    }
}