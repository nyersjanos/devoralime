using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Modules
{
    internal class CombatManager : ICombatManager
    {
        private const int MIN_WARRIORS = 2;
        private const int MAX_WARRIORS = 1000;

        private readonly ICombatArena _arena;

        public CombatManager(ICombatArena arena)
        {
            _arena = arena;
        }

        public void Start()
        {
            Console.WriteLine($"Üdvözöllek a(z) {_arena.GetName()} arénában!");
            _arena.ListRules();
            Console.WriteLine($"Elérhetö harcostípusok: {string.Join(", ", _arena.GetAvailableWarriorClassNames())}.");
            RunArena();
        }

        private void RunArena()
        {
            int numberOfWarriors = GetNumberOfWarriors();
            int round = 0;

            _arena.GenerateWarriors(numberOfWarriors);
            _arena.ListWarriors();

            Console.WriteLine("Kezdődik a harc! [Q - kilépés, Enter - folytatás]");

            while (_arena.IsCombatReady() && ContinuePrompt())
            {
                round++;
                Console.WriteLine($"{round}. kör:");

                _arena.DoCombatRound();
                _arena.DoArenaEffect();
                _arena.ListWarriors();
            }

            Console.WriteLine("A harc véget ért.");

            _arena.ListResults();

            AskToRestart();
        }

        private void AskToRestart()
        {
            Console.WriteLine("Szeretnél még egy kört játszani?(Y/N)");

            if (CheckRestartKey())
            {
                RunArena();
            }

            Console.WriteLine($"{_arena.GetName()} játék véget ért.");
        }

        private static bool CheckRestartKey()
        {
            var key = Console.ReadKey();

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
            var key = Console.ReadKey();

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
