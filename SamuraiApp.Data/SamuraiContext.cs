using System;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SamuraiApp.Data
{
	public class SamuraiContext : DbContext
	{
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Horse> Horses { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=192.168.0.114, 49172;Initial Catalog=SamuraiAppDb; User ID=novistudent;Password=novistudent",
                options => options.MaxBatchSize(100))
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name, DbLoggerCategory.Database.Transaction.Name}, LogLevel.Debug)
                .EnableSensitiveDataLogging();          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(),
                bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DataJoined)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStat");
        }
    }
}

