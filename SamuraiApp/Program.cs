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
            AddQuotetoExistingSamuraiNotTracked(1);
        }
        static void AddSamurais(params string[] names)
        {
            _context.AddRange(new Samurai { Name = "Pentel" }, new Samurai { Name = "Explorer"},
                              new Battle { Name = "Anegawa"}, new Battle { Name = "Nagashino"});
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
            using(var context1 = new SamuraiContext())
            {
                disconectedBattles = _context.Battles.ToList();
            }
            disconectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 1, 1);
                b.EndDate = new DateTime(1570, 12, 1);
            });
            using(var context2 = new SamuraiContext())
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
            using(var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }
    }
}