using Microsoft.AspNetCore.Mvc;


namespace CarManagementApp.Services.Soap
{
    public class CarSoapService : ICarSoapService
    {
        public string Ping() => "Działa";
    }
}
