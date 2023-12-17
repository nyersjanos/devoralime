using Microsoft.Extensions.DependencyInjection;
using System;
using Warriors.Modules;

namespace Warriors
{
    internal class Program
    {
        private static ServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            Initialize();

            Console.WriteLine("Hello, World!");
        }

        private static void Initialize()
        {
            _serviceProvider = new ServiceCollection()
            .AddSingleton<ICombatArena, OneVsOneArena>()
            .AddSingleton<ICombatManager, CombatManager>()
            .BuildServiceProvider();
        }
    }
}
