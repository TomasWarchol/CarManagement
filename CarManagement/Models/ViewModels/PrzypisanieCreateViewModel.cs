using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Models.ViewModels
{
    public class PrzypisanieCreateViewModel
    {
        [Required] public int PojazdId { get; set; }
        [Required] public int KierowcaId { get; set; }
        [Required] public DateTime DataOd { get; set; } = DateTime.Today;
        public DateTime? DataDo { get; set; }
        public string? TrybUzytkowania { get; set; }
        public IEnumerable<Pojazd>? PojazdySelect { get; set; }
        public IEnumerable<Kierowca>? KierowcySelect { get; set; }
    }
}