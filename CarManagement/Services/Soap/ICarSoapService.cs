using System.ServiceModel;

namespace CarManagement.Services.Soap
{
    [ServiceContract]
    public interface ICarSoapService
    {
        [OperationContract]
        string Ping();
    }
}
