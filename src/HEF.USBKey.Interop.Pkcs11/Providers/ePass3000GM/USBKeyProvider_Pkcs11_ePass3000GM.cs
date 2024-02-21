namespace HEF.USBKey.Interop.Pkcs11
{
    public class USBKeyProvider_Pkcs11_ePass3000GM : USBKeyProvider_Pkcs11_Base, IUSBKeyProvider_Pkcs11
    {
        public const string Pkcs11_ProviderName = "ePass3000GM";

        public const string Pkcs11_LibraryRelativePath = @"ePass3000GM\ZJCAePass3000gm.dll";
        public const string Pkcs11_CspProviderName = "ZJCA CSP v1.0 for EnterSafe ePass3000gm";

        public string ProviderName => Pkcs11_ProviderName;

        public USBKeyProvider_Pkcs11_ePass3000GM()
            : this(Pkcs11_LibraryRelativePath, Pkcs11_CspProviderName)
        { }

        public USBKeyProvider_Pkcs11_ePass3000GM(string libraryRelativePath, string cspProvideName)
            : base(libraryRelativePath, cspProvideName)
        { }
    }
}
