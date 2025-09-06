using Microsoft.EntityFrameworkCore;

public class ContactsContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("contacts"); // таблица в нижнем регистре
            entity.HasKey(e => e.Id).HasName("id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Surname).HasColumnName("surname");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Подставь свои данные подключения
        optionsBuilder.UseNpgsql("Host=localhost;Database=contactsdb;Username=postgres;Password=1111");
    }
}