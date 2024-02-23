namespace HEF.USBKey.Common
{
    public class USBKey_DeviceInOutEvent
    {
        public string ProviderName { get; set; }

        public string DeviceIdOrName { get; set; }

        public DeviceEventTypes InOutEventType { get; set; }
    }
}
