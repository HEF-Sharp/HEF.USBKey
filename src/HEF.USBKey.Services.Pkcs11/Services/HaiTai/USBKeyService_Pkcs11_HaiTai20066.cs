using HEF.USBKey.Interop.Pkcs11;

namespace HEF.USBKey.Services.Pkcs11
{
    public class USBKeyService_Pkcs11_HaiTai20066 : USBKeyService_Pkcs11_Base, IUSBKeyService_Pkcs11
    {
        public USBKeyService_Pkcs11_HaiTai20066()
            : base(new USBKeyProvider_Pkcs11_HaiTai20066())
        { }
    }
}
