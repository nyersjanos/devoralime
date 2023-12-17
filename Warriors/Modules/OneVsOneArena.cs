using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Warriors.Models;

namespace Warriors.Modules
{
    internal class OneVsOneArena : ICombatArena
    {
        private readonly IServiceProvider _serviceProvider;
        private List<IWarrior> _warriors = new List<IWarrior>();

        public OneVsOneArena(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string GetName()
        {
            return "1v1 Aréna";
        }

        public void ListRules()
        {
            Console.WriteLine("Ebben az arénában minden körben 2 véletlenszerűen választott harcos küzd meg egymással.");
            Console.WriteLine("Az aréna el van átkozva: a harcosok életereje minden kör végén a felére csökken." +
                "Ha az életerejük így a maximum negyede alá esik, meghalnak.");
            Console.WriteLine("Csak egy maradhat!");
        }

        public List<string> GetAvailableWarriorClassNames()
        {
            return _serviceProvider.GetServices<IWarrior>().Select(x => x.ClassName).ToList();
        }

        public void GenerateWarriors(int numberOfWarriors)
        {
            _warriors = new List<IWarrior>();

            Random rnd = new Random();
            int classCount = _serviceProvider.GetServices<IWarrior>().Count();

            for (int i = 0; i < numberOfWarriors; i++)
            {
                IWarrior warrior = _serviceProvider.GetServices<IWarrior>().ToArray()[i];
                warrior.SetIndex(_warriors.Count(x => x.ClassName == warrior.ClassName) + 1);
                _warriors.Add(warrior);
            }
        }

        public void ListWarriors()
        {
            if (_warriors != null && _warriors.Count > 0)
            {
                Console.WriteLine($"Csatában lévő harcosok: {string.Join(", ", _warriors.Select(x => x.NameWithHP))}");
            }
            else
            {
                Console.WriteLine("Nincsenek elérhető harcosok.");
            }
        }

        public void DoCombatRound()
        {
            throw new NotImplementedException();
        }

        public void DoArenaEffect()
        {
            var casualties = new List<IWarrior>();

            Console.WriteLine("Lesújt az átok...");

            foreach (IWarrior warrior in _warriors)
            {
                if (!warrior.IsAlive)
                {
                    return;
                }

                warrior.CurrentHP = warrior.CurrentHP / 2;

                if (warrior.CurrentHP < warrior.BaseHP / 4)
                {
                    warrior.Die();
                    casualties.Add(warrior);
                }
            }

            if (casualties.Count > 0)
            {
                Console.WriteLine($"A következő harcosokat elragadta az átok: {string.Join(", ", casualties.Select(x => x.Name))}.");
            }
        }

        public bool IsCombatReady()
        {
            return _warriors.Count(x => x.IsAlive) >= 2;
        }

        public void ListResults()
        {
            var winners = _warriors.Where(x => x.IsAlive).ToList();

            if (winners.Count == 0)
            {
                Console.WriteLine("Mindenki elesett a harcban, nincs gyöztes.");
            }
            else if (winners.Count == 1)
            {
                Console.WriteLine($"!!! A gyöztes: {winners[0].NameWithHP} !!!");
            }
            else
            {
                Console.WriteLine($"A harc szabálytalanul ért véget.");
            }
        }
    }
}
