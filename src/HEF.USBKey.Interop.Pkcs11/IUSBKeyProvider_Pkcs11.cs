using System;

namespace HEF.USBKey.Interop.Pkcs11
{
    public interface IUSBKeyProvider_Pkcs11 : IDisposable
    {
        string ProviderName { get; }

        string CspProviderName { get; }
    }
}
