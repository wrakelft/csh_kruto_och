using System.Diagnostics.Contracts;
using System.Text.Json;

public static class FileService
{
  private const string path = "contacts.json";

  public static List<Contact> LoadContacts()
  {
    if (!File.Exists(path))
      return new List<Contact>();

    try
    {
      var json = File.ReadAllText(path);
      var contacts = JsonSerializer.Deserialize<List<Contact>>(json);
      return contacts ?? new List<Contact>();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error loading contacts: {ex.Message}");

      return new List<Contact>();
    }
  }

  public static void SaveContacts(List<Contact> contacts)
  {
    try
    {
      var options = new JsonSerializerOptions { WriteIndented = true };
      var json = JsonSerializer.Serialize(contacts, options);
      File.WriteAllText(path, json);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error saving contacts: {ex.Message}");
    }
  }
}
