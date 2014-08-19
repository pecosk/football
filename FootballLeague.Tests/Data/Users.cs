namespace FootballLeague.Tests.Data
{
    using FootballLeague.Models;

    public static class Users
    {
        private static User _ferko;

        private static User _jurko;

        private static User _dano;

        private static User _milan;

        private static User _peto;

        public static User Ferko
        {
            get
            {
                if (_ferko == null)
                {
                    _ferko = new User { Id = 1, Name = "Ferko", Inactive = false };
                }

                return _ferko;
            }
        }

        public static User Jurko
        {
            get
            {
                if (_jurko == null)
                {
                    _jurko = new User { Id = 2, Name = "Jurko", Inactive = false };
                }
                return _jurko;
            }
        }

        public static User Peto
        {
            get
            {
                if (_peto == null)
                {
                    _peto = new User { Id = 3, Name = "Peto", Inactive = false };
                }
                return _peto;
            }        
        }

        public static User Dano
        {
            get
            {
                if (_dano == null)
                {
                    _dano = new User { Id = 4, Name = "Dano", Inactive = false };
                }
                return _dano;
            }
        }

        public static User Milan
        {
            get
            {
                if (_milan == null)
                {
                    _milan = new User { Id = 5, Name = "Milan", Inactive = false };
                }
                return _milan;
            }
        }
    }
}