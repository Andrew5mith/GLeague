using GLeague.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GLeague.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PlayerProfile> PlayerProfiles { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMembership> TeamMemberships { get; set; }
        public DbSet<SeasonRegistration> SeasonRegistrations { get; set; }
        public DbSet<Draft> Drafts { get; set; }
        public DbSet<DraftPick> DraftPicks { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerGameStat> PlayerGameStats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One-to-one profile
            builder.Entity<PlayerProfile>()
                .HasOne(p => p.User)
                .WithOne(u => u.PlayerProfile)
                .HasForeignKey<PlayerProfile>(p => p.UserId);

            // Captain relationship
            builder.Entity<Team>()
                .HasOne(t => t.Captain)
                .WithMany(u => u.CaptainedTeams)
                .HasForeignKey(t => t.CaptainId)
                .OnDelete(DeleteBehavior.Restrict);

            // Team membership
            builder.Entity<TeamMembership>()
                .HasOne(m => m.Player)
                .WithMany(u => u.TeamMemberships)
                .HasForeignKey(m => m.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Avoid cascade cycles for games
            builder.Entity<Game>()
                .HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Game>()
                .HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            // Unique stats per player per game
            builder.Entity<PlayerGameStat>()
                .HasIndex(s => new { s.GameId, s.PlayerId })
                .IsUnique();
        }

    }
}
