namespace HEF.USBKey.Interop.Pkcs11
{
    public class USBKeyProvider_Pkcs11_GX : USBKeyProvider_Pkcs11_Base, IUSBKeyProvider_Pkcs11
    {
        public const string Pkcs11_ProviderName = "GX";

        public const string Pkcs11_LibraryRelativePath = @"GX\pkcs11.dll";
        public const string Pkcs11_CspProviderName = "ZJCA CC RSA Crypto Provider For CCID UKEY v1.0";

        public string ProviderName => Pkcs11_ProviderName;

        public USBKeyProvider_Pkcs11_GX()
            : this(Pkcs11_LibraryRelativePath, Pkcs11_CspProviderName)
        { }

        public USBKeyProvider_Pkcs11_GX(string libraryRelativePath, string cspProvideName)
            : base(libraryRelativePath, cspProvideName)
        { }
    }
}
