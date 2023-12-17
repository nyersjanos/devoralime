using Microsoft.Extensions.DependencyInjection;
using System;
using Warriors.Models;
using Warriors.Modules;

namespace Warriors
{
    internal class Program
    {
        private static ServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            Initialize();

            Console.WriteLine("Nyers János - Devora Lime tesztfeladat");
            Console.WriteLine("Indulhat a játék!");

            var manager = _serviceProvider.GetService<ICombatManager>();
            manager.Start();

            Console.WriteLine("Játék vége, köszönöm!");
            Console.ReadLine();
        }

        private static void Initialize()
        {
            _serviceProvider = new ServiceCollection()
            .AddTransient<IWarrior, Archer>()
            .AddTransient<IWarrior, Fighter>()
            .AddTransient<IWarrior, Knight>()
            .AddScoped<ICombatArena, OneVsOneArena>()
            .AddSingleton<ICombatManager, CombatManager>()
            .BuildServiceProvider();
        }
    }
}
