using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarManagementApp.Models
{
    public class PracownikAdministracyjny
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(128, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Haslo { get; set; } = string.Empty;

        [Required]
        public string Imie { get; set; } = string.Empty;

        [Required]
        public string Nazwisko { get; set; } = string.Empty;

        [ForeignKey(nameof(Uprawnienia))]
        public int? Id_Uprawnienia { get; set; }

        public virtual Uprawnienia? Uprawnienia { get; set; }

    }

}
