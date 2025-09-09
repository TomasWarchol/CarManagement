namespace CarManagementApp.Models
{
    public class SerwisPojazdu
    {
        public int Id { get; set; }
        public int PojazdId { get; set; }
        public DateTime Data { get; set; }
        public int? PrzebiegKm { get; set; }
        public string? Rodzaj { get; set; } // przegląd/naprawa
        public decimal? Koszt { get; set; }

        public virtual Pojazd Pojazd { get; set; } = null!;
    }
}