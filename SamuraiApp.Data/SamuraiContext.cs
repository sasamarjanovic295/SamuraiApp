using System;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System.Linq;

namespace SamuraiApp.Data
{
	public class SamuraiContext : DbContext
	{
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public List<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=192.168.0.112, 1434;Initial Catalog=SamuraiAppDb; User ID=novistudent;Password=novistudent");
            base.OnConfiguring(optionsBuilder);
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
        }
    }
}

