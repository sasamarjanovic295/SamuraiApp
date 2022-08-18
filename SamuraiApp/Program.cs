using System;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        public static void Main()
        {
            GetHorseWithSamurai();
        }
        static void AddSamurais(params string[] names)
        {
            _context.AddRange(new Samurai { Name = "Pentel" }, new Samurai { Name = "Explorer" },
                              new Battle { Name = "Anegawa" }, new Battle { Name = "Nagashino" });
            _context.SaveChanges();
        }
        static void GetSamurais()
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            System.Console.WriteLine($"Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                System.Console.WriteLine(samurai.Name);
            }
        }
        private static void QueryFilters()
        {
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var samurais = _context.Samurais.Where(
                s => EF.Functions.Like(s.Name, "%ma%"));
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
        private static void QueryAggregates()
        {
            //var name = "sale";
            //var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
            var samurai = _context.Samurais.Find(2);
        }
        private static void RetriveAndUpdateSamurai()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }
        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }
        private static void RemoveSamurai()
        {
            var samurai = _context.Samurais.Find(6);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }
        private static void QueryAndUpdateBattles_Disconected()
        {
            List<Battle> disconectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconectedBattles = _context.Battles.ToList();
            }
            disconectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 1, 1);
                b.EndDate = new DateTime(1570, 12, 1);
            });
            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconectedBattles);
                context2.SaveChanges();
            }
        }
        //Interacting with Related Data
        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote{Text = "I've come to save you"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you are happy that I've saved you"
            });
            _context.SaveChanges();
        }
        private static void AddQuotetoExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }
        private static void Simpler_AddQuotetoExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }
        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais
                .Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList();
        }
        private static void ProjectSomeProperties()
        {
            var samuraisAndQuotes = _context.Samurais
            .Select(s => new
            {
                s.Id,
                s.Name,
                NumberOfQuotes = s.Quotes.Count,
                HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
            })
            .ToList();
        }
        private static void ExplicitLoadQuotes()
        {
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var samurai = _context.Samurais.Find();
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }
        private static void LazyLoadQuotes()
        {
            var samurai = _context.Samurais.Find(1);
            var quoteCount = samurai.Quotes.Count();
        }
        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                .ToList();
        }
        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                .FirstOrDefault(s => s.Id == 1);
            samurai.Quotes[0].Text = "Did you hear that";
            _context.SaveChanges();
        }
        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                           .FirstOrDefault(s => s.Id == 1);
            var quote = samurai.Quotes[0];
            quote.Text += "Did you hear that again?";
            using var newContext = new SamuraiContext();
            //newContext.Quotes.Update(quote);
            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();
        }
        private static void AddingNewSamuraiToAnExistingBattle()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Adds sadf" });
            _context.SaveChanges();
        }
        private static void ReturnBattleWithSamurais()
        {
            var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
        }
        private static void ReturnAllBattlesWithSamurais()
        {
            var battle = _context.Battles.Include(b => b.Samurais).ToList();
        }
        private static void AddAllSamuraisToAllBattles()
        {
            var allbattles = _context.Battles.Include(b => b.Samurais).ToList();
            var allsamurais = _context.Samurais.ToList();
            foreach (var battle in allbattles)
            {
                battle.Samurais.AddRange(allsamurais);
            }
            _context.SaveChanges();
        }
        private static void RemoveSamuraiFromBattle()
        {
            var battleWithSamurai = _context.Battles
                .Include(b => b.Samurais.Where(s => s.Id == 3))
                .Single(s => s.BattleId == 3);
            var samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }
        private static void RemoveSamuraiFromBattleExplicit()
        {
            var b_s = _context.Set<BattleSamurai>()
                .SingleOrDefault(bs => bs.BattlesId == 1 && bs.SamuraisId == 1);
            if (b_s != null)
            {
                _context.Remove(b_s);
                _context.SaveChanges();
            }
        }
        private static void AddNewHorseToSamuraiUisngId()
        {
            var horse = new Horse { Name = "Scout", SamuraiId = 3 };
            _context.Add(horse);
            _context.SaveChanges();
        }
        private static void AddNewHorseToSamuraiObject()
        {
            var samurai = _context.Samurais.Find(5);
            samurai.Horse = new Horse { Name = "Black Beauty" };
            _context.SaveChanges();
        }
        private static void AddNewHorsetoDisconectedSAmuraiObject()
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 5);
            samurai.Horse = new Horse { Name = "Black Saasd" };
            using var newContext = new SamuraiContext();
            newContext.Samurais.Attach(samurai);
            newContext.SaveChanges();
        }
        private static void ReplaceHorse()
        {
            var samurai = _context.Samurais.Include(s => s.Horse)
              .FirstOrDefault(s => s.Id == 5);
            samurai.Horse = new Horse { Name = "Trigger" };
        }
        private static void GetSamuraiWithHorse()
        {
            var samurai = _context.Samurais.Include(s => s.Horse).ToList();
        }
        private static void GetHorseWithSamurai()
        {
            var horseWithSamrai = _context.Samurais.Include(s => s.Horse)
                .FirstOrDefault(s => s.Horse.Id == 3);
            var horseSamuraiPairs = _context.Samurais
                .Where(s => s.Horse != null)
                .Select(s => new { Horse = s.Horse, Samurai = s })
                .ToList();
            var horseonly = _context.Set<Horse>().Find(5);
        }
    }
}