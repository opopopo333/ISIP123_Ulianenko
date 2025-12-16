using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRoguelike
{
    // Базовый класс врага — отвечает только за свойства и поведение, связанное с сущностью врага
    public abstract class Enemy
    {
        public string Name;
        public int HP;
        public int Attack;
        public int Defense;


        // Возвращает урон, который враг наносит в своем ходу
        public virtual int AttackValue() => Math.Max(Attack - 5, 1);
        public virtual bool IgnorePlayerDefense => false;


        // Метод получения урона — позволяет подклассам модифицировать входящий урон (например, слизень)
        // Возвращает реальный нанесённый урон (после модификации и применения к HP)
        public virtual int ReceiveDamage(int incoming)
        {
            int actual = Math.Max(incoming - Defense, 0);
            HP -= actual;
            return actual;
        }
    }
}
