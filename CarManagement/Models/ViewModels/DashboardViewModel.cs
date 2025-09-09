namespace CarManagementApp.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int LiczbaAktywnychPojazdow { get; set; }
        public int WygasajacePolisy30Dni { get; set; }
        public int NadchodzaceSerwisy30Dni { get; set; }
        public int LiczbaKierowcow { get; set; }
        public List<ZblizajacyTerminDto> ZblizajaceTerminy { get; set; } = new();
    }

    public class ZblizajacyTerminDto
    {
        public string Typ { get; set; } = string.Empty; // Polisa/Serwis
        public string Opis { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public int PojazdId { get; set; }
        public string NumerRejestracyjny { get; set; } = string.Empty;
    }
}