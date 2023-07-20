using HEF.USBKey.Interop.SKF;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_ePass3000GM : USBKeyService_SKF_ePass3003, IUSBKeyService_SKF
    {
        public USBKeyService_SKF_ePass3000GM()
            : base(new USBKeyProvider_SKF_ePass3000GM())
        { }
    }
}
