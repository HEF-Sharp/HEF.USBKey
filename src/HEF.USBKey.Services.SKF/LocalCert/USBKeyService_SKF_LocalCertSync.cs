using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
                
                var deviceCertCollection = BuildDeviceCertCollection(deviceX509Certs.ToArray());

                USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(presentDevice.ProviderName, presentDevice.DeviceName, deviceCertCollection);
            }

            ComposeUSBKeySKFService.StartMonitorDeviceEvent(USBKeyLocalCertDeviceEventHandler);
        }

        public void CancelSyncUSBKeysCertsToLocalCurrentUser()
        {
            ComposeUSBKeySKFService.CancelMonitorDeviceEvent();

            USBKeyLocalCertStoreService.RemoveAllDeviceCertsFromLocalCurrentUser();
        }

        #region Helper Functions
        protected X509Certificate2Collection BuildDeviceCertCollection(params SKF_Certificate_X509[] deviceX509Certs)
        {
            var deviceCertCollection = new X509Certificate2Collection();

            deviceCertCollection.AddRange(deviceX509Certs.Select(m => m.X509Cert).ToArray());

            return deviceCertCollection;
        }
        #endregion
    }
}