using HEF.USBKey.Interop.SKF;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_LongMai3000GM : USBKeyService_SKF_Base, IUSBKeyService_SKF
    {
        public USBKeyService_SKF_LongMai3000GM()
            : base(new USBKeyProvider_SKF_LongMai3000GM())
        { }
    }
}
