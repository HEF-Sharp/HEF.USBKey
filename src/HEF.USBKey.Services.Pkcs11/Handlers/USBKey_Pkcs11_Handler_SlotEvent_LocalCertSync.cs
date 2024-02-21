using HEF.USBKey.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HEF.USBKey.Services.Pkcs11
{
    public class USBKey_Pkcs11_Handler_SlotEvent_LocalCertSync : IUSBKey_Pkcs11_Handler_SlotEvent
    {
        public USBKey_Pkcs11_Handler_SlotEvent_LocalCertSync(IUSBKeyService_LocalCertStore usbKeyLocalCertStoreService)
        {
            USBKeyLocalCertStoreService = usbKeyLocalCertStoreService ?? throw new ArgumentNullException(nameof(usbKeyLocalCertStoreService));
        }

        protected IUSBKeyService_LocalCertStore USBKeyLocalCertStoreService { get; }

        public void Handle_SlotInOutEvent(Pkcs11_SlotInOutEvent slotInOutEvent, IUSBKeyService_Pkcs11 usbKeyPkcs11Service)
        {
            if (slotInOutEvent.InOutEventType == DeviceEventTypes.PlugIn)
                Handle_SlotPlugInEvent(slotInOutEvent, usbKeyPkcs11Service);
            else if (slotInOutEvent.InOutEventType == DeviceEventTypes.PullOut)
                Handle_SlotPullOutEvent(slotInOutEvent);
        }

        protected void Handle_SlotPlugInEvent(Pkcs11_SlotInOutEvent slotInOutEvent, IUSBKeyService_Pkcs11 usbKeyPkcs11Service)
        {
            var slotCerts = usbKeyPkcs11Service.ExportCertificates(slotInOutEvent.SlotId);

            var slotX509Certs = BuildSlotX509Certs(usbKeyPkcs11Service, slotCerts.ToArray());

            USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(slotInOutEvent.ProviderName, slotInOutEvent.SlotId.ToString(), slotX509Certs.ToArray());
        }

        protected void Handle_SlotPullOutEvent(Pkcs11_SlotInOutEvent slotInOutEvent)
        {
            USBKeyLocalCertStoreService.RemoveDeviceCertsFromLocalCurrentUser(slotInOutEvent.ProviderName, slotInOutEvent.SlotId.ToString());
        }

        #region Helper Functions
        protected static IEnumerable<Pkcs11_Certificate_X509> BuildSlotX509Certs(
            IUSBKeyService_Pkcs11 usbKeyPkcs11Service, params Pkcs11_Certificate[] slotCerts)
        {
            foreach (var slotCert in slotCerts)
            {
                yield return slotCert.BuildX509Certificate(usbKeyPkcs11Service);
            }
        }
        #endregion
    }
}
