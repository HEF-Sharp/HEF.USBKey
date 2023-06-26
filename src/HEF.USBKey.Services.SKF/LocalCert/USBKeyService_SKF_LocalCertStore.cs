using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_LocalCertStore : IUSBKeyService_SKF_LocalCertStore
    {
        private readonly ConcurrentDictionary<string, X509Certificate2Collection> _deviceX509CertsDict
            = new ConcurrentDictionary<string, X509Certificate2Collection>();

        public void AddDeviceCertsToLocalCurrentUser(string providerName, string deviceName, X509Certificate2Collection deviceCertCollection)
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadWrite);

            try
            {
                foreach (var deviceCert in deviceCertCollection)
                {
                    if (!x509Store.Certificates.Contains(deviceCert))
                        x509Store.Add(deviceCert);
                }

                var deviceCertsCacheKey = FormatDeviceCertsCacheKey(providerName, deviceName);
                _deviceX509CertsDict.AddOrUpdate(deviceCertsCacheKey, deviceCertCollection, (key, certCollection) => deviceCertCollection);
            }
            finally
            {
                x509Store.Close();
            }
        }

        public void RemoveDeviceCertsFromLocalCurrentUser(string providerName, string deviceName)
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadWrite);

            try
            {
                var deviceCertsCacheKey = FormatDeviceCertsCacheKey(providerName, deviceName);

                if (_deviceX509CertsDict.TryGetValue(deviceCertsCacheKey, out var deviceCertCollection))
                {
                    x509Store.RemoveRange(deviceCertCollection);

                    _deviceX509CertsDict.TryRemove(deviceCertsCacheKey, out var _);
                }
            }
            finally
            {
                x509Store.Close();
            }
        }

        public void RemoveAllDeviceCertsFromLocalCurrentUser()
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadWrite);

            try
            {
                foreach (var deviceCertsCacheKey in _deviceX509CertsDict.Keys)
                {
                    if (_deviceX509CertsDict.TryGetValue(deviceCertsCacheKey, out var deviceCertCollection))
                    {
                        x509Store.RemoveRange(deviceCertCollection);

                        _deviceX509CertsDict.TryRemove(deviceCertsCacheKey, out var _);
                    }
                }
            }
            finally
            {
                x509Store.Close();
            }
        }

        #region Helper Functions
        protected static string FormatDeviceCertsCacheKey(string providerName, string deviceName)
            => $"{providerName}--{deviceName}";
        #endregion
    }
}
