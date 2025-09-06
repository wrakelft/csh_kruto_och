using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Contact
{
  [JsonIgnore]
  public int Id { get; set; }

  [Required]
  public string Name { get; set; } = "";

  [Required]
  public string Surname { get; set; } = "";

  [Required]
  public string Phone { get; set; } = "";

  [Required]
  public string Email { get; set; } = "";
}
