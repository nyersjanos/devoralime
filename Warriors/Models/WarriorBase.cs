using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Models
{
    internal abstract class WarriorBase : IWarrior
    {
        private const int REGEN_AMOUNT = 10;

        protected int _index;
        protected int _hp;

        public abstract string ClassName { get; }

        public string Name => ClassName + "_" + _index;

        public abstract int BaseHP { get; }

        public int CurrentHP
        {
            get
            {
                return _hp;
            }
            set
            {
                if (value <= 0)
                {
                    _hp = 0;
                }
                else if (value > BaseHP)
                {
                    _hp = BaseHP;
                }
                else
                {
                    _hp = value;
                }
            }
        }

        public bool IsAlive { get; private set; } = true;

        public string NameWithHP => $"{Name} ({CurrentHP})";

        public void SetIndex(int index)
        {
            _index = index;
        }

        public void Regenerate()
        {
            if (IsAlive)
            {
                CurrentHP += REGEN_AMOUNT;
            }
        }

        public void Die()
        {
            IsAlive = false;
        }
    }
}
