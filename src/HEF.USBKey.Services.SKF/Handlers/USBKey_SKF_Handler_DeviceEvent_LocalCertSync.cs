using HEF.USBKey.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;

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
            var deviceCerts = usbKeySKFService.ExportDeviceCertificates(deviceInOutEvent.DeviceName);
            
            var deviceX509Certs = BuildDeviceX509Certs(deviceCerts.ToArray());

            USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(deviceInOutEvent.ProviderName, deviceInOutEvent.DeviceName, deviceX509Certs.ToArray());
        }

        protected void Handle_DevicePullOutEvent(SKF_DeviceInOutEvent deviceInOutEvent)
        {
            USBKeyLocalCertStoreService.RemoveDeviceCertsFromLocalCurrentUser(deviceInOutEvent.ProviderName, deviceInOutEvent.DeviceName);
        }

        #region Helper Functions
        protected static IEnumerable<SKF_Certificate_X509> BuildDeviceX509Certs(params SKF_Certificate[] deviceCerts)
        {
            foreach (var deviceCert in deviceCerts)
            {
                yield return new SKF_Certificate_X509
                {
                    ForSign = deviceCert.ForSign,
                    CertBytes = deviceCert.CertBytes,
                    X509Cert = new X509Certificate2(deviceCert.CertBytes, (SecureString)null, X509KeyStorageFlags.UserKeySet)
                };
            }
        }
        #endregion
    }
}
