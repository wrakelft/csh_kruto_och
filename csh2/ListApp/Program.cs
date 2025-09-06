using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main()
    {
        using var db = new ContactsContext();
        await db.Database.EnsureCreatedAsync(); // создаст таблицу, если её нет

        Console.WriteLine("Enter the number of action and press [Enter]. Then follow instructions.");

        while (true)
        {
            ShowMenu();
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    await ViewAllContacts(db);
                    break;

                case "2":
                    await SearchContacts(db);
                    break;

                case "3":
                    await AddNewContact(db);
                    break;

                case "4":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. View all contacts");
        Console.WriteLine("2. Search");
        Console.WriteLine("3. New contact");
        Console.WriteLine("4. Exit");
        Console.Write("> ");
    }

    static async Task ViewAllContacts(ContactsContext db)
    {
        var contacts = await db.Contacts.ToListAsync();

        if (!contacts.Any())
        {
            Console.WriteLine("No contacts found.");
            return;
        }

        int i = 1;
        foreach (var c in contacts)
        {
            Console.WriteLine($"#{i}  Name: {c.Name}, Surname: {c.Surname}, Phone: {c.Phone}, Email: {c.Email}");
            i++;
        }
    }

    static async Task AddNewContact(ContactsContext db)
    {
        Console.WriteLine("New contact");

        string name;
        do
        {
            Console.Write("Name: ");
            name = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(name))
                Console.WriteLine("Name cannot be empty.");
        } while (string.IsNullOrWhiteSpace(name));

        string surname;
        do
        {
            Console.Write("Surname: ");
            surname = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(surname))
                Console.WriteLine("Surname cannot be empty.");
        } while (string.IsNullOrWhiteSpace(surname));

        string phone;
        do
        {
            Console.Write("Phone: ");
            phone = Console.ReadLine() ?? "";
            if (!Regex.IsMatch(phone, @"^\+?\d{7,15}$"))
                Console.WriteLine("Invalid phone number. Only digits allowed, may start with '+'. Length 7-15 digits.");
        } while (!Regex.IsMatch(phone, @"^\+?\d{7,15}$"));

        string email;
        do
        {
            Console.Write("E-mail: ");
            email = (Console.ReadLine() ?? "").Trim().ToLower();
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                Console.WriteLine("Invalid email format.");
        } while (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"));

        var contact = new Contact
        {
            Name = name,
            Surname = surname,
            Phone = phone,
            Email = email
        };

        db.Contacts.Add(contact);
        await db.SaveChangesAsync();
        Console.WriteLine("Contact created.");
    }

    static async Task SearchContacts(ContactsContext db)
    {
        string option;
        do
        {
            Console.WriteLine("Search by:");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Surname");
            Console.WriteLine("3. Name and Surname");
            Console.WriteLine("4. Phone");
            Console.WriteLine("5. E-mail");
            Console.WriteLine("6. All fields");
            Console.Write("> ");

            option = Console.ReadLine() ?? "";
            if (!new[] { "1", "2", "3", "4", "5", "6" }.Contains(option))
                Console.WriteLine("Please enter a number from 1 to 6.");
        } while (!new[] { "1", "2", "3", "4", "5", "6" }.Contains(option));

        Console.Write("Request: ");
        string query = (Console.ReadLine() ?? "").ToLower();

        IQueryable<Contact> results = option switch
        {
            "1" => db.Contacts.Where(c => c.Name.ToLower().Contains(query)),
            "2" => db.Contacts.Where(c => c.Surname.ToLower().Contains(query)),
            "3" => db.Contacts.Where(c => (c.Name + " " + c.Surname).ToLower().Contains(query)),
            "4" => db.Contacts.Where(c => c.Phone.ToLower().Contains(query)),
            "5" => db.Contacts.Where(c => c.Email.ToLower().Contains(query)),
            "6" => db.Contacts.Where(c =>
                        c.Name.ToLower().Contains(query) ||
                        c.Surname.ToLower().Contains(query) ||
                        c.Phone.ToLower().Contains(query) ||
                        c.Email.ToLower().Contains(query)),
            _ => db.Contacts.Where(c => false)
        };

        var list = await results.ToListAsync();

        Console.WriteLine($"Results ({list.Count}):");

        int i = 1;
        foreach (var c in list)
        {
            Console.WriteLine($"#{i}  Name: {c.Name}, Surname: {c.Surname}, Phone: {c.Phone}, Email: {c.Email}");
            i++;
        }
    }
}