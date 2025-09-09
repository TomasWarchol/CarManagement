using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementApp.Models
{
    public class Uprawnienia
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty; // np. Admin, FleetManager, User
        public virtual ICollection<Kierowca> Uzytkownicy { get; } = new HashSet<Kierowca>();
    }
}
