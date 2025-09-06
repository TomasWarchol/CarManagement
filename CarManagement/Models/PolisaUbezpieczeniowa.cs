namespace CarManagement.Models
{
    public class PolisaUbezpieczeniowa
    {
        public int Id { get; set; }
        public int PojazdId { get; set; }
        public string Typ { get; set; } = "OC"; // OC/AC/NW/AS
        public string Numer { get; set; } = string.Empty;
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }
        public virtual Pojazd Pojazd { get; set; } = null!;
    }
}