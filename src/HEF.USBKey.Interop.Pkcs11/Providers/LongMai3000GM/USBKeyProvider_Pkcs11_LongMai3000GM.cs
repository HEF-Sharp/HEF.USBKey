namespace HEF.USBKey.Interop.Pkcs11
{
    public class USBKeyProvider_Pkcs11_LongMai3000GM : USBKeyProvider_Pkcs11_Base, IUSBKeyProvider_Pkcs11
    {
        public const string Pkcs11_ProviderName = "LongMai3000GM";

        public const string Pkcs11_LibraryRelativePath = @"LongMai3000GM\GM3000ZJCA_P11.dll";
        public const string Pkcs11_CspProviderName = "ZJCA CSP v1.0 for Longmai gm3000";

        public string ProviderName => Pkcs11_ProviderName;

        public USBKeyProvider_Pkcs11_LongMai3000GM()
            : this(Pkcs11_LibraryRelativePath, Pkcs11_CspProviderName)
        { }

        public USBKeyProvider_Pkcs11_LongMai3000GM(string libraryRelativePath, string cspProvideName)
            : base(libraryRelativePath, cspProvideName)
        { }
    }
}
