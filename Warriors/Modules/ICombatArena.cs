using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors.Modules
{
    public interface ICombatArena
    {
        string GetName();
        void ListRules();
        bool IsCombatReady();
        void ListResults();
        List<string> GetAvailableWarriorClassNames();
        void GenerateWarriors(int numberOfWarriors);
        void ListWarriors();
        void DoCombatRound();
        void DoArenaEffect();
    }
}
