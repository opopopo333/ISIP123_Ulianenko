using System;
using System.Linq;
using AutoServiceGame;

namespace AutoServiceGame
{
    

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Добро пожаловать в игру 'Автосервис' ===");

            decimal balance = 10000m;
            int clientCounter = 0;
            Random random = new Random();

            while (true)
            {
                Console.WriteLine($"\nТекущий баланс: {balance} руб.");
                Console.WriteLine("1. Обслужить клиента");
                Console.WriteLine("2. Закупить детали");
                Console.WriteLine("3. Выйти");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    clientCounter++;
                    ServeClient(ref balance, random);
                }
                else if (choice == "2")
                {
                    BuyParts(ref balance);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Выход из игры...");
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод.");
                }
            }
        }

        static void ServeClient(ref decimal balance, Random random)
        {
            var parts = Core.Context.Parts.ToList();
            if (!parts.Any())
            {
                Console.WriteLine("❌ Нет деталей на складе! Закупите их сначала.");
                return;
            }

            var part = parts[random.Next(parts.Count)];
            Console.WriteLine($"\nКлиент приехал с проблемой: требуется замена детали \"{part.Name}\".");

            decimal workPrice = part.Price * 0.5m ?? 0;
            decimal totalCost = part.Price + workPrice ?? 0;

            Console.WriteLine($"Стоимость ремонта: {totalCost} руб.");

            if (part.Quantity > 0)
            {
                part.Quantity--;
                balance += totalCost;
                Console.WriteLine($"✅ Ремонт успешно выполнен! Заработано: {totalCost} руб.");
            }
            else
            {
                Console.WriteLine("❌ Детали нет на складе! Вы потеряли 500 руб. за отказ.");
                balance -= 500m;
            }

            Core.Context.SaveChanges();
        }

        static void BuyParts(ref decimal balance)
        {
            var parts = Core.Context.Parts.ToList();
            Console.WriteLine("\nДоступные детали для закупки:");
            foreach (var p in parts)
            {
                Console.WriteLine($"{p.Id}. {p.Name} — {p.Price} руб. (в наличии: {p.Quantity})");
            }

            Console.Write("Введите ID детали для покупки: ");
            if (int.TryParse(Console.ReadLine(), out int partId))
            {
                var part = Core.Context.Parts.FirstOrDefault(p => p.Id == partId);
                if (part == null)
                {
                    Console.WriteLine("❌ Деталь не найдена.");
                    return;
                }

                Console.Write("Введите количество: ");
                if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
                {
                    decimal totalPrice = part.Price * count ?? 0;
                    if (balance >= totalPrice)
                    {
                        part.Quantity += count;
                        balance -= totalPrice;
                        Core.Context.SaveChanges();
                        Console.WriteLine($"✅ Куплено {count} шт. детали {part.Name} за {totalPrice} руб.");
                    }
                    else
                    {
                        Console.WriteLine("❌ Недостаточно средств!");
                    }
                }
            }
        }
    }
}