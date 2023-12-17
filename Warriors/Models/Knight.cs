using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Models
{
    internal class Knight : WarriorBase
    {
        public override string ClassName => "Lovas";

        public override int BaseHP => 150;

        public override void Attack(IWarrior defender)
        {
            switch (defender)
            {
                case Knight:
                    defender.Die();
                    break;
                case Fighter:
                    this.Die();
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
