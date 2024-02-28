using HEF.USBKey.Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HEF.USBKey.Services.Pkcs11
{
    public class USBKeyService_Pkcs11Adapter : IUSBKeyService
    {
        public USBKeyService_Pkcs11Adapter(IUSBKeyService_Pkcs11 usbKeyPkcs11Service)
        {
            USBKeyPkcs11Service = usbKeyPkcs11Service ?? throw new ArgumentNullException(nameof(usbKeyPkcs11Service));
        }

        protected IUSBKeyService_Pkcs11 USBKeyPkcs11Service { get; }

        public IEnumerable<USBKey_PresentDevice> GetPresentDevices()
        {
            var presentSlots = USBKeyPkcs11Service.GetPresentSlotList();

            foreach (var presentSlot in presentSlots)
            {
                var slotTokenInfo = presentSlot.GetTokenInfo();

                yield return new USBKey_PresentDevice
                {
                    ProviderName = USBKeyPkcs11Service.Provider.ProviderName,
                    DeviceIdOrName = presentSlot.SlotId.ToString(),
                    Manufacturer = slotTokenInfo.ManufacturerId,
                    SerialNumber = slotTokenInfo.SerialNumber,
                    HardwareVersion = slotTokenInfo.HardwareVersion,
                    FirmwareVersion = slotTokenInfo.FirmwareVersion
                };
            }
        }

        public USBKey_PresentDevice GetPresentDevice(string deviceIdOrName)
        {
            var slotId = Convert.ToUInt64(deviceIdOrName);
            var presentSlot = USBKeyPkcs11Service.GetPresentSlotById(slotId);

            var slotTokenInfo = presentSlot.GetTokenInfo();

            return new USBKey_PresentDevice
            {
                ProviderName = USBKeyPkcs11Service.Provider.ProviderName,
                DeviceIdOrName = presentSlot.SlotId.ToString(),
                Manufacturer = slotTokenInfo.ManufacturerId,
                SerialNumber = slotTokenInfo.SerialNumber,
                HardwareVersion = slotTokenInfo.HardwareVersion,
                FirmwareVersion = slotTokenInfo.FirmwareVersion
            };
        }

        public IEnumerable<USBKey_Certificate> ExportDeviceCertificates(string deviceIdOrName)
        {
            var slotId = Convert.ToUInt64(deviceIdOrName);

            return USBKeyPkcs11Service.ExportCertificates(slotId);
        }

        public IEnumerable<USBKey_Certificate_X509> ExportDeviceX509Certificates(string deviceIdOrName)
        {
            var slotId = Convert.ToUInt64(deviceIdOrName);

            return USBKeyPkcs11Service.ExportX509Certificates(slotId);
        }

        public bool ChangeDevicePIN(string deviceIdOrName, string oldPin, string newPin)
        {
            var slotId = Convert.ToUInt64(deviceIdOrName);

            return USBKeyPkcs11Service.ChangeTokenPIN(slotId, oldPin, newPin);
        }

        public bool VerifyDevicePIN(string deviceIdOrName, string pin)
        {
            var slotId = Convert.ToUInt64(deviceIdOrName);

            return USBKeyPkcs11Service.VerifyTokenPIN(slotId, pin);
        }

        public void StartMonitorDeviceEvent(Action<USBKey_DeviceInOutEvent> deviceEventAction, CancellationToken cancellationToken)
        {
            Action<Pkcs11_SlotInOutEvent> slotEventAction = (slotInOutEvent) =>
            {
                var usbKeyDeviceEvent = new USBKey_DeviceInOutEvent
                {
                    ProviderName = slotInOutEvent.ProviderName,
                    DeviceIdOrName = slotInOutEvent.SlotId.ToString(),
                    InOutEventType = slotInOutEvent.InOutEventType
                };

                deviceEventAction?.Invoke(usbKeyDeviceEvent);
            };

            USBKeyPkcs11Service.StartMonitorSlotEvent(slotEventAction, cancellationToken);
        }
    }
}
