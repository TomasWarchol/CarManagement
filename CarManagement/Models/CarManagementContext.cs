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

        public DbSet<PracownikAdministracyjny> Uzytkownicy { get; set; }
        public DbSet<Kategoria> Kategoria { get; set; }
        public DbSet<Uprawnienia> Uprawnienia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PrzypisaniePojazdu: relacja wiele-do-jednego z Pojazd i Kierowca
            modelBuilder.Entity<PrzypisaniePojazdu>()
                .HasOne(p => p.Pojazd)
                .WithMany(poj => poj.Przypisania)
                .HasForeignKey(p => p.PojazdId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrzypisaniePojazdu>()
                .HasOne(p => p.Kierowca)
                .WithMany(k => k.Przypisania)
                .HasForeignKey(p => p.KierowcaId)
                .OnDelete(DeleteBehavior.Restrict);

            // SerwisPojazdu: relacja wiele-do-jednego z Pojazd
            modelBuilder.Entity<SerwisPojazdu>()
                .HasOne(s => s.Pojazd)
                .WithMany(p => p.Serwisy)
                .HasForeignKey(s => s.PojazdId)
                .OnDelete(DeleteBehavior.Cascade);

            // PolisaUbezpieczeniowa: relacja wiele-do-jednego z Pojazd
            modelBuilder.Entity<PolisaUbezpieczeniowa>()
                .HasOne(p => p.Pojazd)
                .WithMany(poj => poj.Polisy)
                .HasForeignKey(p => p.PojazdId)
                .OnDelete(DeleteBehavior.Cascade);

            // KartaPaliwowa: relacja wiele-do-jednego z Kierowca
            modelBuilder.Entity<KartaPaliwowa>()
                .HasOne(k => k.Kierowca)
                .WithMany(kier => kier.KartyPaliwowe)
                .HasForeignKey(k => k.KierowcaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Pojazd: relacja wiele-do-jednego z KategoriaPojazdu
            modelBuilder.Entity<Pojazd>()
                .HasOne(p => p.Kategoria)
                .WithMany(k => k.Pojazdy)
                .HasForeignKey(p => p.KategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tankowanie: relacje opcjonalne
            modelBuilder.Entity<Tankowanie>()
                .HasOne<Tankowanie>()
                .WithOne()
                .HasForeignKey<Tankowanie>(t => t.PojazdId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unikalność numeru rejestracyjnego pojazdu
            modelBuilder.Entity<Pojazd>()
                .HasIndex(p => p.NumerRejestracyjny)
                .IsUnique();

            // Unikalność e-maila kierowcy
            modelBuilder.Entity<Kierowca>()
                .HasIndex(k => k.Email)
                .IsUnique();
        }
    }
}