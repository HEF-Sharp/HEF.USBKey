using HEF.USBKey.Interop.SKF;
using System.Collections.Generic;

namespace HEF.USBKey.Services.SKF
{
    public interface IUSBKeyService_SKF_Compose
    {
        /// <summary>
        /// 获取插入的设备
        /// </summary>
        IEnumerable<SKF_PresentDevice> GetPresentDevices();

        /// <summary>
        /// 导出设备所有证书
        /// </summary>
        IEnumerable<SKF_Certificate_X509> ExportDeviceCertificates(string providerName, string deviceName);

        /// <summary>
        /// 修改设备默认App对应PIN码
        /// </summary>
        SKFResult<int> ChangeDeviceDefaultAppPIN(string providerName, string deviceName, string oldPin, string newPin);

        /// <summary>
        /// 修改设备App对应PIN码
        /// </summary>
        SKFResult<int> ChangeDeviceAppPIN(string providerName, string deviceName, string appName, string oldPin, string newPin);

        /// <summary>
        /// 校验设备默认App对应PIN码
        /// </summary>
        SKFResult<int> VerifyDeviceDefaultAppPIN(string providerName, string deviceName, string pin);

        /// <summary>
        /// 校验设备App对应PIN码
        /// </summary>
        SKFResult<int> VerifyDeviceAppPIN(string providerName, string deviceName, string appName, string pin);

        /// <summary>
        /// 开始监测设备事件
        /// </summary>
        void StartMonitorDeviceEvent(params IUSBKey_SKF_Handler_DeviceEvent[] usbKeyDeviceEventHandlers);

        /// <summary>
        /// 取消监测设备事件
        /// </summary>
        void CancelMonitorDeviceEvent();
    }
}
