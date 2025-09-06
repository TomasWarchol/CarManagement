namespace CarManagement.Models
{
    public class Kierowca
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HasloHash { get; set; } = string.Empty;
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;

        public int? Id_Uprawnienia { get; set; }
        public virtual Uprawnienia? Uprawnienia { get; set; }

        public virtual ICollection<PrzypisaniePojazdu> Przypisania { get; } = new HashSet<PrzypisaniePojazdu>();
        public virtual ICollection<KartaPaliwowa> KartyPaliwowe { get; } = new HashSet<KartaPaliwowa>();
        public virtual ICollection<Tankowanie> Tankowania { get; } = new HashSet<Tankowanie>();
    }
}