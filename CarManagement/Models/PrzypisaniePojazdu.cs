namespace CarManagement.Models
{
    public class PrzypisaniePojazdu
    {
        public int Id { get; set; }
        public int PojazdId { get; set; }
        public int KierowcaId { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime? DataDo { get; set; }
        public string? TrybUzytkowania { get; set; } // Służbowy/Prac.-prywatny

        public virtual Pojazd Pojazd { get; set; } = null!;
        public virtual Kierowca Kierowca { get; set; } = null!;
    }
}