using Microsoft.AspNetCore.Mvc;


namespace CarManagement.Services.Soap
{
    public class HRSoapService : IHRSoapService
    {
        public string Ping() => "Działa";
    }
}
