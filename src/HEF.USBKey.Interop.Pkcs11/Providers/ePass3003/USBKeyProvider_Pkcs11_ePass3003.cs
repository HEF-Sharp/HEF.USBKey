namespace HEF.USBKey.Interop.Pkcs11
{
    public class USBKeyProvider_Pkcs11_ePass3003 : USBKeyProvider_Pkcs11_Base, IUSBKeyProvider_Pkcs11
    {
        public const string Pkcs11_ProviderName = "ePass3003";

        public const string Pkcs11_LibraryRelativePath = @"ePass3003\ZJCAePassP11v211.dll";
        public const string Pkcs11_CspProviderName = "ZJCA CSP v1.0 for EnterSafe ePass3003";

        public string ProviderName => Pkcs11_ProviderName;

        public USBKeyProvider_Pkcs11_ePass3003()
            : this(Pkcs11_LibraryRelativePath, Pkcs11_CspProviderName)
        { }

        public USBKeyProvider_Pkcs11_ePass3003(string libraryRelativePath, string cspProvideName)
            : base(libraryRelativePath, cspProvideName)
        { }
    }
}
