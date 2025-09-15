using Microsoft.EntityFrameworkCore;

public class ContactsContext : DbContext
{
    public ContactsContext(DbContextOptions<ContactsContext> options) : base(options) { }
    
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("contacts"); // таблица в нижнем регистре
            entity.HasKey(e => e.Id).HasName("id");
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Surname).HasColumnName("surname");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email");
        });
    }
}