using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRoguelike
{
    public class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            HP = 40;
            Attack = 8;
            Defense = 5;
        }

        public override bool IgnorePlayerDefense => true;


        // Скелет наносит урон без модификации (в примере игнорировал защиту игрока), но мы вернём базовую логику
    }


    public class SkeletonBoss : Skeleton
    {
        public SkeletonBoss()
        {
            Name = "Ковальский (Босс Скелет)";
            HP = (int)(40 * 2.5);
            Attack = (int)(8 * 1.3);
            Defense = (int)(5 * 1.4);
        }
    }
}