using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp
{
    public enum Genre
    {
        Fantasy = 1,
        ScienceFiction,
        Detective,
        Romance,
        History
    }

    public class Book
    {
        private static int _idCounter = 1;
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public Genre Genre { get; }
        public int Year { get; }
        public decimal Price { get; }

        public Book(string title, string author, Genre genre, int year, decimal price)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название книги не может быть пустым.");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Автор не может быть пустым.");
            if (year <= 0)
                throw new ArgumentException("Год издания должен быть положительным числом.");
            if (price <= 0)
                throw new ArgumentException("Цена должна быть положительным числом.");

            Id = _idCounter++;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }
        public override string ToString()
        {
            return $"ID: {Id} | Название: {Title} | Автор: {Author} | Жанр: {Genre} | Год: {Year} | Цена: {Price} руб.";
        }
    }

    class Program
    {
        static List<Book> books = new List<Book>();

        static void Main()
        {

            while (true)
            {
                Console.WriteLine("\n=== Меню ===");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу по ID");
                Console.WriteLine("3. Найти книги");
                Console.WriteLine("4. Отсортировать книги");
                Console.WriteLine("5. Самая дорогая и самая дешёвая книга");
                Console.WriteLine("6. Группировка по авторам");
                Console.WriteLine("7. Показать все книги");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": AddBook(); break;
                    case "2": RemoveBook(); break;
                    case "3": SearchBooks(); break;
                    case "4": SortBooks(); break;
                    case "5": ShowPriceExtremes(); break;
                    case "6": GroupByAuthor(); break;
                    case "7": ShowAllBooks(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод, попробуйте снова."); break;
                }
            }
        }
        static void AddBook()
        {
            try
            {
                Console.Write("Введите название: ");
                string title = Console.ReadLine();

                Console.Write("Введите автора: ");
                string author = Console.ReadLine();

                Console.WriteLine("Выберите жанр:");
                foreach (var g in Enum.GetValues(typeof(Genre)))
                    Console.WriteLine($"{(int)g}. {g}");
                Genre genre = (Genre)Convert.ToInt32(Console.ReadLine());

                Console.Write("Введите год издания: ");
                int year = int.Parse(Console.ReadLine());

                Console.Write("Введите цену: ");
                decimal price = decimal.Parse(Console.ReadLine());

                var book = new Book(title, author, genre, year, price);
                books.Add(book);
                Console.WriteLine("Книга успешно добавлена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void RemoveBook()
        {
            Console.Write("Введите ID книги для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var book = books.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    books.Remove(book);
                    Console.WriteLine("Книга удалена.");
                }
                else Console.WriteLine("Книга не найдена.");
            }
            else Console.WriteLine("Некорректный ввод.");
        }



    }
}