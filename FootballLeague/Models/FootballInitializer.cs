using System;
using System.Data.Entity;

namespace FootballLeague.Models
{
	public class FootballInitializer : DropCreateDatabaseIfModelChanges<FootballContext>, IDatabaseInitializer<FootballContext>
	{
		public FootballInitializer ()
		{
		}
	}
}

