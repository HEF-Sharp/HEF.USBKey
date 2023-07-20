using HEF.USBKey.Interop.SKF;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_GX : USBKeyService_SKF_Base, IUSBKeyService_SKF
    {
        public USBKeyService_SKF_GX()
            : base(new USBKeyProvider_SKF_GX())
        { }
    }
}
