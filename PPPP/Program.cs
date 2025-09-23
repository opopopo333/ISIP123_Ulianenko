using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryApp
{
    enum Category { Electronics = 1, Groceries, Clothing }

    class Product
    {
        public string Code { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }
        public bool InStock => Quantity > 0;

        public Product(string code, string name, decimal price, int quantity, Category category)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Имя пустое");
            if (price < 0) throw new ArgumentException("Цена < 0");
            if (quantity < 0) throw new ArgumentException("Кол-во < 0");

            Code = code;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public override string ToString() =>
            $"{Code} | {Name} | {Category} | Цена: {Price:C} | Кол-во: {Quantity} | В наличии: {(InStock ? "Да" : "Нет")}";
    }

    class Program
    {
        static List<Product> products = new();
        static int nextId = 1;

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            while (true)
            {
                Console.WriteLine("\n1-Добавить  2-Удалить  3-Поставка  4-Продажа  5-Поиск  6-Все товары  0-Выход");
                Console.Write("Команда: ");
                string cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "1": AddProduct(); break;
                    case "2": RemoveProduct(); break;
                    case "3": SupplyProduct(); break;
                    case "4": SellProduct(); break;
                    case "5": SearchProduct(); break;
                    case "0": return;
                    default: Console.WriteLine("Ошибка ввода"); break;
                }
            }
        }

        static string GenCode() => "1" + nextId++.ToString("D3");

        static void AddProduct()
        {
            Console.Write("Название: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Ошибка: пустое имя"); return; }

            Console.Write("Цена: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price) || price < 0) { Console.WriteLine("Ошибка цены"); return; }

            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out var qty) || qty < 0) { Console.WriteLine("Ошибка количества"); return; }

            Console.Write("Категория (1-Эл,2-Продукты,3-Одежда): ");
            if (!int.TryParse(Console.ReadLine(), out var c) || c < 1 || c > 3) { Console.WriteLine("Ошибка категории"); return; }

            products.Add(new Product(GenCode(), name, price, qty, (Category)c));
            Console.WriteLine("Товар добавлен");
        }

        static void RemoveProduct()
        {
            Console.Write("Код: ");
            string code = Console.ReadLine();
            var p = products.FirstOrDefault(x => x.Code == code);
            if (p == null) Console.WriteLine("Не найден");
            else { products.Remove(p); Console.WriteLine("Удалён"); }
        }

        static void SupplyProduct()
        {
            Console.Write("Код: ");
            string code = Console.ReadLine();
            Console.Write("Сколько добавить: ");
            if (!int.TryParse(Console.ReadLine(), out var amt) || amt <= 0) { Console.WriteLine("Ошибка количества"); return; }

            var p = products.FirstOrDefault(x => x.Code == code);
            if (p == null) Console.WriteLine("Не найден");
            else { p.Quantity += amt; Console.WriteLine("Поставка выполнена"); }
        }

        static void SearchProduct()
        {
            Console.Write("Поиск по (1-код,2-имя,3-категория): ");
            string mode = Console.ReadLine();

            if (mode == "1")
            {
                Console.Write("Код: ");
                string code = Console.ReadLine();
                var p1 = products.FirstOrDefault(x => x.Code == code);
                if (p1 == null) Console.WriteLine("Не найден");
                else Console.WriteLine(p1);
            }
            else if (mode == "2")
            {
                Console.Write("Имя содержит: ");
                string q = Console.ReadLine()?.ToLower() ?? "";
                var res = products.Where(x => x.Name.ToLower().Contains(q));
                foreach (var p in res) Console.WriteLine(p);
            }
            else if (mode == "3")
            {
                Console.Write("Категория (1-Эл,2-Продукты,3-Одежда): ");
                if (!int.TryParse(Console.ReadLine(), out var c) || c < 1 || c > 3) { Console.WriteLine("Ошибка"); return; }
                foreach (var p in products.Where(x => x.Category == (Category)c)) Console.WriteLine(p);
            }



        }
    }


