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


}
