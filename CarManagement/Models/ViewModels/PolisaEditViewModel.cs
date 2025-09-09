using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Models.ViewModels
{
    public class PolisaEditViewModel
    {
        public int? Id { get; set; }
        [Required] public int PojazdId { get; set; }
        [Required, StringLength(10)] public string Typ { get; set; } = "OC";
        [Required, StringLength(40)] public string Numer { get; set; } = string.Empty;
        [Required] public DateTime DataOd { get; set; } = DateTime.Today;
        [Required] public DateTime DataDo { get; set; } = DateTime.Today.AddYears(1);
        public IEnumerable<Pojazd>? PojazdySelect { get; set; }
    }
}