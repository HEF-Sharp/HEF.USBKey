namespace HEF.USBKey.Interop.Pkcs11
{
    public class USBKeyProvider_Pkcs11_HaiTai20240 : USBKeyProvider_Pkcs11_Base, IUSBKeyProvider_Pkcs11
    {
        public const string Pkcs11_ProviderName = "HaiTai20240";

        public const string Pkcs11_LibraryRelativePath = @"HaiTai20240\HtPkcs1120240.dll";
        public const string Pkcs11_CspProviderName = "ZJCA CSP v1.0 for HaiTai 20240";

        public string ProviderName => Pkcs11_ProviderName;

        public USBKeyProvider_Pkcs11_HaiTai20240()
            : this(Pkcs11_LibraryRelativePath, Pkcs11_CspProviderName)
        { }

        public USBKeyProvider_Pkcs11_HaiTai20240(string libraryRelativePath, string cspProvideName)
            : base(libraryRelativePath, cspProvideName)
        { }
    }
}
