using System.Collections.Generic;

namespace HEF.USBKey.Services.SKF
{
    public interface IUSBKeyService_SKF_LocalCertStore
    {
        IEnumerable<SKF_Certificate_X509> GetDeviceCertsFromCache(string providerName, string deviceName);

        void AddDeviceCertsToLocalCurrentUser(string providerName, string deviceName, params SKF_Certificate_X509[] deviceX509Certs);

        void RemoveDeviceCertsFromLocalCurrentUser(string providerName, string deviceName);

        void RemoveAllDeviceCertsFromLocalCurrentUser();
    }
}
