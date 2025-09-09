using System.ServiceModel;

namespace CarManagementApp.Services.Soap
{
    [ServiceContract]
    public interface ICarSoapService
    {
        [OperationContract]
        string Ping();
    }
}
