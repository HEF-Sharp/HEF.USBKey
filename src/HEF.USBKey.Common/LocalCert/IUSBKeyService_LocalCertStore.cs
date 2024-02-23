using System.Collections.Generic;

namespace HEF.USBKey.Common
{
    public interface IUSBKeyService_LocalCertStore
    {
        IEnumerable<USBKey_Certificate_X509> GetDeviceCertsFromCache(string providerName, string deviceIdOrName);

        void AddDeviceCertsToLocalCurrentUser(string providerName, string deviceIdOrName, params USBKey_Certificate_X509[] deviceX509Certs);

        void RemoveDeviceCertsFromLocalCurrentUser(string providerName, string deviceIdOrName);

        void RemoveAllDeviceCertsFromLocalCurrentUser();
    }
}
