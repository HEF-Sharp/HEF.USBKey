using HEF.USBKey.Common;
using System;
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
            var slotX509Certs = usbKeyPkcs11Service.ExportX509Certificates(slotInOutEvent.SlotId);            

            USBKeyLocalCertStoreService.AddDeviceCertsToLocalCurrentUser(slotInOutEvent.ProviderName, slotInOutEvent.SlotId.ToString(), slotX509Certs.ToArray());
        }

        protected void Handle_SlotPullOutEvent(Pkcs11_SlotInOutEvent slotInOutEvent)
        {
            USBKeyLocalCertStoreService.RemoveDeviceCertsFromLocalCurrentUser(slotInOutEvent.ProviderName, slotInOutEvent.SlotId.ToString());
        }
    }
}
