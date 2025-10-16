using System;
using System.Numerics;

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
}