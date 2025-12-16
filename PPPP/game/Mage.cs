using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRoguelike
{
    public class Mage : Enemy
    {
        public int FreezeChance { get; protected set; } = 20;


        public Mage()
        {
            Name = "Маг";
            HP = 25;
            Attack = 12;
            Defense = 2;
        }
    }


    public class MageBoss : Mage
    {
        public MageBoss()
        {
            Name = "Архимаг C++ (Босс Маг)";
            HP = (int)(25 * 1.8);
            Attack = (int)(12 * 1.6);
            Defense = (int)(2 * 1.1);
            FreezeChance += 10;
        }
    }
}