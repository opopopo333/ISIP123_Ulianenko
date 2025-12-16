using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRoguelike
{
    public class Goblin : Enemy
    {
        public int CritChance { get; protected set; } = 20;


        public Goblin()
        {
            Name = "Гоблин";
            HP = 30;
            Attack = 10;
            Defense = 3;
        }


        public override int AttackValue()
        {
            if (RandomProvider.Next(100) < CritChance) return Attack * 2;
            return Attack;
        }
    }


    public class GoblinBoss : Goblin
    {
        public GoblinBoss()
        {
            Name = "ВВГ (Босс Гоблин)";
            HP = (int)(30 * 2.0);
            Attack = (int)(10 * 1.5);
            Defense = (int)(3 * 1.2);
            CritChance += 10;
        }
    }
}
