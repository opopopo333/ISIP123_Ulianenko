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

    
}

