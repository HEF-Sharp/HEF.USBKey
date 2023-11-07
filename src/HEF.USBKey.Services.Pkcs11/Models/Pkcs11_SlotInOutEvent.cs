using HEF.USBKey.Common;

namespace HEF.USBKey.Services.Pkcs11
{
    public class Pkcs11_SlotInOutEvent
    {
        public string ProviderName { get; set; }

        public ulong SlotId { get; set; }

        public DeviceEventTypes InOutEventType { get; set; }
    }
}
