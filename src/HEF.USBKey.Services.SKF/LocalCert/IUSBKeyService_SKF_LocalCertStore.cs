using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Services.SKF
{
    public interface IUSBKeyService_SKF_LocalCertStore
    {
        void AddDeviceCertsToLocalCurrentUser(string providerName, string deviceName, X509Certificate2Collection deviceCertCollection);

        void RemoveDeviceCertsFromLocalCurrentUser(string providerName, string deviceName);

        void RemoveAllDeviceCertsFromLocalCurrentUser();
    }
}
