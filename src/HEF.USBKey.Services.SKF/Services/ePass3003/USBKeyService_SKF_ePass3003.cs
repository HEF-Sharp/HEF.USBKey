using HEF.USBKey.Interop.SKF;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_ePass3003 : USBKeyService_SKF_Base_LoopMonitor, IUSBKeyService_SKF
    {
        public USBKeyService_SKF_ePass3003()
            : base(new USBKeyProvider_SKF_ePass3003())
        { }
    }
}
