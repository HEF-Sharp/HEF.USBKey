using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Common
{
    public class USBKeyService_LocalCertStore : IUSBKeyService_LocalCertStore
    {
        private readonly ConcurrentDictionary<string, USBKey_Certificate_X509[]> _deviceX509CertsDict
            = new ConcurrentDictionary<string, USBKey_Certificate_X509[]>();

        public IEnumerable<USBKey_Certificate_X509> GetDeviceCertsFromCache(string providerName, string deviceId)
        {
            var deviceCertsCacheKey = FormatDeviceCertsCacheKey(providerName, deviceId);

            if (_deviceX509CertsDict.TryGetValue(deviceCertsCacheKey, out var deviceX509Certs))
            {
                return deviceX509Certs;
            }

            return Enumerable.Empty<USBKey_Certificate_X509>();
        }

        public void AddDeviceCertsToLocalCurrentUser(string providerName, string deviceId, params USBKey_Certificate_X509[] deviceX509Certs)
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadWrite);

            try
            {
                var deviceCertCollection = BuildDeviceCertCollection(deviceX509Certs);

                foreach (var deviceCert in deviceCertCollection)
                {
                    if (!x509Store.Certificates.Contains(deviceCert))
                        x509Store.Add(deviceCert);
                }

                var deviceCertsCacheKey = FormatDeviceCertsCacheKey(providerName, deviceId);
                _deviceX509CertsDict.AddOrUpdate(deviceCertsCacheKey, deviceX509Certs, (key, certCollection) => deviceX509Certs);
            }
            finally
            {
                x509Store.Close();
            }
        }

        #region RemoveDeviceCerts
        public void RemoveDeviceCertsFromLocalCurrentUser(string providerName, string deviceId)
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadWrite);

            try
            {
                var deviceCertsCacheKey = FormatDeviceCertsCacheKey(providerName, deviceId);

                RemoveDeviceCertsFromX509Store(deviceCertsCacheKey, x509Store);
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
                    RemoveDeviceCertsFromX509Store(deviceCertsCacheKey, x509Store);
                }
            }
            finally
            {
                x509Store.Close();
            }
        }

        protected void RemoveDeviceCertsFromX509Store(string deviceCertsCacheKey, X509Store x509Store)
        {
            if (_deviceX509CertsDict.TryGetValue(deviceCertsCacheKey, out var deviceX509Certs))
            {
                var deviceCertCollection = BuildDeviceCertCollection(deviceX509Certs);
                x509Store.RemoveRange(deviceCertCollection);

                _deviceX509CertsDict.TryRemove(deviceCertsCacheKey, out var _);
            }
        }
        #endregion

        #region Helper Functions
        protected static string FormatDeviceCertsCacheKey(string providerName, string deviceId)
            => $"{providerName}--{deviceId}";

        protected static X509Certificate2Collection BuildDeviceCertCollection(params USBKey_Certificate_X509[] deviceX509Certs)
        {
            var deviceCertCollection = new X509Certificate2Collection();

            deviceCertCollection.AddRange(deviceX509Certs.Select(m => m.X509Cert).ToArray());

            return deviceCertCollection;
        }
        #endregion
    }
}
