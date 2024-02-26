using HEF.USBKey.Common;
using HEF.USBKey.Interop.SKF;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKFAdapter : IUSBKeyService
    {
        public USBKeyService_SKFAdapter(IUSBKeyService_SKF usbKeySKFService)
        {
            USBKeySKFService = usbKeySKFService ?? throw new ArgumentNullException(nameof(usbKeySKFService));
        }

        protected IUSBKeyService_SKF USBKeySKFService { get; }

        public IEnumerable<USBKey_PresentDevice> GetPresentDevices()
        {
            var presentDevices = USBKeySKFService.GetPresentDevices();

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

        public USBKey_PresentDevice GetPresentDevice(string deviceIdOrName)
        {
            var presentDevice = USBKeySKFService.GetPresentDevice(deviceIdOrName);

            return new USBKey_PresentDevice
            {
                ProviderName = presentDevice.ProviderName,
                DeviceIdOrName = presentDevice.DeviceName,
                Manufacturer = presentDevice.Manufacturer,
                SerialNumber = presentDevice.SerialNumber,
                HardwareVersion = presentDevice.HWVersion,
                FirmwareVersion = presentDevice.FirmwareVersion
            };
        }

        public IEnumerable<USBKey_Certificate> ExportDeviceCertificates(string deviceIdOrName)
        {
            return USBKeySKFService.ExportDeviceCertificates(deviceIdOrName);
        }

        public bool ChangeDevicePIN(string deviceIdOrName, string oldPin, string newPin)
        {
            var result = USBKeySKFService.ChangeDeviceDefaultAppPIN(deviceIdOrName, oldPin, newPin);

            return result.IsSuccess();
        }

        public bool VerifyDevicePIN(string deviceIdOrName, string pin)
        {
            var result = USBKeySKFService.VerifyDeviceDefaultAppPIN(deviceIdOrName, pin);

            return result.IsSuccess();
        }

        public void StartMonitorDeviceEvent(Action<USBKey_DeviceInOutEvent> deviceEventAction, CancellationToken cancellationToken)
        {
            Action<SKF_DeviceInOutEvent> deviceEventAction_SKF = (deviceInOutEvent_SKF) =>
            {
                var usbKeyDeviceEvent = new USBKey_DeviceInOutEvent
                {
                    ProviderName = deviceInOutEvent_SKF.ProviderName,
                    DeviceIdOrName = deviceInOutEvent_SKF.DeviceName,
                    InOutEventType = deviceInOutEvent_SKF.InOutEventType
                };

                deviceEventAction?.Invoke(usbKeyDeviceEvent);
            };

            USBKeySKFService.StartMonitorDeviceEvent(deviceEventAction_SKF, cancellationToken);
        }
    }
}
