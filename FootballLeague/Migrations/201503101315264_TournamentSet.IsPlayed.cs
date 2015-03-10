namespace FootballLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentSetIsPlayed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentSets", "IsPlayed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TournamentSets", "IsPlayed");
        }
    }
}
