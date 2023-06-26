namespace HEF.USBKey.Interop.SKF
{
    public class SKF_DeviceEvent
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public int EventType { get; set; }
    }

    public enum SKF_DeviceEventTypes
    {
        None = 0,
        /// <summary>
        /// 插入
        /// </summary>
        PlugIn = 1,
        /// <summary>
        /// 拔出
        /// </summary>
        PullOut = 2
    }
}
