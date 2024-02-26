using HEF.USBKey.Common;
using System;
using System.Collections.Generic;

namespace HEF.USBKey.Services.Pkcs11
{
    public class USBKeyService_Compose_Pkcs11Adapter : IUSBKeyService_Compose
    {
        public USBKeyService_Compose_Pkcs11Adapter(IUSBKeyService_Pkcs11_Compose composeUSBKeyPkcs11Service)
        {
            ComposeUSBKeyPkcs11Service = composeUSBKeyPkcs11Service ?? throw new ArgumentNullException(nameof(composeUSBKeyPkcs11Service));
        }

        protected IUSBKeyService_Pkcs11_Compose ComposeUSBKeyPkcs11Service { get; }

        public IEnumerable<USBKey_PresentDevice> GetPresentDevices()
        {
            var presentSlots = ComposeUSBKeyPkcs11Service.GetPresentSlotList();

            foreach (var presentSlot in presentSlots)
            {
                var slotTokenInfo = presentSlot.Slot.GetTokenInfo();

                yield return new USBKey_PresentDevice
                {
                    ProviderName = presentSlot.ProviderName,
                    DeviceIdOrName = presentSlot.Slot.SlotId.ToString(),
                    Manufacturer = slotTokenInfo.ManufacturerId,
                    SerialNumber = slotTokenInfo.SerialNumber,
                    HardwareVersion = slotTokenInfo.HardwareVersion,
                    FirmwareVersion = slotTokenInfo.FirmwareVersion
                };
            }
        }

        public IEnumerable<USBKey_Certificate_X509> ExportDeviceCertificates(string providerName, string deviceIdOrName)
        {
            var slotId = Convert.ToUInt64(deviceIdOrName);

            return ComposeUSBKeyPkcs11Service.ExportCertificates(providerName, slotId);
        }

        public bool ChangeDevicePIN(string providerName, string deviceIdOrName, string oldPin, string newPin)
        {
            var slotId = Convert.ToUInt64(deviceIdOrName);

            return ComposeUSBKeyPkcs11Service.ChangeTokenPIN(providerName, slotId, oldPin, newPin);
        }

        public bool VerifyDevicePIN(string providerName, string deviceIdOrName, string pin)
        {
            var slotId = Convert.ToUInt64(deviceIdOrName);

            return ComposeUSBKeyPkcs11Service.VerifyTokenPIN(providerName, slotId, pin);
        }

        public void StartMonitorDeviceEvent()
        {
            ComposeUSBKeyPkcs11Service.StartMonitorSlotEvent();
        }

        public void AttachDeviceEventHandlers(params IUSBKey_Handler_DeviceEvent[] usbKeyDeviceEventHandlers)
        {
            var usbKeyPkcs11SlotEventHandlers = new List<IUSBKey_Pkcs11_Handler_SlotEvent>();

            foreach (var deviceEventHandler in usbKeyDeviceEventHandlers)
            {
                usbKeyPkcs11SlotEventHandlers.Add(new USBKey_Handler_DeviceEvent_Pkcs11Converter(deviceEventHandler));
            }

            ComposeUSBKeyPkcs11Service.AttachSlotEventHandlers(usbKeyPkcs11SlotEventHandlers.ToArray());
        }

        public void CancelMonitorDeviceEvent()
        {
            ComposeUSBKeyPkcs11Service.CancelMonitorSlotEvent();
        }
    }
}
