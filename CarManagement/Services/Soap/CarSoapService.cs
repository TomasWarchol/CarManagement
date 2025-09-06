using Microsoft.AspNetCore.Mvc;


namespace CarManagement.Services.Soap
{
    public class CarSoapService : ICarSoapService
    {
        public string Ping() => "Działa";
    }
}
