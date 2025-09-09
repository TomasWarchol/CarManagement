using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Models
{
    public class KategoriaPojazdu
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nazwa { get; set; } = string.Empty;

        public virtual ICollection<Pojazd> Pojazdy { get; } = new HashSet<Pojazd>();
    }
}