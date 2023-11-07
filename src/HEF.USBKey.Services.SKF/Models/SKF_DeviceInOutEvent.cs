using HEF.USBKey.Common;
using HEF.USBKey.Interop.SKF;

namespace HEF.USBKey.Services.SKF
{
    public class SKF_DeviceInOutEvent : SKF_DeviceEvent
    {
        /// <summary>
        /// 提供程序名称
        /// </summary>
        public string ProviderName { get; set; }

        public DeviceEventTypes InOutEventType => FormatDeviceEventType(EventType);

        private static DeviceEventTypes FormatDeviceEventType(int eventType)
        {
            return eventType switch
            {
                (int)DeviceEventTypes.PlugIn => DeviceEventTypes.PlugIn,
                (int)DeviceEventTypes.PullOut => DeviceEventTypes.PullOut,
                _ => DeviceEventTypes.None,
            };
        }
    }
}
