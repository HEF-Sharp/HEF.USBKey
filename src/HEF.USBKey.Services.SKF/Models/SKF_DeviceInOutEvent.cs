using HEF.USBKey.Interop.SKF;

namespace HEF.USBKey.Services.SKF
{
    public class SKF_DeviceInOutEvent : SKF_DeviceEvent
    {
        /// <summary>
        /// 提供程序名称
        /// </summary>
        public string ProviderName { get; set; }

        public bool IsPlugIn => EventType == (int)SKF_DeviceEventTypes.PlugIn;

        public bool IsPullOut => EventType == (int)SKF_DeviceEventTypes.PullOut;
    }
}
