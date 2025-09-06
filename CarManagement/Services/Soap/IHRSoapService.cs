using System.ServiceModel;

namespace CarManagement.Services.Soap
{
    [ServiceContract]
    public interface IHRSoapService
    {
        [OperationContract]
        string Ping();
    }
}
