using HEF.USBKey.Common;
using System;

namespace HEF.USBKey.Services.SKF
{
    public class USBKey_Handler_DeviceEvent_SKFConverter : IUSBKey_SKF_Handler_DeviceEvent
    {
        public USBKey_Handler_DeviceEvent_SKFConverter(IUSBKey_Handler_DeviceEvent usbKeyDeviceEventHandler)
        {
            USBKeyDeviceEventHandler = usbKeyDeviceEventHandler ?? throw new ArgumentNullException(nameof(usbKeyDeviceEventHandler));
        }

        protected IUSBKey_Handler_DeviceEvent USBKeyDeviceEventHandler { get; }

        public void Handle_DeviceInOutEvent(SKF_DeviceInOutEvent deviceInOutEvent, IUSBKeyService_SKF usbKeySKFService)
        {
            var usbKeyDeviceEvent = new USBKey_DeviceInOutEvent
            {
                ProviderName = deviceInOutEvent.ProviderName,
                DeviceIdOrName = deviceInOutEvent.DeviceName,
                InOutEventType = deviceInOutEvent.InOutEventType
            };

            var usbKeyService = new USBKeyService_SKFAdapter(usbKeySKFService);

            USBKeyDeviceEventHandler.Handle_DeviceInOutEvent(usbKeyDeviceEvent, usbKeyService);
        }
    }
}
