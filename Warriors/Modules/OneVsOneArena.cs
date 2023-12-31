﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Warriors.Models;

namespace Warriors.Modules
{
    internal class OneVsOneArena : ArenaBase
    {
        private const int REGEN_AMOUNT = 10;

        public OneVsOneArena(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override string GetName()
        {
            return "1v1 Aréna";
        }

        public override void ListRules()
        {
            Console.WriteLine("Ebben az arénában minden körben 2 véletlenszerüen választott harcos küzd meg egymással.");
            Console.WriteLine("A két csatázó harcos életereje a felére csökken, a pihenöké 10-zel növekszik.");
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

            ExhaustWarrior(attacker);
            ExhaustWarrior(defender);

            liveWarriors.ForEach(x => x.Regenerate(REGEN_AMOUNT));
            Console.WriteLine($"A többi harcos pihent (+{REGEN_AMOUNT}).");
        }

        private void ExhaustWarrior(IWarrior warrior)
        {
            if (warrior.IsAlive)
            {
                warrior.CurrentHP = warrior.CurrentHP / 2;

                if (warrior.CurrentHP < warrior.BaseHP / 4)
                {
                    warrior.Die();
                    Console.WriteLine($"{warrior.Name} harcban szerzett sebei halálosnak bizonyultak.");
                }
                else
                {
                    Console.WriteLine($"{warrior.Name} megsérült a harcban, új életereje: {warrior.CurrentHP}.");
                }
            }
        }
    }
}
