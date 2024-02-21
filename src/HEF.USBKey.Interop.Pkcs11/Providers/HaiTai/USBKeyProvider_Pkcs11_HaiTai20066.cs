namespace HEF.USBKey.Interop.Pkcs11
{
    public class USBKeyProvider_Pkcs11_HaiTai20066 : USBKeyProvider_Pkcs11_Base, IUSBKeyProvider_Pkcs11
    {
        public const string Pkcs11_ProviderName = "HaiTai20066";

        public const string Pkcs11_LibraryRelativePath = @"HaiTai20066\HtPkcs1120066.dll";
        public const string Pkcs11_CspProviderName = "ZJCA CSP v1.0 for HaiTai 20066";

        public string ProviderName => Pkcs11_ProviderName;

        public USBKeyProvider_Pkcs11_HaiTai20066()
            : this(Pkcs11_LibraryRelativePath, Pkcs11_CspProviderName)
        { }

        public USBKeyProvider_Pkcs11_HaiTai20066(string libraryRelativePath, string cspProvideName)
            : base(libraryRelativePath, cspProvideName)
        { }
    }
}
