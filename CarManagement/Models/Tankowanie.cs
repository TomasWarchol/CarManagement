namespace CarManagement.Models
{
    public class Tankowanie
    {
        public int Id { get; set; }
        public int PojazdId { get; set; }
        public int? KierowcaId { get; set; }
        public int? KartaPaliwowaId { get; set; }

        public DateTime Data { get; set; }
        public decimal IloscLitrow { get; set; }
        public decimal CenaZaLitr { get; set; }
        public decimal Kwota { get; set; } // można liczyć po stronie app/DB
        public int? PrzebiegKm { get; set; }

        public virtual Pojazd Pojazd { get; set; } = null!;
        public virtual Kierowca? Kierowca { get; set; }
        public virtual KartaPaliwowa? KartaPaliwowa { get; set; }
    }
}