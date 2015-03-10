namespace FootballLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTrounamentRound : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TournamentMatches", "Tournament_Id", "dbo.Tournaments");
            DropIndex("dbo.TournamentMatches", new[] { "Tournament_Id" });
            CreateTable(
                "dbo.TournamentRounds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoundNumber = c.Int(nullable: false),
                        Tournament_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id, cascadeDelete: true)
                .Index(t => t.Tournament_Id);
            
            AddColumn("dbo.TournamentMatches", "MatchNumber", c => c.Int(nullable: false));
            AddColumn("dbo.TournamentMatches", "Round_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.TournamentMatches", "Round_Id");
            AddForeignKey("dbo.TournamentMatches", "Round_Id", "dbo.TournamentRounds", "Id", cascadeDelete: true);
            DropColumn("dbo.TournamentMatches", "Tournament_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TournamentMatches", "Tournament_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.TournamentMatches", "Round_Id", "dbo.TournamentRounds");
            DropForeignKey("dbo.TournamentRounds", "Tournament_Id", "dbo.Tournaments");
            DropIndex("dbo.TournamentRounds", new[] { "Tournament_Id" });
            DropIndex("dbo.TournamentMatches", new[] { "Round_Id" });
            DropColumn("dbo.TournamentMatches", "Round_Id");
            DropColumn("dbo.TournamentMatches", "MatchNumber");
            DropTable("dbo.TournamentRounds");
            CreateIndex("dbo.TournamentMatches", "Tournament_Id");
            AddForeignKey("dbo.TournamentMatches", "Tournament_Id", "dbo.Tournaments", "Id", cascadeDelete: true);
        }
    }
}
