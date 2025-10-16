using System;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    public static Random rnd = new Random();
    static int turnCount = 0;

    static void Main()
    {
        Player player = new Player();
        Console.WriteLine("Добро пожаловать в мини-рогалик!");

        while (player.HP > 0)
        {
            turnCount++;
            Console.WriteLine($"\n--- Ход {turnCount} ---");

            // Каждые 10 ходов босс
            if (turnCount % 10 == 0)
            {
                Enemy boss = Enemy.GenerateBoss();
                Console.WriteLine($"Вас атакует Босс: {boss.Name}!");
                Battle(player, boss);
            }
            else
            {
                if (rnd.Next(2) == 0) // сундук или враг
                {
                    Chest(player);
                }
                else
                {
                    Enemy enemy = Enemy.GenerateRandomEnemy();
                    Console.WriteLine($"Вас атакует {enemy.Name}!");
                    Battle(player, enemy);
                }
            }
        }

        Console.WriteLine("Игра окончена! Вы погибли.");
    }
    static void Battle(Player player, Enemy enemy)
    {
        bool playerFrozen = false;

        while (player.HP > 0 && enemy.HP > 0)
        {
            if (!playerFrozen)
            {
                Console.WriteLine($"\nВаш ход! HP: {player.HP}, Враг HP: {enemy.HP}");
                Console.WriteLine("1. Атаковать  2. Защищаться");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    int damage = Math.Max(player.Weapon - enemy.Defense, 0);
                    enemy.HP -= damage;
                    Console.WriteLine($"Вы нанесли {damage} урона!");
                }
                else
                {
                    player.Defending = true;
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

            if (player.Defending)
            {
                if (rnd.Next(100) < 40)
                {
                    Console.WriteLine("Вы полностью уклонились от атаки!");
                    blocked = true;
                }
                else
                {
                    double blockPercent = rnd.Next(70, 101) / 100.0;
                    enemyDamage = (int)(enemyDamage * (1 - blockPercent));
                    Console.WriteLine($"Вы блокировали {blockPercent * 100}% урона.");
                }
                player.Defending = false;
            }

            if (!blocked)
            {
                player.HP -= enemyDamage;
                Console.WriteLine($"{enemy.Name} нанес вам {enemyDamage} урона!");
            }

            // Проверка особенностей врага
            if (enemy is Goblin g && rnd.Next(100) < g.CritChance)
            {
                int critDamage = g.Attack;
                player.HP -= critDamage;
                Console.WriteLine($"Критический удар Гоблина! Вы получили {critDamage} урона!");
            }
            else if (enemy is Mage m && rnd.Next(100) < m.FreezeChance)
            {
                playerFrozen = true;
                Console.WriteLine("Маг заморозил вас! Вы пропустите следующий ход!");
            }
        }

        if (player.HP > 0)
            Console.WriteLine($"Вы победили {enemy.Name}!");
    }

    static void Chest(Player player)
    {
        Console.WriteLine("Вы нашли сундук!");
        int itemType = rnd.Next(3);

        switch (itemType)
        {
            case 0: // зелье
                player.HP = player.MaxHP;
                Console.WriteLine("Вы нашли лечебное зелье! Полное восстановление здоровья.");
                break;
            case 1: // оружие
                int newWeapon = rnd.Next(5, 16);
                Console.WriteLine($"Вы нашли оружие с уроном {newWeapon}. Ваше текущее оружие {player.Weapon}.");
                Console.WriteLine("1. Взять  2. Выбросить");
                if (Console.ReadLine() == "1") player.Weapon = newWeapon;
                break;
            case 2: // доспехи
                int newArmor = rnd.Next(3, 11);
                Console.WriteLine($"Вы нашли доспехи с защитой {newArmor}. Ваша текущая защита {player.Defense}.");
                Console.WriteLine("1. Взять  2. Выбросить");
                if (Console.ReadLine() == "1") player.Defense = newArmor;
                break;
        }
    }
}

class Player
{
    public int MaxHP = 100;
    public int HP = 100;
    public int Weapon = 10;
    public int Defense = 5;
    public bool Defending = false;
}

abstract class Enemy
{
    public string Name;
    public int HP;
    public int Attack;
    public int Defense;

    public virtual int AttackValue() => Math.Max(Attack - 5, 1); // базовый расчет урона

    public static Enemy GenerateRandomEnemy()
    {
        int type = Program.rnd.Next(3);
        return type switch
        {
            0 => new Goblin(),
            1 => new Skeleton(),
            2 => new Mage(),
            _ => new Goblin(),
        };
    }

    public static Enemy GenerateBoss()
    {
        int type = Program.rnd.Next(3);
        return type switch
        {
            0 => new GoblinBoss(),
            1 => new SkeletonBoss(),
            2 => new MageBoss(),
            _ => new GoblinBoss(),
        };
    }
}

class Goblin : Enemy
{
    public int CritChance = 20;

    public Goblin()
    {
        Name = "Гоблин";
        HP = 30;
        Attack = 10;
        Defense = 3;
    }

    public override int AttackValue()
    {
        if (Program.rnd.Next(100) < CritChance)
            return Attack * 2;
        return Attack;
    }
}
class Skeleton : Enemy
{
    public Skeleton()
    {
        Name = "Скелет";
        HP = 40;
        Attack = 8;
        Defense = 5;
    }

    public override int AttackValue() => Attack; // игнорирует защиту игрока
}
