using Microsoft.Extensions.DependencyInjection;
using Warriors.Models;

namespace Warriors.Modules
{
    internal abstract class ArenaBase : ICombatArena
    {
        protected readonly IServiceProvider _serviceProvider;
        protected List<IWarrior> _warriors = new List<IWarrior>();

        public ArenaBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public abstract string GetName();

        public abstract void ListRules();

        public abstract bool IsCombatReady();

        public abstract void ListResults();

        public abstract void DoCombatRound();

        public virtual void DoArenaEffect()
        {
        }

        public virtual List<string> GetAvailableWarriorClassNames()
        {
            return _serviceProvider.GetServices<IWarrior>().Select(x => x.ClassName).ToList();
        }

        public virtual void GenerateWarriors(int numberOfWarriors)
        {
            _warriors = new List<IWarrior>();

            Random rnd = new Random();
            int classCount = _serviceProvider.GetServices<IWarrior>().Count();

            for (int i = 0; i < numberOfWarriors; i++)
            {
                IWarrior warrior = _serviceProvider.GetServices<IWarrior>().ToArray()[rnd.Next(classCount)];
                warrior.SetIndex(_warriors.Count(x => x.ClassName == warrior.ClassName) + 1);
                _warriors.Add(warrior);
            }

            _warriors = _warriors.OrderBy(x => x.Name).ToList();
        }

        public virtual void ListWarriors()
        {
            if (_warriors == null || _warriors.Count == 0)
            {
                Console.WriteLine("Nincsenek elérhetö harcosok.");
            }
            else if (_warriors.Count(x => x.IsAlive) > 0)
            {
                Console.WriteLine($"Csatában lévö harcosok: {string.Join(", ", _warriors.Where(x => x.IsAlive).Select(x => x.NameWithHP))}");
            }
            else
            {
                Console.WriteLine("Minden harcos meghalt.");
            }
        }
    }
}
