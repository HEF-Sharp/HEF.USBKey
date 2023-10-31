using System;

namespace HEF.USBKey.Interop.Pkcs11
{
    public class USBKeyProvider_Pkcs11_HaiTai20066 : USBKeyProvider_Pkcs11_Base, IUSBKeyProvider_Pkcs11
    {
        public const string Pkcs11_ProviderName = "HaiTai20066";

        public string ProviderName => Pkcs11_ProviderName;

        public USBKeyProvider_Pkcs11_HaiTai20066(string libraryRelativePath, string cspProvideName)
            : base(libraryRelativePath)
        {
            if (string.IsNullOrWhiteSpace(cspProvideName))
                throw new ArgumentNullException(cspProvideName);

            CspProviderName = cspProvideName;
        }

        public string CspProviderName { get; }
    }
}
