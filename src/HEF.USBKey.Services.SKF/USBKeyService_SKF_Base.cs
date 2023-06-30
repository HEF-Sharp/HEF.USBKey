using HEF.USBKey.Interop.SKF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEF.USBKey.Services.SKF
{
    public abstract class USBKeyService_SKF_Base
    {
        private Task _monitorDeviceEventTask;

        internal USBKeyService_SKF_Base(IUSBKeyProvider_SKF usbKeyProvider_SKF)
        {
            Provider = usbKeyProvider_SKF ?? throw new ArgumentNullException(nameof(usbKeyProvider_SKF));
        }

        public IUSBKeyProvider_SKF Provider { get; }

        public IEnumerable<SKF_PresentDevice> GetPresentDevices()
        {
            var enumDeviceResult = Provider.SKF_EnumDevice(true);
            foreach (var deviceName in enumDeviceResult.Data ?? Enumerable.Empty<string>())
            {
                using var deviceConnection = Provider.ACT_DeviceConnection(deviceName);

                if (!deviceConnection.Connected)
                    continue;
                   
                var infoResult = Provider.SKF_GetDeviceInfo(deviceConnection.HDevice);
                if (infoResult.IsSuccess())
                {
                    var deviceInfo = infoResult.Data;

                    var presentDevice = new SKF_PresentDevice
                    {
                        ProviderName = Provider.ProviderName,
                        DeviceName = deviceName,
                        Version = FormatDeviceVersion(deviceInfo.Version),
                        Manufacturer = deviceInfo.Manufacturer.ToASCIIString(),
                        Issuer = deviceInfo.Issuer.ToASCIIString(),
                        SerialNumber = deviceInfo.SerialNumber.ToASCIIString(),
                        HWVersion = FormatDeviceVersion(deviceInfo.HWVersion),
                        FirmwareVersion = FormatDeviceVersion(deviceInfo.FirmwareVersion)
                    };

                    yield return presentDevice;
                }
            }
        }

        public IEnumerable<SKF_Certificate> ExportDeviceCertificates(string deviceName)
        {
            using var deviceConnection = Provider.ACT_DeviceConnection(deviceName);
            if (deviceConnection.Connected)
            {
                var enumAppResult = Provider.SKF_EnumApplication(deviceConnection.HDevice);
                foreach (var appName in enumAppResult.Data ?? Enumerable.Empty<string>())
                {
                    using var appConnection = Provider.ACT_ApplicationConnection(deviceConnection.HDevice, appName);

                    if (!appConnection.Opened)
                        continue;
                        
                    var enumContainerResult = Provider.SKF_EnumContainer(appConnection.HApplication);                    
                    foreach (var containerName in enumContainerResult.Data ?? Enumerable.Empty<string>())
                    {
                        using var containerConnection = Provider.ACT_ContainerConnection(appConnection.HApplication, containerName);

                        if (!containerConnection.Opened)
                            continue;
                        
                        //签名证书
                        var certResult = Provider.SKF_ExportCertificate(containerConnection.HContainer, true);
                        if (certResult.IsSuccess())
                        {
                            yield return new SKF_Certificate { ForSign = true, CertBytes = certResult.Data };
                        }

                        //加密证书
                        certResult = Provider.SKF_ExportCertificate(containerConnection.HContainer, false);
                        if (certResult.IsSuccess())
                        {
                            yield return new SKF_Certificate { ForSign = false, CertBytes = certResult.Data };
                        }
                    }
                }
            }
        }

        #region Change DeviceAppPIN
        public SKFResult<int> ChangeDeviceDefaultAppPIN(string deviceName, string oldPin, string newPin)
        {
            return ChangeDeviceAppPIN(deviceName, (hDevice) =>
            {
                var enumAppResult = Provider.SKF_EnumApplication(hDevice);
                return enumAppResult.Data?.FirstOrDefault();
            }, oldPin, newPin);
        }

        public SKFResult<int> ChangeDeviceAppPIN(string deviceName, string appName, string oldPin, string newPin)
        {
            return ChangeDeviceAppPIN(deviceName, (hDevice) => appName, oldPin, newPin);
        }

        protected SKFResult<int> ChangeDeviceAppPIN(string deviceName, Func<IntPtr, string> appNameGetter, string oldPin, string newPin)
        {
            using var deviceConnection = Provider.ACT_DeviceConnection(deviceName);
            if (!deviceConnection.Connected)
                return deviceConnection.ConnectResult.ToResult<int>();

            var appName = appNameGetter.Invoke(deviceConnection.HDevice);

            using var appConnection = Provider.ACT_ApplicationConnection(deviceConnection.HDevice, appName);
            if (!appConnection.Opened)
                return appConnection.OpenResult.ToResult<int>();

            return Provider.SKF_ChangePIN(appConnection.HApplication, (int)SKF_PINTypes.UserType, oldPin, newPin);
        }
        #endregion

        #region Verify DeviceAppPIN
        public SKFResult<int> VerifyDeviceDefaultAppPIN(string deviceName, string pin)
        {
            return VerifyDeviceAppPIN(deviceName, (hDevice) =>
            {
                var enumAppResult = Provider.SKF_EnumApplication(hDevice);
                return enumAppResult.Data?.FirstOrDefault();
            }, pin);
        }

        public SKFResult<int> VerifyDeviceAppPIN(string deviceName, string appName, string pin)
        {
            return VerifyDeviceAppPIN(deviceName, (hDevice) => appName, pin);
        }

        protected SKFResult<int> VerifyDeviceAppPIN(string deviceName, Func<IntPtr, string> appNameGetter, string pin)
        {
            using var deviceConnection = Provider.ACT_DeviceConnection(deviceName);
            if (!deviceConnection.Connected)
                return deviceConnection.ConnectResult.ToResult<int>();

            var appName = appNameGetter.Invoke(deviceConnection.HDevice);

            using var appConnection = Provider.ACT_ApplicationConnection(deviceConnection.HDevice, appName);
            if (!appConnection.Opened)
                return appConnection.OpenResult.ToResult<int>();

            return Provider.SKF_VerifyPIN(appConnection.HApplication, (int)SKF_PINTypes.UserType, pin);
        }
        #endregion

        public void StartMonitorDeviceEvent(Action<SKF_DeviceInOutEvent> deviceEventAction, CancellationToken cancellationToken)
        {
            if (_monitorDeviceEventTask != null && _monitorDeviceEventTask.Status == TaskStatus.Running)
                return;   //监听设备事件 任务正在运行 则不做处理

            _monitorDeviceEventTask = Task.Factory.StartNew(() => ProcessingWaitDeviceEvent(deviceEventAction, cancellationToken), TaskCreationOptions.LongRunning);
            cancellationToken.Register(CancelWaitDeviceEvent);
        }

        #region MonitorDeviceEvent
        protected virtual Task ProcessingWaitDeviceEvent(Action<SKF_DeviceInOutEvent> deviceEventAction, CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                var waitEventResult = Provider.SKF_WaitForDeviceEvent();

                if (waitEventResult.IsSuccess())
                {
                    var deviceEvent = waitEventResult.Data;

                    var deviceInOutEvent = new SKF_DeviceInOutEvent
                    {
                        ProviderName = Provider.ProviderName,
                        DeviceName = deviceEvent.DeviceName,
                        EventType = deviceEvent.EventType
                    };

                    deviceEventAction?.Invoke(deviceInOutEvent);
                }
            }

            return Task.FromResult(0);
        }

        protected virtual void CancelWaitDeviceEvent()
        {
            Provider.SKF_CancelWaitForDeviceEvent();
        }
        #endregion

        #region Helper Functions
        protected static string FormatDeviceVersion(SKF_Version version) => $"{version.Major}.{version.Minor}";
        #endregion
    }
}
