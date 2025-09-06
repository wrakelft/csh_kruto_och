using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
  private readonly ContactsContext _context;

  public ContactsController(ContactsContext context)
  {
    _context = context;
  }

  // GET: api/contacts
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
  {
    return await _context.Contacts.ToListAsync();
  }

  // GET: api/contacts/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Contact>> GetContact(int id)
  {
    var contact = await _context.Contacts.FindAsync(id);

    if (contact == null)
    {
      return NotFound();
    }

    return contact;
  }

  // POST: api/contacts
  [HttpPost]
  public async Task<ActionResult<Contact>> PostContact(Contact contact)
  {
    if (string.IsNullOrWhiteSpace(contact.Name))
      return BadRequest("Name is required");

    if (string.IsNullOrWhiteSpace(contact.Surname))
      return BadRequest("Surname is required");

    if (!Regex.IsMatch(contact.Phone, @"^\+?\d{7,15}$"))
      return BadRequest("Invalid phone number format");

    if (!Regex.IsMatch(contact.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
      return BadRequest("Invalid email format");

    _context.Contacts.Add(contact);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
  }

  // GET: api/contacts/search
  [HttpGet("search")]
  public async Task<ActionResult<IEnumerable<Contact>>> SearchContacts(
    [FromQuery] string? name = null,
    [FromQuery] string? surname = null,
    [FromQuery] string? phone = null,
    [FromQuery] string? email = null,
    [FromQuery] string? query = null)
  {
    IQueryable<Contact> results = _context.Contacts;

    if (!string.IsNullOrEmpty(query))
    {
      results = results.Where(c =>
      c.Name.ToLower().Contains(query.ToLower()) ||
      c.Surname.ToLower().Contains(query.ToLower()) ||
      c.Phone.Contains(query) ||
      c.Email.ToLower().Contains(query.ToLower()));
    }
    else
    {
      if (!string.IsNullOrEmpty(name))
        results = results.Where(c => c.Name.ToLower().Contains(name.ToLower()));

      if (!string.IsNullOrEmpty(surname))
        results = results.Where(c => c.Surname.ToLower().Contains(surname.ToLower()));

      if (!string.IsNullOrEmpty(phone))
        results = results.Where(c => c.Phone.Contains(phone));

      if (!string.IsNullOrEmpty(email))
        results = results.Where(c => c.Email.ToLower().Contains(email.ToLower()));
    }

    return await results.ToListAsync();
  }

  private bool ContactExists(int id)
  {
    return _context.Contacts.Any(e => e.Id == id);
  }
}
