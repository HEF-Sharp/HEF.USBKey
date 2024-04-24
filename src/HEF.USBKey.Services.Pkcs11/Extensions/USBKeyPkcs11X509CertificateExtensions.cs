using HEF.USBKey.Interop.Pkcs11;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Services.Pkcs11
{
    public static class USBKeyPkcs11X509CertificateExtensions
    {
        public static Pkcs11_Certificate_X509 BuildX509Certificate(
            this Pkcs11_Certificate slotCert, IUSBKeyProvider_Pkcs11 usbKeyPkcs11Provider)
        {
            var x509Cert = new X509Certificate2(slotCert.CertBytes, (SecureString)null, X509KeyStorageFlags.UserKeySet);

            var keyUsageExt = x509Cert.Extensions.OfType<X509KeyUsageExtension>().FirstOrDefault();
            slotCert.ForSign = keyUsageExt?.KeyUsages.HasFlag(X509KeyUsageFlags.DigitalSignature) ?? false;

            //Set hardware linked PrivateKey
            CspParameters csp = new CspParameters(1, usbKeyPkcs11Provider.CspProviderName, slotCert.Id);
            csp.KeyNumber = slotCert.ForSign ? (int)KeyNumber.Signature : (int)KeyNumber.Exchange;
            x509Cert.PrivateKey = new RSACryptoServiceProvider(csp);

            return new Pkcs11_Certificate_X509
            {
                Id = slotCert.Id,
                Label = slotCert.Label,
                ForSign = slotCert.ForSign,
                CertBytes = slotCert.CertBytes,
                X509Cert = x509Cert
            };
        }
    }
}
