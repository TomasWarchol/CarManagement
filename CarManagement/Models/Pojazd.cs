namespace CarManagementApp.Models
{
    public class Pojazd
    {
        public int Id { get; set; }
        public string NumerRejestracyjny { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string? Marka { get; set; }
        public string? Model { get; set; }
        public int? Rok { get; set; }
        public string? TypNapedu { get; set; }  // np. Benzyna/EV/Diesel/MHEV
        public int? PrzebiegKm { get; set; }
        public string? Status { get; set; }     // Aktywny/Wycofany

        public int? KategoriaId { get; set; }
        public virtual KategoriaPojazdu? Kategoria { get; set; }

        public virtual ICollection<PrzypisaniePojazdu> Przypisania { get; } = new HashSet<PrzypisaniePojazdu>();
        public virtual ICollection<PolisaUbezpieczeniowa> Polisy { get; } = new HashSet<PolisaUbezpieczeniowa>();
        public virtual ICollection<SerwisPojazdu> Serwisy { get; } = new HashSet<SerwisPojazdu>();
        public virtual ICollection<Tankowanie> Tankowania { get; } = new HashSet<Tankowanie>();
    }
}