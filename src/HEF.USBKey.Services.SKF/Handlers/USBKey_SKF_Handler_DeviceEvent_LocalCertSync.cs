using HEF.USBKey.Common;
using System;
using System.Linq;

namespace HEF.USBKey.Services.SKF
{
    public class USBKey_SKF_Handler_DeviceEvent_LocalCertSync : IUSBKey_SKF_Handler_DeviceEvent
    {
        public USBKey_SKF_Handler_DeviceEvent_LocalCertSync(IUSBKeyService_LocalCertStore usbKeyLocalCertStoreService)
        {
            USBKeyLocalCertStoreService = usbKeyLocalCertStoreService ?? throw new ArgumentNullException(nameof(usbKeyLocalCertStoreService));
        }

        protected IUSBKeyService_LocalCertStore USBKeyLocalCertStoreService { get; }

        public void Handle_DeviceInOutEvent(SKF_DeviceInOutEvent deviceInOutEvent, IUSBKeyService_SKF usbKeySKFService)
        {
            if (deviceInOutEvent.InOutEventType == DeviceEventTypes.PlugIn)
                Handle_DevicePlugInEvent(deviceInOutEvent, usbKeySKFService);
            else if (deviceInOutEvent.InOutEventType == DeviceEventTypes.PullOut)
                Handle_DevicePullOutEvent(deviceInOutEvent);
        }

        protected void Handle_DevicePlugInEvent(SKF_DeviceInOutEvent deviceInOutEvent, IUSBKeyService_SKF usbKeySKFService)
        {
            var deviceX509Certs = usbKeySKFService.ExportDeviceX509Certificates(deviceInOutEvent.DeviceName);

            USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(deviceInOutEvent.ProviderName, deviceInOutEvent.DeviceName, deviceX509Certs.ToArray());
        }

        protected void Handle_DevicePullOutEvent(SKF_DeviceInOutEvent deviceInOutEvent)
        {
            USBKeyLocalCertStoreService.RemoveDeviceCertsFromLocalCurrentUser(deviceInOutEvent.ProviderName, deviceInOutEvent.DeviceName);
        }
    }
}
