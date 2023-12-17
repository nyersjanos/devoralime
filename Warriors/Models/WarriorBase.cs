using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Models
{
    internal abstract class WarriorBase : IWarrior
    {
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

        public WarriorBase()
        {
            _hp = BaseHP;
        }

        public void SetIndex(int index)
        {
            _index = index;
        }

        public void Regenerate(int regenAmount)
        {
            if (IsAlive)
            {
                CurrentHP += regenAmount;
            }
        }

        public void Die()
        {
            IsAlive = false;
            CurrentHP = 0;
        }

        public virtual void Attack(IWarrior defender)
        {
            Random rnd = new Random();

            if (rnd.Next(2) > 0)
            {
                // valami történjen ha ismeretlen harcossal küzd - 50% eséllyel legyőzi
                defender.Die();
            }
            else
            {
                this.Die();
            }
        }
    }
}
