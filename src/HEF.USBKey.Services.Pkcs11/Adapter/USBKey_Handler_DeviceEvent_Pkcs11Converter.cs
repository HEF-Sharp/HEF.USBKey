using HEF.USBKey.Common;
using System;

namespace HEF.USBKey.Services.Pkcs11
{
    public class USBKey_Handler_DeviceEvent_Pkcs11Converter : IUSBKey_Pkcs11_Handler_SlotEvent
    {
        public USBKey_Handler_DeviceEvent_Pkcs11Converter(IUSBKey_Handler_DeviceEvent usbKeyDeviceEventHandler)
        {
            USBKeyDeviceEventHandler = usbKeyDeviceEventHandler ?? throw new ArgumentNullException(nameof(usbKeyDeviceEventHandler));
        }

        protected IUSBKey_Handler_DeviceEvent USBKeyDeviceEventHandler { get; }

        public void Handle_SlotInOutEvent(Pkcs11_SlotInOutEvent slotInOutEvent, IUSBKeyService_Pkcs11 usbKeyPkcs11Service)
        {
            var usbKeyDeviceEvent = new USBKey_DeviceInOutEvent
            {
                ProviderName = slotInOutEvent.ProviderName,
                DeviceIdOrName = slotInOutEvent.SlotId.ToString(),
                InOutEventType = slotInOutEvent.InOutEventType
            };

            var usbKeyService = new USBKeyService_Pkcs11Adapter(usbKeyPkcs11Service);

            USBKeyDeviceEventHandler.Handle_DeviceInOutEvent(usbKeyDeviceEvent, usbKeyService);
        }
    }
}
