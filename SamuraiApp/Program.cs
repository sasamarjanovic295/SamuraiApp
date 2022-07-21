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
            //AddSamurais("sale", "zmaj");
            GetSamurais();
        }
        static void AddSamurais(params string[] names)
        {
            foreach (var name in names)
            {
                _context.Add(new Samurai { Name = name });
            }
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
    }
}