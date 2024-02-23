namespace HEF.USBKey.Common
{
    public class USBKey_PresentDevice
    {
        public string ProviderName { get; set; }

        public string DeviceIdOrName { get; set; }

        /// <summary>
        /// 设备厂商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 设备序列号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 设备硬件版本
        /// </summary>        
        public string HardwareVersion { get; set; }

        /// <summary>
        /// 设备固件版本
        /// </summary>        
        public string FirmwareVersion { get; set; }
    }
}
