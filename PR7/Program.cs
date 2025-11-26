using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR7
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    public class Game
    {
        private decimal Balance => Core.Context.WareHouse.First().Balance;

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("=== АВТОСЕРВИС — НАЧАЛО ИГРЫ ===");
            Console.WriteLine();

            // Проверка, что база данных корректно загружена
            if (!Core.Context.Parts.Any())
            {
                Console.WriteLine("Ошибка: таблица Parts пустая.");
                return;
            }

            if (!Core.Context.WareHouse.Any())
            {
                Console.WriteLine("Ошибка: отсутствует запись WareHouse.");
                return;
            }

            MainLoop();
        }

        private void MainLoop()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Ваш баланс: {Balance} руб.");
                Console.WriteLine("1 — принять клиента");
                Console.WriteLine("2 — закупить детали");
                Console.WriteLine("3 — склад");
                Console.WriteLine("0 — выход");

                Console.Write("Выбор: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": HandleClient(); break;
                    case "2": PurchaseMenu(); break;
                    case "3": PrintWarehouse(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
            }
        }

        // --- Клиенты ---
        private void HandleClient()
        {
            Random rnd = new Random();
            var part = Core.Context.Parts.OrderBy(x => rnd.Next()).First();

            Console.WriteLine();
            Console.WriteLine($"Клиент: сломалась деталь — {part.Name}");
            Console.WriteLine($"Стоимость ремонта: {part.Price + 200} руб.");

            var warehousePart = Core.Context.WarehouseParts
                .FirstOrDefault(x => x.PartsID == part.ID);

            if (warehousePart != null && warehousePart.Count > 0)
            {
                RepairCar(part, warehousePart);
            }
            else
            {
                Console.WriteLine("Детали нет на складе.");
                Console.WriteLine("1 — отказать (-100 руб.)");
                Console.WriteLine("0 — назад");

                var ch = Console.ReadLine();
                if (ch == "1") PayFine();
            }
        }

        private void RepairCar(Parts part, WarehouseParts whPart)
        {
            whPart.Count--;
            Core.Context.WarehouseParts.Attach(whPart);
            Core.Context.SaveChanges();

            var warehouse = Core.Context.WareHouse.First();
            warehouse.Balance += part.Price + 200;
            Core.Context.SaveChanges();

            Console.WriteLine($"Ремонт выполнен! +{part.Price + 200} руб.");
        }

        private void PayFine()
        {
            var warehouse = Core.Context.WareHouse.First();
            warehouse.Balance -= 100;
            Core.Context.SaveChanges();

            Console.WriteLine("-100 руб. штраф");
        }

        // --- Закупка ---
        private void PurchaseMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== МЕНЮ ЗАКУПКИ ===");

            var parts = Core.Context.Parts.ToList();

            for (int i = 0; i < parts.Count; i++)
                Console.WriteLine($"{i + 1}. {parts[i].Name} — {parts[i].Price} руб.");

            Console.Write("Выберите деталь (0 — назад): ");
            if (!int.TryParse(Console.ReadLine(), out int choice) ||
                choice < 1 || choice > parts.Count)
                return;

            var part = parts[choice - 1];

            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                Console.WriteLine("Неверное количество.");
                return;
            }

            decimal total = count * part.Price;
            var warehouse = Core.Context.WareHouse.First();

            if (warehouse.Balance < total)
            {
                Console.WriteLine("Недостаточно средств.");
                return;
            }

            warehouse.Balance -= total;
            Core.Context.SaveChanges();

            AddToStock(part.ID, count);

            Console.WriteLine($"Закуплено {count} шт. деталь {part.Name} (-{total} руб.)");
        }

        private void AddToStock(int partId, int count)
        {
            var row = Core.Context.WarehouseParts.FirstOrDefault(x => x.PartsID == partId);

            if (row == null)
            {
                row = new WarehouseParts()
                {
                    WarehouseID = Core.Context.WareHouse.First().Id,
                    PartsID = partId,
                    Count = count
                };

                Core.Context.WarehouseParts.Add(row);
            }
            else
            {
                row.Count += count;
            }

            Core.Context.SaveChanges();
        }

        // --- Просмотр склада ---
        private void PrintWarehouse()
        {
            Console.WriteLine();
            Console.WriteLine("=== СКЛАД ===");

            var items = Core.Context.WarehouseParts.ToList();

            foreach (var i in items)
            {
                var p = Core.Context.Parts.First(x => x.ID == i.PartsID);
                Console.WriteLine($"{p.Name} — {i.Count} шт.");
            }
        }
    }
}
