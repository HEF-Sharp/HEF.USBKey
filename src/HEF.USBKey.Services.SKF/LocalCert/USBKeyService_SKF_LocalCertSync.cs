using System;
using System.Linq;

namespace HEF.USBKey.Services.SKF
{
    public class USBKeyService_SKF_LocalCertSync : IUSBKeyService_SKF_LocalCertSync
    {
        public USBKeyService_SKF_LocalCertSync(IUSBKeyService_SKF_Compose composeUSBKeySKFService,
            IUSBKeyService_SKF_LocalCertStore usbKeyLocalCertStoreService)
        {
            ComposeUSBKeySKFService = composeUSBKeySKFService ?? throw new ArgumentException(nameof(composeUSBKeySKFService));

            USBKeyLocalCertStoreService = usbKeyLocalCertStoreService ?? throw new ArgumentNullException(nameof(usbKeyLocalCertStoreService));

            USBKeyLocalCertDeviceEventHandler = new USBKey_SKF_Handler_DeviceEvent_LocalCertSync(usbKeyLocalCertStoreService);
        }

        protected IUSBKeyService_SKF_Compose ComposeUSBKeySKFService { get; }

        protected IUSBKeyService_SKF_LocalCertStore USBKeyLocalCertStoreService { get; }

        protected IUSBKey_SKF_Handler_DeviceEvent USBKeyLocalCertDeviceEventHandler { get; }

        public void StartSyncUSBKeysCertsToLocalCurrentUser()
        {
            var presentDevices = ComposeUSBKeySKFService.GetPresentDevices();

            foreach (var presentDevice in presentDevices)
            {
                var deviceX509Certs = ComposeUSBKeySKFService.ExportDeviceCertificates(presentDevice.ProviderName, presentDevice.DeviceName);

                USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(presentDevice.ProviderName, presentDevice.DeviceName, deviceX509Certs.ToArray());
            }

            ComposeUSBKeySKFService.AttachDeviceEventHandlers(USBKeyLocalCertDeviceEventHandler);
            ComposeUSBKeySKFService.StartMonitorDeviceEvent();
        }

        public void CancelSyncUSBKeysCertsToLocalCurrentUser()
        {
            ComposeUSBKeySKFService.CancelMonitorDeviceEvent();

            USBKeyLocalCertStoreService.RemoveAllDeviceCertsFromLocalCurrentUser();
        }
    }
}