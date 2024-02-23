using System;
using System.Collections.Generic;
using System.Threading;

namespace HEF.USBKey.Common
{
    public interface IUSBKeyService
    {
        /// <summary>
        /// 获取插入的设备
        /// </summary>
        IEnumerable<USBKey_PresentDevice> GetPresentDevices();

        /// <summary>
        /// 获取插入设备信息
        /// </summary>
        USBKey_PresentDevice GetPresentDevice(string deviceIdOrName);

        /// <summary>
        /// 导出设备所有证书
        /// </summary>
        IEnumerable<USBKey_Certificate> ExportDeviceCertificates(string deviceIdOrName);

        /// <summary>
        /// 修改设备PIN码
        /// </summary>
        bool ChangeDevicePIN(string deviceIdOrName, string oldPin, string newPin);

        /// <summary>
        /// 校验设备PIN码
        /// </summary>
        bool VerifyDevicePIN(string deviceIdOrName, string pin);

        /// <summary>
        /// 开始监测设备事件
        /// </summary>
        void StartMonitorDeviceEvent(Action<USBKey_DeviceInOutEvent> deviceEventAction, CancellationToken cancellationToken);
    }
}
