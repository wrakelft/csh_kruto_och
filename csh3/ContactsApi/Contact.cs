using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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