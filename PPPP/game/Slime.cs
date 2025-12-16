using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRoguelike
{
    // Новый монстр: Слизень — уменьшает входящий в него урон на 2 единицы
    public class Slime : Enemy
    {


        public Slime()
        {
            Name = "Слизень";
            HP = 20;
            Attack = 8;
            Defense = 1;
        }


        public override int ReceiveDamage(int incoming)
        {
            // сначала уменьшаем входящий урон слизня
            int reduced = Math.Max(incoming - 2, 0);
            // затем учитываем его защиту
            int actual = Math.Max(reduced - Defense, 0);
            HP -= actual;
            return actual;
        }
    }
}