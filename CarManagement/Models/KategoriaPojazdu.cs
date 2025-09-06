namespace CarManagement.Models
{
    public class KategoriaPojazdu
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public virtual ICollection<Pojazd> Pojazdy { get; } = new HashSet<Pojazd>();
    }
}