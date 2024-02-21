using System.Collections.Generic;

namespace HEF.USBKey.Common
{
    public interface IUSBKeyService_LocalCertStore
    {
        IEnumerable<USBKey_Certificate_X509> GetDeviceCertsFromCache(string providerName, string deviceId);

        void AddDeviceCertsToLocalCurrentUser(string providerName, string deviceId, params USBKey_Certificate_X509[] deviceX509Certs);

        void RemoveDeviceCertsFromLocalCurrentUser(string providerName, string deviceId);

        void RemoveAllDeviceCertsFromLocalCurrentUser();
    }
}
