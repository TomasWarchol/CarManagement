using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Proszę podać imię")]
    [Display(Name = "Imię")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Imię musi mieć od 2 do 100 znaków")]
    [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż' -]+$", ErrorMessage = "Imię może zawierać tylko litery, spacje, myślnik i apostrof")]
    public string Imie { get; set; } = string.Empty;

    [Required(ErrorMessage = "Proszę podać nazwisko")]
    [Display(Name = "Nazwisko")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nazwisko musi mieć od 2 do 100 znaków")]
    [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż' -]+$", ErrorMessage = "Nazwisko może zawierać tylko litery, spacje, myślnik i apostrof")]
    public string Nazwisko { get; set; } = string.Empty;

    [Required(ErrorMessage = "Proszę podać e-mail")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
    [StringLength(254, ErrorMessage = "E-mail jest zbyt długi")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Proszę podać hasło")]
    [DataType(DataType.Password)]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Hasło musi mieć od 8 do 128 znaków")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$",
        ErrorMessage = "Hasło musi zawierać małą i wielką literę oraz cyfrę")]
    [Display(Name = "Hasło")]
    public string Haslo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Proszę potwierdzić hasło")]
    [DataType(DataType.Password)]
    [Compare(nameof(Haslo), ErrorMessage = "Hasła nie są zgodne")]
    [Display(Name = "Potwierdź hasło")]
    public string ConfirmHaslo { get; set; } = string.Empty;
}
