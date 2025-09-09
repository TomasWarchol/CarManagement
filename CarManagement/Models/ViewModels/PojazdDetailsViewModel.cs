namespace CarManagementApp.Models.ViewModels
{
    public class PojazdDetailsViewModel
    {
        public Pojazd Pojazd { get; set; } = null!;
        public IEnumerable<PolisaUbezpieczeniowa> PolisyAktywne { get; set; } = new List<PolisaUbezpieczeniowa>();
        public IEnumerable<SerwisPojazdu> OstatnieSerwisy { get; set; } = new List<SerwisPojazdu>();
        public IEnumerable<PrzypisaniePojazdu> HistoriaPrzypisan { get; set; } = new List<PrzypisaniePojazdu>();
        public decimal? SrednieSpalanieL100 { get; set; }
    }
}