using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Modules
{
    internal class CombatManager : ICombatManager
    {
        private readonly ICombatArena _arena;

        public CombatManager(ICombatArena arena)
        {
            _arena = arena;
        }
    }
}
