using System;
using System.Linq;

namespace HEF.USBKey.Common
{
    public class USBKey_Handler_DeviceEvent_LocalCertSync : IUSBKey_Handler_DeviceEvent
    {
        public USBKey_Handler_DeviceEvent_LocalCertSync(IUSBKeyService_LocalCertStore usbKeyLocalCertStoreService)
        {
            USBKeyLocalCertStoreService = usbKeyLocalCertStoreService ?? throw new ArgumentNullException(nameof(usbKeyLocalCertStoreService));
        }

        protected IUSBKeyService_LocalCertStore USBKeyLocalCertStoreService { get; }

        public void Handle_DeviceInOutEvent(USBKey_DeviceInOutEvent deviceInOutEvent, IUSBKeyService usbKeyService)
        {
            if (deviceInOutEvent.InOutEventType == DeviceEventTypes.PlugIn)
                Handle_DevicePlugInEvent(deviceInOutEvent, usbKeyService);
            else if (deviceInOutEvent.InOutEventType == DeviceEventTypes.PullOut)
                Handle_DevicePullOutEvent(deviceInOutEvent);
        }

        protected void Handle_DevicePlugInEvent(USBKey_DeviceInOutEvent deviceInOutEvent, IUSBKeyService usbKeyService)
        {
            var deviceX509Certs = usbKeyService.ExportDeviceX509Certificates(deviceInOutEvent.DeviceIdOrName);            

            USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(deviceInOutEvent.ProviderName, deviceInOutEvent.DeviceIdOrName, deviceX509Certs.ToArray());
        }

        protected void Handle_DevicePullOutEvent(USBKey_DeviceInOutEvent deviceInOutEvent)
        {
            USBKeyLocalCertStoreService.RemoveDeviceCertsFromLocalCurrentUser(deviceInOutEvent.ProviderName, deviceInOutEvent.DeviceIdOrName);
        }
    }
}
