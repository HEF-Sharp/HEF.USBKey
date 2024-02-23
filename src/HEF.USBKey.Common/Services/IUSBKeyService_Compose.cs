using System.Collections.Generic;

namespace HEF.USBKey.Common
{
    public interface IUSBKeyService_Compose
    {
        /// <summary>
        /// 获取插入的设备
        /// </summary>
        IEnumerable<USBKey_PresentDevice> GetPresentDevices();

        /// <summary>
        /// 导出设备所有证书
        /// </summary>
        IEnumerable<USBKey_Certificate_X509> ExportDeviceCertificates(string providerName, string deviceIdOrName);        

        /// <summary>
        /// 修改设备PIN码
        /// </summary>
        bool ChangeDevicePIN(string providerName, string deviceIdOrName, string oldPin, string newPin);

        /// <summary>
        /// 校验设备PIN码
        /// </summary>
        bool VerifyDevicePIN(string providerName, string deviceIdOrName, string pin);

        /// <summary>
        /// 开始监测设备事件
        /// </summary>
        void StartMonitorDeviceEvent();

        /// <summary>
        /// 附加设备事件Handlers
        /// </summary>
        void AttachDeviceEventHandlers(params IUSBKey_Handler_DeviceEvent[] usbKeyDeviceEventHandlers);

        /// <summary>
        /// 取消监测设备事件
        /// </summary>
        void CancelMonitorDeviceEvent();
    }
}
