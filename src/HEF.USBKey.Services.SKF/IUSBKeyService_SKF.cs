using HEF.USBKey.Interop.SKF;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HEF.USBKey.Services.SKF
{
    public interface IUSBKeyService_SKF
    {
        IUSBKeyProvider_SKF Provider { get; }

        /// <summary>
        /// 获取插入的设备
        /// </summary>
        IEnumerable<SKF_PresentDevice> GetPresentDevices();

        /// <summary>
        /// 获取插入设备信息
        /// </summary>
        SKF_PresentDevice GetPresentDevice(string deviceName);

        /// <summary>
        /// 导出设备所有证书
        /// </summary>
        IEnumerable<SKF_Certificate> ExportDeviceCertificates(string deviceName);

        /// <summary>
        /// 导出设备所有证书
        /// </summary>
        IEnumerable<SKF_Certificate_X509> ExportDeviceX509Certificates(string deviceName);

        /// <summary>
        /// 修改设备默认App对应PIN码
        /// </summary>
        SKFResult<int> ChangeDeviceDefaultAppPIN(string deviceName, string oldPin, string newPin);

        /// <summary>
        /// 修改设备App对应PIN码
        /// </summary>
        SKFResult<int> ChangeDeviceAppPIN(string deviceName, string appName, string oldPin, string newPin);

        /// <summary>
        /// 校验设备默认App对应PIN码
        /// </summary>
        SKFResult<int> VerifyDeviceDefaultAppPIN(string deviceName, string pin);

        /// <summary>
        /// 校验设备App对应PIN码
        /// </summary>
        SKFResult<int> VerifyDeviceAppPIN(string deviceName, string appName, string pin);

        /// <summary>
        /// 开始监测设备事件
        /// </summary>
        void StartMonitorDeviceEvent(Action<SKF_DeviceInOutEvent> deviceEventAction, CancellationToken cancellationToken);
    }
}
