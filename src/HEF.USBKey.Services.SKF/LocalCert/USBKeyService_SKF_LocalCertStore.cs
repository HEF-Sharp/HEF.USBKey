using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_LocalCertStore : IUSBKeyService_SKF_LocalCertStore
    {
        private readonly ConcurrentDictionary<string, SKF_Certificate_X509[]> _deviceX509CertsDict
            = new ConcurrentDictionary<string, SKF_Certificate_X509[]>();

        public IEnumerable<SKF_Certificate_X509> GetDeviceCertsFromCache(string providerName, string deviceName)
        {
            var deviceCertsCacheKey = FormatDeviceCertsCacheKey(providerName, deviceName);

            if (_deviceX509CertsDict.TryGetValue(deviceCertsCacheKey, out var deviceX509Certs))
            {
                return deviceX509Certs;
            }

            return Enumerable.Empty<SKF_Certificate_X509>();
        }

        public void AddDeviceCertsToLocalCurrentUser(string providerName, string deviceName, params SKF_Certificate_X509[] deviceX509Certs)
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

                var deviceCertsCacheKey = FormatDeviceCertsCacheKey(providerName, deviceName);
                _deviceX509CertsDict.AddOrUpdate(deviceCertsCacheKey, deviceX509Certs, (key, certCollection) => deviceX509Certs);
            }
            finally
            {
                x509Store.Close();
            }
        }

        #region RemoveDeviceCerts
        public void RemoveDeviceCertsFromLocalCurrentUser(string providerName, string deviceName)
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadWrite);

            try
            {
                var deviceCertsCacheKey = FormatDeviceCertsCacheKey(providerName, deviceName);

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
        protected static string FormatDeviceCertsCacheKey(string providerName, string deviceName)
            => $"{providerName}--{deviceName}";

        protected static X509Certificate2Collection BuildDeviceCertCollection(params SKF_Certificate_X509[] deviceX509Certs)
        {
            var deviceCertCollection = new X509Certificate2Collection();

            deviceCertCollection.AddRange(deviceX509Certs.Select(m => m.X509Cert).ToArray());

            return deviceCertCollection;
        }
        #endregion
    }
}
