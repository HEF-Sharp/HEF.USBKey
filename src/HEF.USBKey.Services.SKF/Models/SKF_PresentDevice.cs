namespace HEF.USBKey.Services.SKF
{
    public class SKF_PresentDevice
    {
        /// <summary>
        /// 提供程序名称
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }        

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 设备厂商
        /// </summary>        
        public string Manufacturer { get; set; }

        /// <summary>
        /// 发行厂商
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 设备序列号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 设备硬件版本
        /// </summary>        
        public string HWVersion { get; set; }

        /// <summary>
        /// 设备固件版本
        /// </summary>        
        public string FirmwareVersion { get; set; }
    }
}
