using System;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        public static void Main()
        {
            _context.Database.EnsureCreated();
            GetSamurais("Before Add:");
            AddSamurai("saleZmaj");
            GetSamurais("After Add:");
            System.Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
        static void AddSamurai(string name)
        {
            var samurai = new Samurai { Name = name};
            _context.Add(samurai);
            _context.SaveChanges();
        }
        static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            System.Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                System.Console.WriteLine(samurai.Name);
            }
        }
    }
}