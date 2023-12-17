using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Models
{
    internal class Archer : WarriorBase
    {
        public override string ClassName => "Íjász";

        public override int BaseHP => 100;

        public override void Attack(IWarrior defender)
        {
            switch (defender)
            {
                case Knight:
                    if (new Random().Next(10) < 4)
                    {
                        defender.Die();
                    }
                    break;
                case Fighter:
                    defender.Die();
                    break;
                case Archer:
                    defender.Die();
                    break;
                default:
                    base.Attack(defender);
                    break;
            }
        }
    }
}
