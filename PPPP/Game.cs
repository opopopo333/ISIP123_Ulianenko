using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;



namespace MiniRoguelike
{
    // Отвечает за цикл игры и логику сражений (SRP: Game управляет процессом игры)
    public class Game
    {
        private readonly Player _player = new Player();
        private readonly EnemyFactory _factory = new EnemyFactory();
        private int _turnCount = 0;


        public void Run()
        {
            Console.WriteLine("Добро пожаловать в мини-рогалик!");


            while (_player.HP > 0)
            {
                _turnCount++;
                Console.WriteLine($"\n--- Ход {_turnCount} ---");


                if (_turnCount % 10 == 0)
                {
                    var boss = _factory.CreateRandomEnemy(isBoss: true);
                    Console.WriteLine($"Вас атакует Босс: {boss.Name}!");
                    Battle(boss);
                }
                else
                {
                    if (RandomProvider.Next(2) == 0)
                        Chest();
                    else
                    {
                        var enemy = _factory.CreateRandomEnemy();
                        Console.WriteLine($"Вас атакует {enemy.Name}!");
                        Battle(enemy);
                    }
                }
            }


            Console.WriteLine("Игра окончена! Вы погибли.");
        }


        private void Battle(Enemy enemy)
        {
            bool playerFrozen = false;


            while (_player.HP > 0 && enemy.HP > 0)
            {
                if (!playerFrozen)
                {
                    Console.WriteLine($"\nВаш ход! HP: {_player.HP}, Враг HP: {enemy.HP}");
                    Console.WriteLine("1. Атаковать 2. Защищаться");
                    var choice = Console.ReadLine();


                    if (choice == "1")
                    {
                        int dealt = enemy.ReceiveDamage(_player.Weapon);
                        Console.WriteLine($"Вы нанесли {dealt} урона!");
                    }
                    else
                    {
                        _player.Defending = true;
                        Console.WriteLine("Вы готовитесь к защите!");
                    }
                }
                else
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    playerFrozen = false;
                }


                if (enemy.HP <= 0) break;


                // Ход врага
                int enemyDamage = enemy.AttackValue();
                bool blocked = false;


                if (_player.Defending)
                {
                    if (RandomProvider.Next(100) < 40)
                    {
                        Console.WriteLine("Вы полностью уклонились от атаки!");
                        blocked = true;
                    }
                    else
                    {
                        double blockPercent = RandomProvider.Next(70, 101) / 100.0;
                        enemyDamage = (int)(enemyDamage * (1 - blockPercent));
                        Console.WriteLine($"Вы блокировали {blockPercent * 100}% урона.");
                    }


                    _player.Defending = false;
                }


                if (!blocked)
                {
                    // Применяем броню игрока
                    int finalDamage = enemy.IgnorePlayerDefense
                    ? enemyDamage
                    : Math.Max(enemyDamage - _player.Defense, 0);

                    _player.HP -= finalDamage;
                    Console.WriteLine($"{enemy.Name} нанес вам {finalDamage} урона!");
                }


                // Особенности врага
                if (enemy is Goblin g && RandomProvider.Next(100) < g.CritChance)
                {
                    int critDamage = g.Attack;
                    int dmgAfterDef = Math.Max(critDamage - _player.Defense, 0);
                    _player.HP -= dmgAfterDef;
                    Console.WriteLine($"Критический удар Гоблина! Вы получили {dmgAfterDef} урона!");
                }
                else if (enemy is Mage m && RandomProvider.Next(100) < m.FreezeChance)
                {
                    playerFrozen = true;
                    Console.WriteLine("Маг заморозил вас! Вы пропустите следующий ход!");
                }
            }


            if (_player.HP > 0)
                Console.WriteLine($"Вы победили {enemy.Name}!");
        }


        private void Chest()
        {
            Console.WriteLine("Вы нашли сундук!");
            int itemType = RandomProvider.Next(3);


            switch (itemType)
            {
                case 0:
                    _player.HP = _player.MaxHP;
                    Console.WriteLine("Вы нашли лечебное зелье! Полное восстановление здоровья.");
                    break;
                case 1:
                    int newWeapon = RandomProvider.Next(5, 16);
                    Console.WriteLine($"Вы нашли оружие с уроном {newWeapon}. Ваше текущее оружие {_player.Weapon}.");
                    Console.WriteLine("1. Взять 2. Выбросить");
                    if (Console.ReadLine() == "1") _player.Weapon = newWeapon;
                    break;
                case 2:
                    int newArmor = RandomProvider.Next(3, 11);
                    Console.WriteLine($"Вы нашли доспехи с защитой {newArmor}. Ваша текущая защита {_player.Defense}.");
                    Console.WriteLine("1. Взять 2. Выбросить");
                    if (Console.ReadLine() == "1") _player.Defense = newArmor;
                    break;
            }
        }
    }
}