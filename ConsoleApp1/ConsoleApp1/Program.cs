using System;
using System.Collections.Generic;
using System.Linq;

// Інтерфейс для пошуку товарів
public interface ISearchable
{
    List<Tovar> SearchByCriteria(string criteria);
}

// Клас товару
public class Tovar
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}

// Клас користувача
public class User
{
    public string Login { get; set; }
    public string Password { get; set; }
    public List<string> PurchaseHistory { get; set; } = new List<string>();
}

// Клас замовлення
public class Order
{
    public List<Tovar> Products { get; set; }
    public List<int> Quantities { get; set; }
    public decimal TotalPrice => Products.Zip(Quantities, (p, q) => p.Price * q).Sum();
    public string Status { get; set; }
}

// Клас магазину
public class Store : ISearchable
{
    public List<Tovar> Tovars { get; set; } = new List<Tovar>();
    public List<User> Users { get; set; } = new List<User>();
    public List<Order> Orders { get; set; } = new List<Order>();

    // Додавання товару
    public void AddTovar(Tovar tovar)
    {
        Tovars.Add(tovar);
    }

    // Реєстрація користувача
    public void RegisterUser(User user)
    {
        Users.Add(user);
    }

    // Створення замовлення
    public void CreateOrder(User user, List<Tovar> products, List<int> quantities)
    {
        var order = new Order
        {
            Products = products,
            Quantities = quantities,
            Status = "Pending"
        };
        user.PurchaseHistory.Add($"Order {Orders.Count + 1}");
        Orders.Add(order);
    }

    // Реалізація інтерфейсу ISearchable
    public List<Tovar> SearchByCriteria(string criteria)
    {
        return Tovars.Where(t => t.Name.Contains(criteria) || t.Category.Contains(criteria)).ToList();
    }
}

class Program
{
    static void Main()
    {
        // Приклад використання класів і магазину
        var store = new Store();

        var tovar1 = new Tovar { Name = "Лаптоп", Price = 1200, Description = "Потужний лаптоп", Category = "Електроніка" };
        var tovar2 = new Tovar { Name = "Книга", Price = 20, Description = "Цікава книга", Category = "Книги" };

        store.AddTovar(tovar1);
        store.AddTovar(tovar2);

        var user = new User { Login = "user1", Password = "pass123" };
        store.RegisterUser(user);

        var orderProducts = new List<Tovar> { tovar1, tovar2 };
        var orderQuantities = new List<int> { 1, 2 };

        store.CreateOrder(user, orderProducts, orderQuantities);

        // Пошук товарів за критерієм
        var searchResults = store.SearchByCriteria("лаптоп");
        Console.WriteLine("Результати пошуку:");
        foreach (var result in searchResults)
        {
            Console.WriteLine($"{result.Name} - {result.Price}");
        }
    }
}
