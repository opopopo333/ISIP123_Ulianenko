using PR8;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.Title = "Marketplace";
        Console.WriteLine("Добро пожаловать в онлайн-маркетплейс!");
        Console.WriteLine("1 - Войти");
        Console.WriteLine("2 - Зарегистрироваться");
        Console.WriteLine("3 - Смотреть товары (гость)");
        Console.Write("Выбор: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Login();
                break;
            case "2":
                Register();
                break;
            case "3":
                ShowProducts(null);
                break;
            default:
                Console.WriteLine("Неверный выбор!");
                break;
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    // Регистрация нового пользователя
    static void Register()
    {
        Console.Clear();
        Console.WriteLine("===Регистрация===");

        Console.Write("Введите логин: ");
        string username = Console.ReadLine();

        //проверка, не занят ли ник
        if (Core.Context.Users.Any(u => u.Username == username))
        {
            Console.WriteLine("Пользователь с таким ником уже существует!");
            return;
        }

        Console.Write("Введите пароль: ");
        string pass1 = Console.ReadLine();
        Console.Write("Подтвердите пароль: ");
        string pass2 = Console.ReadLine();

        if (pass1 != pass2)
        {
            Console.WriteLine("Пароли не совпадают!");
            return;
        }

        var newUser = new Users
        {
            Username = username,
            Password = pass1
        };

        Core.Context.Users.Add(newUser);
        Core.Context.SaveChanges();
        Console.WriteLine("Регистрация успешна!");

        UserMenu(newUser);
    }

    //Вход
    static void Login()
    {
        Console.Clear();
        Console.WriteLine("===Вход===");
        Console.Write("Введите логин: ");
        string username = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();

        var user = Core.Context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            Console.WriteLine("Неверный логин или пароль");
            return;
        }

        Console.WriteLine($"Добро пожаловать, {user.Username}!");
        UserMenu(user);
    }

    //меню юзера
    static void UserMenu(Users user)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"===Меню пользователя: {user.Username}===");
            Console.WriteLine("1 - Смотреть товары");
            Console.WriteLine("2 - Посмотреть корзину");
            Console.WriteLine("3 - История заказов");
            Console.WriteLine("4 - Выйти");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowProducts(user);
                    break;
                case "2":
                    ShowCart(user);
                    break;
                case "3":
                    ShowOrderHistory(user);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    //просмотр товаров
    static void ShowProducts(Users user)
    {
        Console.Clear();
        Console.WriteLine("=== Список товаров ===");

        var products = Core.Context.Products.ToList();
        if (products.Count == 0)
        {
            Console.WriteLine("Нет доступных товаров.");
            return;
        }

        foreach (var p in products)
        {
            Console.WriteLine($"{p.Id}. {p.Name} - {p.Price} руб.");
        }

        if (user != null)
        {
            Console.WriteLine("\nВведите ID товара, чтобы добавить в корзину (или 0 для выхода): ");
            if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
            {
                var product = Core.Context.Products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    Core.Context.Cart.Add(new Cart
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        Quantity = 1
                    });
                    Core.Context.SaveChanges();
                    Console.WriteLine($"{product.Name} добавлен в корзину!");
                }
            }
        }

        Console.WriteLine("\nНажмите любую клавишу для возврата...");
        Console.ReadKey();
    }

    //просмотр корзины
    static void ShowCart(Users user)
    {
        Console.Clear();
        Console.WriteLine("=== Ваша корзина ===");

        var cartItems = Core.Context.Cart.Where(c => c.UserId == user.Id).ToList();
        if (cartItems.Count == 0)
        {
            Console.WriteLine("Корзина пуста.");
            Console.ReadKey();
            return;
        }

        int i = 1;
        foreach (var item in cartItems)
        {
            var prod = Core.Context.Products.First(p => p.Id == item.ProductId);
            Console.WriteLine($"{i}. {prod.Name} - {prod.Price} руб. (x{item.Quantity})");
            i++;
        }

        Console.WriteLine("\n1 - Купить один товар");
        Console.WriteLine("2 - Купить всё");
        Console.WriteLine("3 - Очистить корзину");
        Console.WriteLine("4 - Назад");
        Console.Write("Выбор: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                BuySingleItem(user, cartItems);
                break;
            case "2":
                BuyAllItems(user, cartItems);
                break;
            case "3":
                ClearCart(user);
                break;
        }
    }

    //покупка одного товара
    static void BuySingleItem(Users user, List<Cart> cartItems)
    {
        Console.Write("Введите номер товара из корзины: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= cartItems.Count)
        {
            var item = cartItems[index - 1];
            var prod = Core.Context.Products.First(p => p.Id == item.ProductId);

            Console.Write("Введите ПВЗ (например: Москва, ТЦ Европа): ");
            string pvz = Console.ReadLine();

            var order = new Orders
            {
                UserId = user.Id,
                ProductId = prod.Id,
                Quantity = item.Quantity,
                OrderDate = DateTime.Now,
                PickupPoint = pvz
            };

            Core.Context.Orders.Add(order);
            Core.Context.Cart.Remove(item);
            Core.Context.SaveChanges();

            Console.WriteLine($"Вы купили {prod.Name}!");
        }

        Console.ReadKey();
    }

    //покупка всех товаров
    static void BuyAllItems(Users user, List<Cart> cartItems)
    {
        Console.Write("Введите ПВЗ для всех товаров: ");
        string pvz = Console.ReadLine();

        foreach (var item in cartItems)
        {
            var prod = Core.Context.Products.First(p => p.Id == item.ProductId);

            var order = new Orders
            {
                UserId = user.Id,
                ProductId = prod.Id,
                Quantity = item.Quantity,
                OrderDate = DateTime.Now,
                PickupPoint = pvz
            };

            Core.Context.Orders.Add(order);
        }

        Core.Context.Cart.RemoveRange(cartItems);
        Core.Context.SaveChanges();

        Console.WriteLine("Все товары куплены!");
        Console.ReadKey();
    }

    //очистка корзины
    static void ClearCart(Users user)
    {
        var items = Core.Context.Cart.Where(c => c.UserId == user.Id).ToList();
        Core.Context.Cart.RemoveRange(items);
        Core.Context.SaveChanges();
        Console.WriteLine("Корзина очищена!");
        Console.ReadKey();
    }

    //история заказов
    static void ShowOrderHistory(Users user)
    {
        Console.Clear();
        Console.WriteLine("===История заказов===");

        var orders = Core.Context.Orders
            .Where(o => o.UserId == user.Id)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        if (orders.Count == 0)
        {
            Console.WriteLine("У вас пока нет заказов.");
        }
        else
        {
            foreach (var o in orders)
            {
                var prod = Core.Context.Products.First(p => p.Id == o.ProductId);
                Console.WriteLine($"{prod.Name} x{o.Quantity} — {o.OrderDate} — ПВЗ: {o.PickupPoint}");
            }
        }

        Console.WriteLine("\n1 - От старых к новым");
        Console.WriteLine("2 - От новых к старым");
        Console.WriteLine("3 - Назад");
        Console.Write("Выбор: ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            var sorted = orders.OrderBy(o => o.OrderDate).ToList();
            Console.Clear();
            Console.WriteLine("=== От старых к новым ===");
            foreach (var o in sorted)
            {
                var prod = Core.Context.Products.First(p => p.Id == o.ProductId);
                Console.WriteLine($"{prod.Name} — {o.OrderDate}");
            }
        }

        Console.WriteLine("\nНажмите любую клавишу для возврата...");
        Console.ReadKey();
    }
}