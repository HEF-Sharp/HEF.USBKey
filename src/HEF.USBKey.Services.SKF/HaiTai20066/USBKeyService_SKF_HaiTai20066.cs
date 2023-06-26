using HEF.USBKey.Interop.SKF;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_HaiTai20066 : USBKeyService_SKF_Base, IUSBKeyService_SKF
    {
        public USBKeyService_SKF_HaiTai20066()
            : base(new USBKeyProvider_SKF_HaiTai20066())
        { }
    }
}
