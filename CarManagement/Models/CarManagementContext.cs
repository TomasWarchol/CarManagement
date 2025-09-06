using Microsoft.EntityFrameworkCore;

namespace CarManagement.Models
{
    public class CarManagementContext : DbContext
    {
        public CarManagementContext(DbContextOptions<CarManagementContext> options) : base(options) { }

        // Zestawy danych dla systemu floty
        public DbSet<Pojazd> Pojazdy { get; set; }
        public DbSet<Kierowca> Kierowcy { get; set; }
        public DbSet<PrzypisaniePojazdu> Przypisania { get; set; }
        public DbSet<PolisaUbezpieczeniowa> Polisy { get; set; }
        public DbSet<SerwisPojazdu> Serwisy { get; set; }
        public DbSet<KartaPaliwowa> KartyPaliwowe { get; set; }
        public DbSet<Tankowanie> Tankowania { get; set; }
        public DbSet<KategoriaPojazdu> KategoriePojazdow { get; set; }

        // Zestawy danych dla starszego systemu HR (jeśli nadal potrzebne)
        public DbSet<PracownikAdministracyjny> Uzytkownicy { get; set; }
        public DbSet<Kategoria> Kategoria { get; set; }
        public DbSet<Uprawnienia> Uprawnienia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Tutaj można dodać konfigurację modelu, np. klucze złożone, relacje itp.
        }
    }
}