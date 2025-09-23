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
            LoadTestData();

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
                    case "6": ListAll(); break;
                    case "0": return;
                    default: Console.WriteLine("Ошибка ввода"); break;
                }
            }
        }




    }

}

