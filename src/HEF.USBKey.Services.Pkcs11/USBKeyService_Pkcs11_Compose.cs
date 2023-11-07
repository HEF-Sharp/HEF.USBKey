using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Services.Pkcs11
{
    public class USBKeyService_Pkcs11_Compose : IUSBKeyService_Pkcs11_Compose
    {
        private readonly IDictionary<string, IUSBKeyService_Pkcs11> _usbKeyPkcs11ServiceDict;

        #region Constructor
        public USBKeyService_Pkcs11_Compose(IEnumerable<IUSBKeyService_Pkcs11> usbKeyPkcs11Services)
        {
            if (usbKeyPkcs11Services == null || !usbKeyPkcs11Services.Any())
                throw new ArgumentNullException(nameof(usbKeyPkcs11Services));

            _usbKeyPkcs11ServiceDict = new Dictionary<string, IUSBKeyService_Pkcs11>();
            InitUSBKeyPkcs11Services(usbKeyPkcs11Services);            
        }

        private void InitUSBKeyPkcs11Services(IEnumerable<IUSBKeyService_Pkcs11> usbKeyPkcs11Services)
        {
            foreach (var usbKeyPkcs11Service in usbKeyPkcs11Services)
            {
                AddOrUpdateUSBKeyPkcs11Service(usbKeyPkcs11Service);
            }
        }

        private void AddOrUpdateUSBKeyPkcs11Service(IUSBKeyService_Pkcs11 usbKeyPkcs11Service)
        {
            var providerName = usbKeyPkcs11Service.Provider.ProviderName;

            if (_usbKeyPkcs11ServiceDict.ContainsKey(providerName))
            {
                _usbKeyPkcs11ServiceDict[providerName] = usbKeyPkcs11Service;
                return;
            }

            _usbKeyPkcs11ServiceDict.Add(providerName, usbKeyPkcs11Service);
        }
        #endregion

        public IEnumerable<Pkcs11_PresentSlot> GetPresentSlotList()
        {
            foreach (var usbKeyPkcs11Service in _usbKeyPkcs11ServiceDict.Values)
            {
                var presentSlots = usbKeyPkcs11Service.GetPresentSlotList();

                foreach (var presentSlot in presentSlots)
                {
                    yield return new Pkcs11_PresentSlot
                    {
                        ProviderName = usbKeyPkcs11Service.Provider.ProviderName,
                        Slot = presentSlot
                    };
                }
            }
        }

        public IEnumerable<Pkcs11_Certificate_X509> ExportCertificates(string providerName, ulong slotId)
        {
            var usbKeyPkcs11Service = GetMatchUSBKeyPkcs11Service(providerName);

            var slotCerts = usbKeyPkcs11Service.ExportCertificates(slotId);

            foreach (var slotCert in slotCerts)
            {
                var x509Cert = new X509Certificate2(slotCert.CertBytes, (SecureString)null, X509KeyStorageFlags.UserKeySet);

                //Set hardware linked PrivateKey
                CspParameters csp = new CspParameters(1, usbKeyPkcs11Service.Provider.CspProviderName, slotCert.Id);
                csp.KeyNumber = slotCert.ForSign ? (int)KeyNumber.Signature : (int)KeyNumber.Exchange;
                x509Cert.PrivateKey = new RSACryptoServiceProvider(csp);

                yield return new Pkcs11_Certificate_X509
                {
                    Id = slotCert.Id,
                    Label = slotCert.Label,
                    ForSign = slotCert.ForSign,
                    CertBytes = slotCert.CertBytes,
                    X509Cert = x509Cert
                };
            }
        }

        #region Helper Functions
        protected IUSBKeyService_Pkcs11 GetMatchUSBKeyPkcs11Service(string providerName)
        {
            if (_usbKeyPkcs11ServiceDict.TryGetValue(providerName, out var matchUSBKeyPkcs11Service))
            {
                return matchUSBKeyPkcs11Service;
            }

            throw new InvalidOperationException("not found target provider name usbKey pkcs11 service");
        }
        #endregion
    }
}
