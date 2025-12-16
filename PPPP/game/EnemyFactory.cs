using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRoguelike
{
    // Простая фабрика — отвечает только за создание экземпляров врагов
    public class EnemyFactory
    {
        public Enemy CreateRandomEnemy(bool isBoss = false)
        {
            int type = RandomProvider.Next(4); // теперь 4 типа: Goblin, Skeleton, Mage, Slime


            if (isBoss)
            {
                return type switch
                {
                    0 => new GoblinBoss(),
                    1 => new SkeletonBoss(),
                    2 => new MageBoss(),
                    // для слизня тоже можно сделать "босс-слизень" или просто обычный слизень в качестве босса;
                    3 => new Slime() as Enemy,
                    _ => new GoblinBoss(),
                };
            }


            return type switch
            {
                0 => new Goblin(),
                1 => new Skeleton(),
                2 => new Mage(),
                3 => new Slime(),
                _ => new Goblin(),
            };
        }
    }
}
