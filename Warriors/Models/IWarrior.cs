using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Models
{
    public interface IWarrior
    {
        string Name { get; }

        string ClassName { get; }

        int BaseHP { get; }

        int CurrentHP { get; set; }

        bool IsAlive { get; }

        string NameWithHP { get; }

        void SetIndex(int index);

        void Regenerate();

        void Die();
    }
}
