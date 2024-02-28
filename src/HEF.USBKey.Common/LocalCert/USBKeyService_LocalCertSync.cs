using System;
using System.Linq;

namespace HEF.USBKey.Common
{
    public class USBKeyService_LocalCertSync : IUSBKeyService_LocalCertSync
    {
        public USBKeyService_LocalCertSync(IUSBKeyService_Compose composeUSBKeyService,
            IUSBKeyService_LocalCertStore usbKeyLocalCertStoreService)
        {
            ComposeUSBKeyService = composeUSBKeyService ?? throw new ArgumentException(nameof(composeUSBKeyService));

            USBKeyLocalCertStoreService = usbKeyLocalCertStoreService ?? throw new ArgumentNullException(nameof(usbKeyLocalCertStoreService));

            USBKeyLocalCertDeviceEventHandler = new USBKey_Handler_DeviceEvent_LocalCertSync(usbKeyLocalCertStoreService);
        }

        protected IUSBKeyService_Compose ComposeUSBKeyService { get; }

        protected IUSBKeyService_LocalCertStore USBKeyLocalCertStoreService { get; }

        protected IUSBKey_Handler_DeviceEvent USBKeyLocalCertDeviceEventHandler { get; }

        public void StartSyncUSBKeysCertsToLocalCurrentUser()
        {
            var presentDevices = ComposeUSBKeyService.GetPresentDevices();

            foreach (var presentDevice in presentDevices)
            {
                var deviceX509Certs = ComposeUSBKeyService.ExportDeviceCertificates(presentDevice.ProviderName, presentDevice.DeviceIdOrName);

                USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(presentDevice.ProviderName, presentDevice.DeviceIdOrName, deviceX509Certs.ToArray());
            }

            ComposeUSBKeyService.AttachDeviceEventHandlers(USBKeyLocalCertDeviceEventHandler);
            ComposeUSBKeyService.StartMonitorDeviceEvent();
        }

        public void CancelSyncUSBKeysCertsToLocalCurrentUser()
        {
            ComposeUSBKeyService.CancelMonitorDeviceEvent();

            USBKeyLocalCertStoreService.RemoveAllDeviceCertsFromLocalCurrentUser();
        }
    }
}
