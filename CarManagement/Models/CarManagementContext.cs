using Microsoft.EntityFrameworkCore;

namespace CarManagementApp.Models
{
    public class CarManagementContext : DbContext
    {
        public CarManagementContext(DbContextOptions<CarManagementContext> options) : base(options) { }

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

            // Przypisanie ↔ Pojazd/Kierowca (wiele-do-jednego)
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

            // Serwis ↔ Pojazd
            modelBuilder.Entity<SerwisPojazdu>()
                .HasOne(s => s.Pojazd)
                .WithMany(p => p.Serwisy)
                .HasForeignKey(s => s.PojazdId)
                .OnDelete(DeleteBehavior.Cascade);

            // Polisa ↔ Pojazd
            modelBuilder.Entity<PolisaUbezpieczeniowa>()
                .HasOne(p => p.Pojazd)
                .WithMany(poj => poj.Polisy)
                .HasForeignKey(p => p.PojazdId)
                .OnDelete(DeleteBehavior.Cascade);

            // KartaPaliwowa ↔ Kierowca (opcjonalnie)
            modelBuilder.Entity<KartaPaliwowa>()
                .HasOne(k => k.Kierowca)
                .WithMany(kier => kier.KartyPaliwowe)
                .HasForeignKey(k => k.KierowcaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Pojazd ↔ KategoriaPojazdu
            modelBuilder.Entity<Pojazd>()
                .HasOne(p => p.Kategoria)
                .WithMany(k => k.Pojazdy)
                .HasForeignKey(p => p.KategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tankowanie ↔ Pojazd (WIELE-DO-JEDNEGO) – POPRAWIONE
            modelBuilder.Entity<Tankowanie>()
                .HasOne(t => t.Pojazd)
                .WithMany(p => p.Tankowania)
                .HasForeignKey(t => t.PojazdId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tankowanie ↔ Kierowca (opcjonalne)
            modelBuilder.Entity<Tankowanie>()
                .HasOne(t => t.Kierowca)
                .WithMany(k => k.Tankowania)
                .HasForeignKey(t => t.KierowcaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tankowanie ↔ KartaPaliwowa (opcjonalne)
            modelBuilder.Entity<Tankowanie>()
                .HasOne(t => t.KartaPaliwowa)
                .WithMany(k => k.Tankowania)
                .HasForeignKey(t => t.KartaPaliwowaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indeksy i unikalności
            modelBuilder.Entity<Pojazd>()
                .HasIndex(p => p.NumerRejestracyjny)
                .IsUnique();

            modelBuilder.Entity<Kierowca>()
                .HasIndex(k => k.Email)
                .IsUnique();
        }
    }
}