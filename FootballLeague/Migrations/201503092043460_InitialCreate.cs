namespace FootballLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlannedTime = c.DateTime(nullable: false),
                        Creator_Id = c.Int(),
                        Team1_Id = c.Int(nullable: false),
                        Team2_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .ForeignKey("dbo.Teams", t => t.Team1_Id)
                .ForeignKey("dbo.Teams", t => t.Team2_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.Team1_Id)
                .Index(t => t.Team2_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        Mail = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Team1Score = c.Int(nullable: false),
                        Team2Score = c.Int(nullable: false),
                        Match_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Matches", t => t.Match_Id, cascadeDelete: true)
                .Index(t => t.Match_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Member1_Id = c.Int(),
                        Member2_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Member1_Id)
                .ForeignKey("dbo.Users", t => t.Member2_Id)
                .Index(t => t.Member1_Id)
                .Index(t => t.Member2_Id);
            
            CreateTable(
                "dbo.TournamentMatches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Team1_Id = c.Int(),
                        Team2_Id = c.Int(),
                        Tournament_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TournamentTeams", t => t.Team1_Id)
                .ForeignKey("dbo.TournamentTeams", t => t.Team2_Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id, cascadeDelete: true)
                .Index(t => t.Team1_Id)
                .Index(t => t.Team2_Id)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.TournamentSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Team1Score = c.Int(nullable: false),
                        Team2Score = c.Int(nullable: false),
                        TournamentMatch_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TournamentMatches", t => t.TournamentMatch_Id, cascadeDelete: true)
                .Index(t => t.TournamentMatch_Id);
            
            CreateTable(
                "dbo.TournamentTeams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                        TournamentId = c.Int(nullable: false),
                        Member1_Id = c.Int(),
                        Member2_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Member1_Id)
                .ForeignKey("dbo.Users", t => t.Member2_Id)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId)
                .Index(t => t.Member1_Id)
                .Index(t => t.Member2_Id);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EliminationType = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                        Creator_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .Index(t => t.Creator_Id);
            
            CreateTable(
                "dbo.MatchUsers",
                c => new
                    {
                        Match_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Match_Id, t.User_Id })
                .ForeignKey("dbo.Matches", t => t.Match_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Match_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentMatches", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentMatches", "Team2_Id", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentMatches", "Team1_Id", "dbo.TournamentTeams");
            DropForeignKey("dbo.TournamentTeams", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Tournaments", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.TournamentTeams", "Member2_Id", "dbo.Users");
            DropForeignKey("dbo.TournamentTeams", "Member1_Id", "dbo.Users");
            DropForeignKey("dbo.TournamentSets", "TournamentMatch_Id", "dbo.TournamentMatches");
            DropForeignKey("dbo.Matches", "Team2_Id", "dbo.Teams");
            DropForeignKey("dbo.Matches", "Team1_Id", "dbo.Teams");
            DropForeignKey("dbo.Teams", "Member2_Id", "dbo.Users");
            DropForeignKey("dbo.Teams", "Member1_Id", "dbo.Users");
            DropForeignKey("dbo.Sets", "Match_Id", "dbo.Matches");
            DropForeignKey("dbo.MatchUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.MatchUsers", "Match_Id", "dbo.Matches");
            DropForeignKey("dbo.Matches", "Creator_Id", "dbo.Users");
            DropIndex("dbo.MatchUsers", new[] { "User_Id" });
            DropIndex("dbo.MatchUsers", new[] { "Match_Id" });
            DropIndex("dbo.Tournaments", new[] { "Creator_Id" });
            DropIndex("dbo.TournamentTeams", new[] { "Member2_Id" });
            DropIndex("dbo.TournamentTeams", new[] { "Member1_Id" });
            DropIndex("dbo.TournamentTeams", new[] { "TournamentId" });
            DropIndex("dbo.TournamentSets", new[] { "TournamentMatch_Id" });
            DropIndex("dbo.TournamentMatches", new[] { "Tournament_Id" });
            DropIndex("dbo.TournamentMatches", new[] { "Team2_Id" });
            DropIndex("dbo.TournamentMatches", new[] { "Team1_Id" });
            DropIndex("dbo.Teams", new[] { "Member2_Id" });
            DropIndex("dbo.Teams", new[] { "Member1_Id" });
            DropIndex("dbo.Sets", new[] { "Match_Id" });
            DropIndex("dbo.Matches", new[] { "Team2_Id" });
            DropIndex("dbo.Matches", new[] { "Team1_Id" });
            DropIndex("dbo.Matches", new[] { "Creator_Id" });
            DropTable("dbo.MatchUsers");
            DropTable("dbo.Tournaments");
            DropTable("dbo.TournamentTeams");
            DropTable("dbo.TournamentSets");
            DropTable("dbo.TournamentMatches");
            DropTable("dbo.Teams");
            DropTable("dbo.Sets");
            DropTable("dbo.Users");
            DropTable("dbo.Matches");
        }
    }
}
