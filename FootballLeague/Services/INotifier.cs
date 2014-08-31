using FootballLeague.Models;
using System;
using System.Collections.Generic;

namespace FootballLeague.Services
{
    public interface INotifier
    {
        void Notify(User user, IEnumerable<User> invites, DateTime time);
    }
}
