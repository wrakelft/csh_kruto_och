using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
  private static List<Contact> contacts = new List<Contact>();
  private static int nextId = 1;

    static void Main()
  {
    contacts = FileService.LoadContacts();
    nextId = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;

    Console.WriteLine("Enter the number of action and press [Enter]. Then follow instructions.");

    while (true)
    {
      ShowMenu();
      string choice = Console.ReadLine() ?? "";

      switch (choice)
      {
        case "1":
          ViewAllContacts();
          break;

        case "2":
          SearchContacts();
          break;

        case "3":
          AddNewContact();
          break;

        case "4":
          FileService.SaveContacts(contacts);
          Console.WriteLine("Data saved to contacts.json. Exiting...");
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

    static void ViewAllContacts()
    {
        if (!contacts.Any())
        {
            Console.WriteLine("No contacts found.");
            return;
        }

        Console.WriteLine($"\nContacts ({contacts.Count}):");
        Console.WriteLine(new string('-', 50));

        int i = 1;
        foreach (var c in contacts)
        {
            Console.WriteLine($"#{i}  Name: {c.Name}, Surname: {c.Surname}, Phone: {c.Phone}, Email: {c.Email}");
            i++;
        }
    }

    static void AddNewContact()
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
            Id = nextId++,
            Name = name.Trim(),
            Surname = surname.Trim(),
            Phone = phone.Trim(),
            Email = email.Trim()
        };

        contacts.Add(contact);
    FileService.SaveContacts(contacts);
        Console.WriteLine("Contact created.");
    }

    static void SearchContacts()
    {
        if (!contacts.Any())
        {
            Console.WriteLine("No contacts to search.");
            return;
        }

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

        IEnumerable<Contact> results = option switch
        {
            "1" => contacts.Where(c => c.Name.ToLower().Contains(query)),
            "2" => contacts.Where(c => c.Surname.ToLower().Contains(query)),
            "3" => contacts.Where(c => (c.Name + " " + c.Surname).ToLower().Contains(query)),
            "4" => contacts.Where(c => c.Phone.ToLower().Contains(query)),
            "5" => contacts.Where(c => c.Email.ToLower().Contains(query)),
            "6" => contacts.Where(c =>
                        c.Name.ToLower().Contains(query) ||
                        c.Surname.ToLower().Contains(query) ||
                        c.Phone.ToLower().Contains(query) ||
                        c.Email.ToLower().Contains(query)),
            _ => Enumerable.Empty<Contact>()
        };

        var list = results.ToList();

        Console.WriteLine($"Results ({list.Count}):");

        int i = 1;
        foreach (var c in list)
        {
            Console.WriteLine($"#{i}  Name: {c.Name}, Surname: {c.Surname}, Phone: {c.Phone}, Email: {c.Email}");
            i++;
        }
    }
}
