namespace CarManagement.Models
{
    public class Rola
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;

        // Nie-nullowalna, od razu z HashSetem
        public virtual ICollection<PracownikAdministracyjny> Uzytkownicy { get; } = new HashSet<PracownikAdministracyjny>();
    }
}

