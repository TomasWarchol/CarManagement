using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Models
{
    public class LogowanieViewModel
    {
        [Required(ErrorMessage = "Proszę podać e-mail")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
        [StringLength(254, ErrorMessage = "E-mail jest zbyt długi")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proszę podać hasło")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Hasło musi mieć od 8 do 128 znaków")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Haslo { get; set; } = string.Empty;

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
