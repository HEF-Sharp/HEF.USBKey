using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HEF.USBKey.Services.SKF
{
    public class USBKey_SKF_Handler_DeviceEvent_LocalCertSync : IUSBKey_SKF_Handler_DeviceEvent
    {
        public USBKey_SKF_Handler_DeviceEvent_LocalCertSync(IUSBKeyService_SKF_LocalCertStore usbKeyLocalCertStoreService)
        {
            USBKeyLocalCertStoreService = usbKeyLocalCertStoreService ?? throw new ArgumentNullException(nameof(usbKeyLocalCertStoreService));
        }

        protected IUSBKeyService_SKF_LocalCertStore USBKeyLocalCertStoreService { get; }

        public void Handle_DeviceInOutEvent(SKF_DeviceInOutEvent deviceInOutEvent, IUSBKeyService_SKF usbKeySKFService)
        {
            if (deviceInOutEvent.IsPlugIn)
                Handle_DevicePlugInEvent(deviceInOutEvent, usbKeySKFService);
            else if (deviceInOutEvent.IsPullOut)
                Handle_DevicePullOutEvent(deviceInOutEvent);
        }

        protected void Handle_DevicePlugInEvent(SKF_DeviceInOutEvent deviceInOutEvent, IUSBKeyService_SKF usbKeySKFService)
        {
            var deviceCerts = usbKeySKFService.ExportDeviceCertificates(deviceInOutEvent.DeviceName);
            
            var deviceCertCollection = BuildDeviceCertCollection(deviceCerts.ToArray());

            USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(deviceInOutEvent.ProviderName, deviceInOutEvent.DeviceName, deviceCertCollection);
        }

        protected void Handle_DevicePullOutEvent(SKF_DeviceInOutEvent deviceInOutEvent)
        {
            USBKeyLocalCertStoreService.RemoveDeviceCertsFromLocalCurrentUser(deviceInOutEvent.ProviderName, deviceInOutEvent.DeviceName);
        }

        #region LocalCertSync
        protected X509Certificate2Collection BuildDeviceCertCollection(params SKF_Certificate[] deviceCerts)
        {
            var deviceCertCollection = new X509Certificate2Collection();

            foreach (var deviceCert in deviceCerts)
            {
                var x509Cert = new X509Certificate2(deviceCert.CertBytes);
                deviceCertCollection.Add(x509Cert);
            }

            return deviceCertCollection;
        }
        #endregion
    }
}
