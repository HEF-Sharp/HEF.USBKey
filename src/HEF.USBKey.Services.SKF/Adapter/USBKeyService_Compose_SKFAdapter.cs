using HEF.USBKey.Common;
using HEF.USBKey.Interop.SKF;
using System;
using System.Collections.Generic;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_Compose_SKFAdapter : IUSBKeyService_Compose
    {
        public USBKeyService_Compose_SKFAdapter(IUSBKeyService_SKF_Compose composeUSBKeySKFService)
        {
            ComposeUSBKeySKFService = composeUSBKeySKFService ?? throw new ArgumentNullException(nameof(composeUSBKeySKFService));
        }

        protected IUSBKeyService_SKF_Compose ComposeUSBKeySKFService { get; }

        public IEnumerable<USBKey_PresentDevice> GetPresentDevices()
        {
            var presentDevices = ComposeUSBKeySKFService.GetPresentDevices();

            foreach (var presentDevice in presentDevices)
            {
                yield return new USBKey_PresentDevice
                {
                    ProviderName = presentDevice.ProviderName,
                    DeviceIdOrName = presentDevice.DeviceName,
                    Manufacturer = presentDevice.Manufacturer,
                    SerialNumber = presentDevice.SerialNumber,
                    HardwareVersion = presentDevice.HWVersion,
                    FirmwareVersion = presentDevice.FirmwareVersion
                };
            }
        }

        public IEnumerable<USBKey_Certificate_X509> ExportDeviceCertificates(string providerName, string deviceIdOrName)
        {
            return ComposeUSBKeySKFService.ExportDeviceCertificates(providerName, deviceIdOrName);
        }

        public bool ChangeDevicePIN(string providerName, string deviceIdOrName, string oldPin, string newPin)
        {
            var result = ComposeUSBKeySKFService.ChangeDeviceDefaultAppPIN(providerName, deviceIdOrName, oldPin, newPin);

            return result.IsSuccess();
        }

        public bool VerifyDevicePIN(string providerName, string deviceIdOrName, string pin)
        {
            var result = ComposeUSBKeySKFService.VerifyDeviceDefaultAppPIN(providerName, deviceIdOrName, pin);

            return result.IsSuccess();
        }

        public void StartMonitorDeviceEvent()
        {
            ComposeUSBKeySKFService.StartMonitorDeviceEvent();
        }

        public void AttachDeviceEventHandlers(params IUSBKey_Handler_DeviceEvent[] usbKeyDeviceEventHandlers)
        {
            var usbKeySKFDeviceEventHandlers = new List<IUSBKey_SKF_Handler_DeviceEvent>();

            foreach (var deviceEventHandler in usbKeyDeviceEventHandlers)
            {
                usbKeySKFDeviceEventHandlers.Add(new USBKey_Handler_DeviceEvent_SKFConverter(deviceEventHandler));
            }

            ComposeUSBKeySKFService.AttachDeviceEventHandlers(usbKeySKFDeviceEventHandlers.ToArray());
        }

        public void CancelMonitorDeviceEvent()
        {
            ComposeUSBKeySKFService.CancelMonitorDeviceEvent();
        }
    }
}
