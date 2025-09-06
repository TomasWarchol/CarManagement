namespace CarManagement.Models
{
    public class KartaPaliwowa
    {
        public int Id { get; set; }
        public string Numer { get; set; } = string.Empty;
        public decimal? LimitMiesieczny { get; set; }
        public bool Aktywna { get; set; } = true;

        public int? KierowcaId { get; set; }
        public virtual Kierowca? Kierowca { get; set; }

        public virtual ICollection<Tankowanie> Tankowania { get; } = new HashSet<Tankowanie>();
    }
}