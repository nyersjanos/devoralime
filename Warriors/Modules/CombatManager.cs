using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Warriors.Modules
{
    internal class CombatManager : ICombatManager
    {
        private const int MIN_WARRIORS = 2;
        private const int MAX_WARRIORS = 1000;

        private readonly IServiceProvider _serviceProvider;

        public CombatManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Start()
        {
            ICombatArena arena = SelectArena();

            Console.WriteLine();
            Console.WriteLine($"Üdvözöllek a(z) {arena.GetName()} arénában!");
            arena.ListRules();
            Console.WriteLine($"Elérhetö harcostípusok: {string.Join(", ", arena.GetAvailableWarriorClassNames())}.");

            RunArena(arena);
        }

        private ICombatArena SelectArena()
        {
            Console.WriteLine("Kérlek válassz arénát:");

            List<ICombatArena> availableArenas = _serviceProvider.GetServices<ICombatArena>().ToList();
            var arenaDictionary = new Dictionary<int, ICombatArena>();

            Console.Write("[");

            for (int i = 1; i <= availableArenas.Count; i++)
            {
                arenaDictionary.Add(i, availableArenas[i-1]);
                Console.Write($"{i} - {arenaDictionary[i].GetName()}");

                if (i < availableArenas.Count)
                {
                    Console.Write(", ");
                }
            }

            Console.WriteLine("]");

            return ReadArenaSelection(arenaDictionary);
        }

        private ICombatArena ReadArenaSelection(Dictionary<int, ICombatArena> arenaDictionary)
        {
            var key = Console.ReadKey(true);

            if (int.TryParse(key.KeyChar.ToString(), out int number) && arenaDictionary.ContainsKey(number))
            {
                return arenaDictionary[number];
            }

            return ReadArenaSelection(arenaDictionary);
        }

        private void RunArena(ICombatArena arena)
        {
            int numberOfWarriors = GetNumberOfWarriors();
            int round = 0;

            arena.GenerateWarriors(numberOfWarriors);
            arena.ListWarriors();

            Console.WriteLine();
            Console.WriteLine("Kezdödik a harc!");

            while (arena.IsCombatReady() && ContinuePrompt())
            {
                round++;
                Console.WriteLine();
                Console.WriteLine($"{round}. kör:");

                arena.DoCombatRound();
                arena.DoArenaEffect();
                arena.ListWarriors();
            }

            Console.WriteLine("A harc véget ért.");

            arena.ListResults();

            AskToRestart(arena);
        }

        private void AskToRestart(ICombatArena arena)
        {
            Console.WriteLine("Szeretnél még egy kört játszani?(Y/N)");

            if (CheckRestartKey())
            {
                RunArena(arena);
            }
            else
            {
                Console.WriteLine($"{arena.GetName()} játék véget ért.");
            }
        }

        private static bool CheckRestartKey()
        {
            var key = Console.ReadKey(true);

            if (char.ToUpperInvariant(key.KeyChar) == 'Y')
            {
                return true;
            }
            else if (char.ToUpperInvariant(key.KeyChar) == 'N')
            {
                return false;
            }

            return CheckRestartKey();
        }

        private static bool ContinuePrompt()
        {
            Console.WriteLine("[Q - kilépés, Enter - folytatás]");

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                return true;
            }
            else if (char.ToUpperInvariant(key.KeyChar) == 'Q')
            {
                return false;
            }

            return ContinuePrompt();
        }

        private static int GetNumberOfWarriors()
        {
            Console.Write("Mennyi harcost szeretnél? (írj be egy számot): ");
            var stringWarriors = Console.ReadLine();

            if (int.TryParse(stringWarriors, out int numberOfWarriors))
            {
                if (numberOfWarriors >= MIN_WARRIORS && numberOfWarriors <= MAX_WARRIORS)
                {
                    return numberOfWarriors;
                }
                else
                {
                    Console.WriteLine($"A harcosok számának {MIN_WARRIORS} és {MAX_WARRIORS} közé kell esnie!");
                    return GetNumberOfWarriors();
                }
            }
            else
            {
                Console.WriteLine("Kérlek számot írj be!");
                return GetNumberOfWarriors();
            }
        }
    }
}
