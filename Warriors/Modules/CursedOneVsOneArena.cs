using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Warriors.Models;

namespace Warriors.Modules
{
    internal class CursedOneVsOneArena : ArenaBase
    {
        private const int REGEN_AMOUNT = 10;

        public CursedOneVsOneArena(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override string GetName()
        {
            return "Elátkozott 1v1 Aréna";
        }

        public override void ListRules()
        {
            Console.WriteLine("Ebben az arénában minden körben 2 véletlenszerüen választott harcos küzd meg egymással.");
            Console.WriteLine("Az aréna el van átkozva: a harcosok életereje minden kör végén a felére csökken. " +
                "Ha az életerejük így a maximum negyede alá esik, meghalnak. A pihenö harcosok életereje 10-zel növekszik.");
            Console.WriteLine("Csak egy maradhat!");
        }

        public override bool IsCombatReady()
        {
            return _warriors.Count(x => x.IsAlive) >= 2;
        }

        public override void ListResults()
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

        public override void DoArenaEffect()
        {
            var casualties = new List<IWarrior>();

            Console.WriteLine("Lesújt az átok...");

            foreach (IWarrior warrior in _warriors)
            {
                if (!warrior.IsAlive)
                {
                    continue;
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
                Console.WriteLine($"A következö harcosokat elragadta az átok: {string.Join(", ", casualties.Select(x => x.Name))}.");
            }
        }

        public override void DoCombatRound()
        {
            if (!IsCombatReady())
            {
                return;
            }

            Random rnd = new Random();
            List<IWarrior> liveWarriors = _warriors.Where(w => w.IsAlive).ToList();

            IWarrior attacker = liveWarriors[rnd.Next(liveWarriors.Count)];
            liveWarriors.Remove(attacker);
            IWarrior defender = liveWarriors[rnd.Next(liveWarriors.Count)];
            liveWarriors.Remove(defender);

            attacker.Attack(defender);
            Console.WriteLine($"Harc kör: {attacker.Name} megtámadta {defender.Name}-t");

            if (attacker.IsAlive && defender.IsAlive)
            {
                Console.WriteLine("...és kiegyeztek egy döntetlenben.");
            }
            else if (attacker.IsAlive && !defender.IsAlive)
            {
                Console.WriteLine($"...és legyözte! Béke {defender.Name} poraira.");
            }
            else if (!attacker.IsAlive && defender.IsAlive)
            {
                Console.WriteLine($"...de túl nagy fába vágta a fejszéjét. Béke {attacker.Name} poraira.");
            }
            else
            {
                Console.WriteLine($"...de az összecsapást egyikük sem élte túl.");
            }

            liveWarriors.ForEach(x => x.Regenerate(REGEN_AMOUNT));
            Console.WriteLine($"A többi harcos pihent (+{REGEN_AMOUNT}).");
        }
    }
}
