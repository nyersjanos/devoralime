using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Models
{
    internal class Fighter : WarriorBase
    {
        public override string ClassName => "Kardos";

        public override int BaseHP => 120;

        public override void Attack(IWarrior defender)
        {
            switch (defender)
            {
                case Knight:
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
